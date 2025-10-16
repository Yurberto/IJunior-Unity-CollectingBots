using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private List<Robot> _robots = new List<Robot>();
    private ResourceScanner _scanner;

    private SpawnpointContainer _robotSpawnpointContainer;
    private List<Vector3> _availableRobotSpawnpoints;

    private RobotSpawner _robotSpawner;

    private int _resourceCount = 0;

    public event Action<int> ResourceValueChanged;

    public void Initialize(RobotSpawner robotSpawner, Hub<Resource> resourceHub)
    {
        _robotSpawner = robotSpawner;
        _scanner = new ResourceScanner(resourceHub);
    }

    private void Awake()
    {
        _robotSpawnpointContainer = GetComponentInChildren<SpawnpointContainer>();
        _availableRobotSpawnpoints = _robotSpawnpointContainer.Spawnpoints;
    }

    private void Start()
    {
        int startRobotValue = 3;

        for (int i = 0; i < startRobotValue; i++)
            CreateRobot();
    }

    private void OnEnable()
    {
        _scanner.StartScan().Forget();

        _scanner.ResourceScanned += SendRobot;
    }

    private void OnDisable()
    {
        _scanner.StopScan();

        foreach (Robot robot in _robots)
        {
            robot.ResourceDelivered -= CollectResource;
        }

        _scanner.ResourceScanned -= SendRobot;
    }

    private void CreateRobot()
    {
        Robot spawned = _robotSpawner.Spawn();
        spawned.Initialize(SpawnUtils.GetSpawnPosition(_availableRobotSpawnpoints));
        _robots.Add(spawned);

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
        ResourceValueChanged?.Invoke(++_resourceCount);

        resource.InvokeRelease();
    }
}
