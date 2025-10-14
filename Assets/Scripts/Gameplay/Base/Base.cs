using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private List<Robot> _robots = new List<Robot>();

    private SpawnpointContainer _robotSpawnpointContainer;
    private List<Vector3> _availableRobotSpawnpoints;

    private RobotSpawner _robotSpawner;
    private Scanner _scanner;

    private int _resourceCount = 0;

    public event Action<int> ResourceValueChanged;

    public void Initialize(RobotSpawner robotSpawner, Scanner scanner)
    {
        _robotSpawner = robotSpawner;
        _scanner = scanner;
    }

    private void Awake()
    {
        _robotSpawnpointContainer = GetComponentInChildren<SpawnpointContainer>();
        _availableRobotSpawnpoints = _robotSpawnpointContainer.Spawnpoints;
    }

    private void Start()
    {
        _scanner.StartScan();

        int startRobotValue = 3;

        for (int i = 0; i < startRobotValue; i++)
            CreateRobot();
    }

    private void OnEnable()
    {
        _scanner.ResourceScanned += SendRobot;
    }

    private void OnDisable()
    {
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
        spawned.ResourceDelivered += CollectResource;
        _robots.Add(spawned);
    }

    private void SendRobot(Resource resource)
    {
        resource.OnChasing();
        bool robotSended = false;

        for (int i = 0; i < _robots.Count; i++)
        {
            if (_robots[i].IsWork == false)
            {
                _robots[i].GoPickUp(resource);
                robotSended = true;
                break;
            }
        }

        if (robotSended == false) 
            resource.MakeCollectable();
    }

    private void CollectResource(Resource resource)
    {
        resource.OnCollect();
        ResourceValueChanged?.Invoke(++_resourceCount);
    }
}
