using UnityEngine;
using Zenject;

public class BaseSpawnSystem : MonoBehaviour
{
    private Robot _startRobot;
    private BaseSpawner _baseSpawner;
    private RobotSpawner _robotSpawner;
    private Hub<Resource> _availableResources;

    [Inject]
    private void Construct(Robot startRobot, BaseSpawner baseSpawner, RobotSpawner robotSpawner, Hub<Resource> availableResources)
    {
        _startRobot = startRobot;
        _baseSpawner = baseSpawner;
        _robotSpawner = robotSpawner;
        _availableResources = availableResources;
    }

    private void Start()
    {
        Spawn(_startRobot, _baseSpawner.transform.position);
    }

    public void Spawn(Robot startRobot, Vector3 spawnPosition)
    {
        Base spawnedBase = _baseSpawner.Spawn();
        spawnedBase.transform.position = spawnPosition;
        spawnedBase.Initialize(startRobot, _robotSpawner, _availableResources);
    }
}
