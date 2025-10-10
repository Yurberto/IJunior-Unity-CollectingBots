using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(ResourceCollector))]
[RequireComponent(typeof(TriggerHandler))]
public class Robot : MonoBehaviour
{
    private Mover _mover;
    private ResourceCollector _resourceCollector;
    private TriggerHandler _resourceDetector;

    private Vector3 _startPosition;
    private bool _isWork = false;

    public bool IsWork => _isWork;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _resourceCollector = GetComponent<ResourceCollector>();
        _resourceDetector = GetComponent<TriggerHandler>();

        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        _resourceDetector.AvailableResourceHitted += Collect;
        _resourceDetector.SpawnTrggierHitted += _mover.Stop;
    }

    private void OnDisable()
    {
        _resourceDetector.AvailableResourceHitted -= Collect;
        _resourceDetector.SpawnTrggierHitted -= _mover.Stop;
    }

    public void MoveTo(Vector3 target)
    {
        _isWork = true;
        _mover.MoveTo(target);
    }

    private void Collect(Resource resource)
    {
        _mover.Stop();
        _resourceCollector.Collect(resource);
        GoToStartPosition();
    }

    private void GoToStartPosition()
    {
        _mover.MoveTo(_startPosition);
    }
}
