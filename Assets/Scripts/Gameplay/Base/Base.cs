using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SpawnpointContainer _robotSpawnpointContainer;
    [SerializeField, Range(0.0f, 10.0f)] private float _workDelay = 0.5f;

    private List<Robot> _robots = new List<Robot>();    
    private ResourceMonitor _resourceMonitor = new ResourceMonitor();

    private RobotSpawner _robotSpawner;
    private List<Vector3> _availableRobotSpawnpoints;

    private Hub<Resource> _resourceHub;

    private CancellationTokenSource _cancellationTokenSource;

    public event Action<int> ResourceValueChanged;

    public void Initialize(RobotSpawner robotSpawner, Hub<Resource> resourceHub)
    {
        _robotSpawner = robotSpawner;
        _resourceHub = resourceHub;
    }

    private void Awake()
    {
        _availableRobotSpawnpoints = _robotSpawnpointContainer.Spawnpoints;
    }

    private void Start()
    {
        CreateRobot();
    }

    private void OnEnable()
    {
        StartWork().Forget();

        _resourceMonitor.CountChanged += OnResourceCountChanged;
    }

    private void OnDisable()
    {
        StopWork();

        for (int i = 0; i < _robots.Count; i++)
        {
            _robots[i].ResourceDelivered -= OnResourceDelivered;
        }

        _resourceMonitor.CountChanged -= OnResourceCountChanged;
    }

    private async UniTaskVoid StartWork()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        while (enabled)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_workDelay), cancellationToken: _cancellationTokenSource.Token);

            if (TryGetRobot(out Robot robot))
            {
                if (_resourceHub.HasAvailable)
                {
                    robot.GoPickUp(_resourceHub.GetAvailable());
                }
            }
        }
    }

    private void StopWork()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    private void CreateRobot()
    {
        Robot spawned = _robotSpawner.Spawn();
        spawned.Initialize(SpawnUtils.GetSpawnPosition(_availableRobotSpawnpoints));
        _robots.Add(spawned);
        
        spawned.ResourceDelivered += OnResourceDelivered;
    }

    private bool TryGetRobot(out Robot robot)
    {
        for (int i = 0; i < _robots.Count; i++)
        {
            if (_robots[i].IsWork == false)
            {
                robot = _robots[i];
                return true;
            }
        }

        robot = null;
        return false;
    }

    private void OnResourceDelivered(Robot deliveryRobot, Resource resource)
    {
        resource.InvokeRelease();
        _resourceHub.Remove(resource);
        _resourceMonitor.AddResource();
    }

    private void OnResourceCountChanged(int value)
    {
        ResourceValueChanged?.Invoke(value);
    }
}
