using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(FlagController))]
public class Base : MonoBehaviour
{
    [SerializeField] private BaseRenderer _baseRenderer;
    [SerializeField] private Transform _robotSpawnpointContainer;

    [SerializeField, Range(0.0f, 10.0f)] private float _workDelay = 0.5f;   

    private Collider _collider;
    private FlagController _flagController;

    private ResourceMonitor _resourceMonitor = new();

    private List<Robot> _robots = new();
    private Hub<Robot> _availableRobots = new();
    private Hub<Transform> _availableRobotSpawnpoints = new();

    private RobotSpawnSystem _robotSpawner;
    private Hub<Resource> _availableResources;

    private CancellationTokenSource _cancellationTokenSource;

    public event Action<Robot, Vector3> RobotReachedFlag;

    public Vector3 Size => _collider.bounds.size;
    public bool CanCreateNew => _robots.Count > 1;

    public void Initialize(Robot startRobot, RobotSpawnSystem robotSpawner, Hub<Resource> availableResources)
    {
        RegisterRobot(startRobot);
        _robotSpawner = robotSpawner;
        _availableResources = availableResources;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        _flagController = GetComponent<FlagController>();

        for (int i = 0; i < _robotSpawnpointContainer.childCount; i++)
            _availableRobotSpawnpoints.Add(_robotSpawnpointContainer.GetChild(i));
    }

    private void OnEnable()
    {
        StartCollecting().Forget();
        _resourceMonitor.CheckMoneyAmountAsync().Forget();

        _resourceMonitor.CreateRobotAvailable += SpawnRobot;
    }

    private void OnDisable()
    {
        StopCollecting();
        _resourceMonitor.StopCheckMoneyAmount();

        for (int i = 0; i < _robots.Count; i++)
            _robots[i].ResourceDelivered -= OnResourceDelivered;

        _resourceMonitor.CreateRobotAvailable -= SpawnRobot;
    }

    public void OnClick()
    {
        _baseRenderer.OnClick();
    }

    public void SetFlag(RaycastHit hit)
    {
        _flagController.PutOn(hit);
        _baseRenderer.OnFlagSet();
        CollectForNewBase();
    }

    private void CollectForNewBase()
    {
        _resourceMonitor.CreateRobotAvailable -= SpawnRobot;
        _resourceMonitor.CreateBaseAvailable += SendRobotToFlag;
    }

    private void SendRobotToFlag()
    {
        SendRobotToFlagAsync().Forget();
    }

    private async UniTaskVoid SendRobotToFlagAsync()
    {
        if (_flagController.Flag.enabled == false)
            return;

        if (_resourceMonitor.TrySpend(CreateCostData.BaseCost) == false)
            return;

        _resourceMonitor.CreateBaseAvailable -= SendRobotToFlag;

        await UniTask.WaitUntil(() => _availableRobots.Count > 0);

        Robot robotCreator = _availableRobots.GetRandom();
        robotCreator.ResourceDelivered -= OnResourceDelivered;
        _robots.Remove(robotCreator);

        await robotCreator.GoToFlag(_flagController.Flag);

        RobotReachedFlag?.Invoke(robotCreator, _flagController.Flag.transform.position);

        _flagController.Remove();
        _baseRenderer.OnDefault();
    }

    private async UniTaskVoid StartCollecting()
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

    private void StopCollecting()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    private void RegisterRobot(Robot robot)
    {
        Vector3 position = _availableRobotSpawnpoints.GetRandom().position;
        robot.Initialize(position);
        robot.transform.parent = transform;

        _robots.Add(robot);
        _availableRobots.Add(robot);

        robot.ResourceDelivered += OnResourceDelivered;
    }

    private void SpawnRobot()
    {
        if (_availableRobotSpawnpoints.IsEmpty)
            return;

        if (_resourceMonitor.TrySpend(CreateCostData.RobotCost) == false)
            return;

        Robot spawned = _robotSpawner.Spawn();

        RegisterRobot(spawned);
    }

    private void OnResourceDelivered(Robot deliveryRobot, Resource resource)
    {
        if (resource == null)
            Debug.Log("REs");
        if (deliveryRobot == null)
            Debug.Log("Robot");

        _availableRobots.Add(deliveryRobot);

        resource.InvokeRelease();
        _resourceMonitor.AddResource();
    }
}
