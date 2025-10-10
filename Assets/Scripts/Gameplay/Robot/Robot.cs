using System;
using UnityEngine;

[RequireComponent(typeof(RobotMover))]
[RequireComponent(typeof(ResourceCollector))]
[RequireComponent(typeof(ResourceDetector))]
public class Robot : MonoBehaviour
{
    private RobotMover _robotMover;
    private ResourceCollector _resourceCollector;
    private ResourceDetector _resourceDetector;

    private bool _isWork = false;

    public event Action ResourceCollected;

    public bool IsWork => _isWork;

    private void Awake()
    {
        _robotMover = GetComponent<RobotMover>();
        _resourceCollector = GetComponent<ResourceCollector>();
        _resourceDetector = GetComponent<ResourceDetector>();
    }

    private void OnEnable()
    {
        _resourceDetector.AvailableResourceHitted += Collect;
    }

    private void OnDisable()
    {
        _resourceDetector.AvailableResourceHitted -= Collect;
    }

    public void MoveTo(Transform target)
    {
        _isWork = true;
        _robotMover.MoveTo(target);
    }

    private void Collect(Resource resource)
    {
        _resourceCollector.Collect(resource);
        ResourceCollected?.Invoke();
    }
}
