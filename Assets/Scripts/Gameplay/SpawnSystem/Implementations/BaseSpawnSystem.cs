using UnityEngine;
using Zenject;

public class BaseSpawnSystem : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _baseContainer;

    private Spawner<Base> _baseSpawner;

    private Robot _startRobot;
    private RobotSpawnSystem _robotSpawner;
    private Hub<Resource> _availableResources;

    [Inject]
    private void Construct(Robot startRobot, RobotSpawnSystem robotSpawner, Hub<Resource> availableResources)
    {
        _startRobot = startRobot;
        _robotSpawner = robotSpawner;
        _availableResources = availableResources;
    }

    private void Awake()
    {
        _baseSpawner = new Spawner<Base>(_basePrefab, _baseContainer);
    }

    private void Start()
    {
        Spawn(_startRobot, Vector3.zero);
    }

    public void Spawn(Robot startRobot, Vector3 spawnPosition)
    {
        Base spawnedBase = _baseSpawner.Spawn();
        spawnedBase.transform.position = spawnPosition;
        spawnedBase.Initialize(startRobot, _robotSpawner, _availableResources);
    }
}
