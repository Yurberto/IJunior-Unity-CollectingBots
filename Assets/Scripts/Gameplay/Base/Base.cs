using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SpawnpointContainer _robotSpawnpointContainer;

    [SerializeField, Range(0.0f, 10.0f)] private float _workDelay = 0.5f;   
    [SerializeField, Range(0.0f, 10.0f)] private float _checkMoneyAmountDelay = 0.5f;

    private List<Robot> _robots = new List<Robot>();
    private ResourceMonitor _resourceMonitor;

    private RobotSpawner _robotSpawner;
    private Hub<Resource> _availableResources;

    private Hub<Robot> _availableRobots;
    private Hub<Vector3> _availableRobotSpawnpoints;

    private CancellationTokenSource _cancellationTokenSource;

    public event Action<int> ResourceValueChanged;

    public void Initialize(RobotSpawner robotSpawner, Hub<Resource> availableResources)
    {
        _robotSpawner = robotSpawner;
        _availableResources = availableResources;
    }

    private void Awake()
    {
        _availableRobots = new Hub<Robot>(_robots);
        _availableRobotSpawnpoints = new Hub<Vector3>(_robotSpawnpointContainer.Spawnpoints);

        _resourceMonitor = new ResourceMonitor(_checkMoneyAmountDelay);
    }

    private void Start()
    {
        _resourceMonitor.AddResource();
        _resourceMonitor.AddResource();
        _resourceMonitor.AddResource();
        SpawnRobot();
    }

    private void OnEnable()
    {
        StartWork().Forget();
        _resourceMonitor.CheckMoneyAmountAsync().Forget();

        _resourceMonitor.CreateRobotAvailable += SpawnRobot;
        _resourceMonitor.Count.Changed += OnResourceCountChanged;
    }

    private void OnDisable()
    {
        StopWork();
        _resourceMonitor.StopCheckMoneyAmount();

        for (int i = 0; i < _robots.Count; i++)
            _robots[i].ResourceDelivered -= OnResourceDelivered;

        _resourceMonitor.CreateRobotAvailable -= SpawnRobot;
        _resourceMonitor.Count.Changed -= OnResourceCountChanged;
    }

    private async UniTaskVoid StartWork()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        while (enabled)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_workDelay), cancellationToken: _cancellationTokenSource.Token);

            if (_availableRobots.IsEmpty ||  _availableResources.IsEmpty)
                continue;

            Robot robot = _availableRobots.GetRandom();
            Resource resource = _availableResources.GetRandom();

            robot.GoPickUp(resource);
        }
    }

    private void StopWork()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    private void SpawnRobot()
    {
        if (_availableRobotSpawnpoints.IsEmpty)
            return;

        if (_resourceMonitor.TrySpend(CreateCostData.RobotCost) == false)
            return;

        Robot spawned = _robotSpawner.Spawn();
        spawned.Initialize(_availableRobotSpawnpoints.GetRandom());

        _robots.Add(spawned);
        _availableRobots.Add(spawned);
        
        spawned.ResourceDelivered += OnResourceDelivered;
    }

    private void OnResourceDelivered(Robot deliveryRobot, Resource resource)
    {
        _availableRobots.Add(deliveryRobot);

        resource.InvokeRelease();
        _resourceMonitor.AddResource();
    }

    private void OnResourceCountChanged(int value)
    {
        ResourceValueChanged?.Invoke(value);
    }
}
