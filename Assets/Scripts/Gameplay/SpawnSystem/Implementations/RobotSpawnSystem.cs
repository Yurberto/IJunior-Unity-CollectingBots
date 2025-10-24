using UnityEngine;

public class RobotSpawnSystem : MonoBehaviour
{
    [SerializeField] private Robot _prefab;
    [SerializeField] private Transform _parentContainer;

    private Spawner<Robot> _robotSpawner;

    private void Awake()
    {
        _robotSpawner = new Spawner<Robot>(_prefab, _parentContainer);
    }

    public Robot Spawn()
    {
        return _robotSpawner.Spawn();
    }
}
