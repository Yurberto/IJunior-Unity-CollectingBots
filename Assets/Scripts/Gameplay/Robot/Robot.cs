using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(TriggerHandler))]
[RequireComponent(typeof(ResourceCollector))]
public class Robot : MonoBehaviour
{
    private Mover _mover;
    private TriggerHandler _triggerHandler;
    private ResourceCollector _resourceCollector;

    private Resource _currentResource;

    private Vector3 _startPosition;
    private bool _isWork = false;

    public event Action<Resource> ResourceDelivered;

    public bool IsWork => _isWork;

    public void Initialize(Vector3 startPosition)
    {
        _startPosition = startPosition;
        transform.position = _startPosition;
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _triggerHandler = GetComponent<TriggerHandler>();
        _resourceCollector = GetComponent<ResourceCollector>();
    }

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnEnable()
    {
        _triggerHandler.ResourceHitted += BackToBase;
    }

    private void OnDisable()
    {
        _triggerHandler.ResourceHitted -= BackToBase;
    }

    public void MoveTo(Vector3 target)
    {
        _isWork = true;
        _mover.MoveTo(target);
    }

    private void BackToBase(Resource resource)
    {
        if (_currentResource != null) 
            return;

        _currentResource = resource;

        PickUp();

        StartCoroutine(PutInOnBase());
    }

    private void PickUp()
    {
        _resourceCollector.PickUp(_currentResource);
        _mover.MoveTo(_startPosition);
    }

    private IEnumerator PutInOnBase()
    {
        float reachedDistance = 0.2f;

        yield return new WaitUntil(() => Vector3.Distance(transform.position, _startPosition) < reachedDistance);

        PutIn();
        _mover.Stop();
        transform.rotation = Quaternion.identity;
    }

    private void PutIn()
    {
        ResourceDelivered?.Invoke(_currentResource);

        _currentResource = null;
        _isWork = false;
    }
}
