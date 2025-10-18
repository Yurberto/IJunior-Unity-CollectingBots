using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SpawnpointContainer _robotSpawnpointContainer;
    [SerializeField, Range(0.0f, 10.0f)] private float _workDelay = 3f;

    private List<Robot> _robots = new List<Robot>();
    private RobotSpawner _robotSpawner;

    private List<Vector3> _availableRobotSpawnpoints;

    private Hub<Resource> _resourceHub;
    private ResourceMonitor _resourceMonitor = new ResourceMonitor();

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
        CreateRobot();
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

        foreach (Robot robot in _robots)
        {
            robot.ResourceDelivered -= CollectResource;
        }

        _resourceMonitor.CountChanged -= OnResourceCountChanged;
    }

    private void CreateRobot()
    {
        Robot spawned = _robotSpawner.Spawn();
        spawned.Initialize(SpawnUtils.GetSpawnPosition(_availableRobotSpawnpoints));
        _robots.Add(spawned);
        Debug.Log("Spawn Robot");

        spawned.ResourceDelivered += CollectResource;
    }

    private void SendRobot(Resource resource)
    {
        for (int i = 0; i < _robots.Count; i++)
        {
            if (_robots[i].IsWork == false)
            {
                _robots[i].GoPickUp(resource);
                break;
            }
        }
    }

    private void CollectResource(Resource resource)
    {
        _resourceMonitor.AddResource();

        resource.InvokeRelease();
    }

    private void OnResourceCountChanged(int value)
    {
        ResourceValueChanged?.Invoke(value);
    }

    private async UniTaskVoid StartWork()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        while (enabled)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_workDelay), cancellationToken: _cancellationTokenSource.Token);

            if (_resourceHub.HasAvailable)
                SendRobot(_resourceHub.GetAvailable());
        }
    }

    private void StopWork()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }
}
