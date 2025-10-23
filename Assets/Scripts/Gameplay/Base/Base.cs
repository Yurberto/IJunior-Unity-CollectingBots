using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _robotSpawnpointContainer;

    [SerializeField, Range(0.0f, 10.0f)] private float _workDelay = 0.5f;   
    [SerializeField, Range(0.0f, 10.0f)] private float _checkMoneyAmountDelay = 0.5f;

    private List<Robot> _robots = new List<Robot>();
    private ResourceMonitor _resourceMonitor;

    private RobotSpawner _robotSpawner;
    private Hub<Resource> _availableResources;

    private Hub<Robot> _availableRobots = new Hub<Robot>();
    private Hub<Vector3> _availableRobotSpawnpoints = new Hub<Vector3>();

    private CancellationTokenSource _cancellationTokenSource;

    public void Initialize(Robot startRobot, RobotSpawner robotSpawner, Hub<Resource> availableResources)
    {
        InitStartRobot(startRobot);
        _robotSpawner = robotSpawner;
        _availableResources = availableResources;
    }

    private void Awake()
    {
        for (int i = 0; i < _robotSpawnpointContainer.childCount; i++)
            _availableRobotSpawnpoints.Add(_robotSpawnpointContainer.GetChild(i).position);

        _resourceMonitor = new ResourceMonitor(_checkMoneyAmountDelay);
    }

    private void OnEnable()
    {
        StartWork().Forget();
        _resourceMonitor.CheckMoneyAmountAsync().Forget();

        _resourceMonitor.CreateRobotAvailable += SpawnRobot;
    }

    private void OnDisable()
    {
        StopWork();
        _resourceMonitor.StopCheckMoneyAmount();

        for (int i = 0; i < _robots.Count; i++)
            _robots[i].ResourceDelivered -= OnResourceDelivered;

        _resourceMonitor.CreateRobotAvailable -= SpawnRobot;
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

    private void InitStartRobot(Robot robot)
    {
        robot.Initialize(_availableRobotSpawnpoints.GetRandom());
        robot.ResourceDelivered += OnResourceDelivered;
        _robots.Add(robot);
        _availableRobots.Add(robot);
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
}
