using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(ResourceDeliverer))]
public class Robot : MonoBehaviour
{
    [SerializeField, Range(0.0f, 5.0f)] private float _reachedDistance = 1.0f;

    [SerializeField] private Mover _mover;
    [SerializeField] private ResourceDeliverer _resourceDeliverer;

    private Resource _currentResource;

    private Vector3 _startPosition;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public event Action<Robot, Resource> ResourceDelivered;

    public Vector3 BasePosition => _startPosition;

    public void Initialize(Vector3 positionOnBase)
    {
        _currentResource = null;

        _startPosition = positionOnBase;
        transform.position = _startPosition;
    }

    private void OnDisable()
    {
        if (_cancellationTokenSource == null)
            return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    public async UniTask GoToFlag(Flag flag)
    {
        _mover.MoveTo(flag.transform.position);

        await UniTask.WaitUntil(() => IsOnPosiotion(flag.transform.position), cancellationToken: _cancellationTokenSource.Token);
        _mover.Stop();
    }

    public void GoPickUp(Resource target)
    {
        _currentResource = target;
        _mover.MoveTo(target.transform.position);

        PickUpAsync().Forget();
    }

    private async UniTaskVoid PickUpAsync()
    {
        if (_currentResource == null)
            Debug.Log("NULL_RESOURCE");
        if (_resourceDeliverer == null)
            Debug.Log("NULL_Delivirer");

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
        ResourceDelivered?.Invoke(this, _currentResource);
        _currentResource = null;

        transform.rotation = Quaternion.identity;
        transform.position = _startPosition;
    }

    private bool IsOnPosiotion(Vector3 position)
    {
        Vector2 position2D = new Vector2(position.x, position.z);
        Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.z);

        return Vector3.Distance(transformPosition2D, position2D) < _reachedDistance;
    }
}
