using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(ResourceDeliverer))]
public class Robot : MonoBehaviour
{
    [SerializeField, Range(0.0f, 5.0f)] private float _reachedDistance = 1.0f;

    private Mover _mover;
    private ResourceDeliverer _resourceDeliverer;

    private Resource _currentResource;

    private Vector3 _startPosition;
    private bool _isWork = false;

    private CancellationTokenSource _cancellationTokenSource;

    public event Action<Robot, Resource> ResourceDelivered;

    public bool IsWork => _isWork;

    public void Initialize(Vector3 startPosition)
    {
        _startPosition = startPosition;
        transform.position = _startPosition;
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _resourceDeliverer = GetComponent<ResourceDeliverer>();
    }

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        if (_cancellationTokenSource == null)
            return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    public void GoPickUp(Resource target)
    {
        _isWork = true;
        _currentResource = target;
        _mover.MoveTo(target.transform.position);

        PickUpAsync().Forget();
    }

    private async UniTaskVoid PickUpAsync()
    {
        await UniTask.WaitUntil(() => IsOnPosiotion(_currentResource.SpawnPosition), cancellationToken: _cancellationTokenSource.Token);

        _resourceDeliverer.PickUp(_currentResource);
        BackToBase();
    }

    private void BackToBase()
    {
        _resourceDeliverer.PickUp(_currentResource);
        _mover.MoveTo(_startPosition);
        
        PutInOnBase().Forget();
    }

    private async UniTaskVoid PutInOnBase()
    {
        await UniTask.WaitUntil(() => IsOnPosiotion(_startPosition), cancellationToken: _cancellationTokenSource.Token);

        PutIn();
        _mover.Stop();
        transform.rotation = Quaternion.identity;
    }

    private void PutIn()
    {
        _isWork = false;
        ResourceDelivered?.Invoke(this, _currentResource);
        Debug.Log("REsourceDelivered");

        _currentResource = null;
    }

    private bool IsOnPosiotion(Vector3 position)
    {
        Vector2 position2D = new Vector2(position.x, position.z);
        Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.z);

        return Vector3.Distance(transformPosition2D, position2D) < _reachedDistance;
    }
}
