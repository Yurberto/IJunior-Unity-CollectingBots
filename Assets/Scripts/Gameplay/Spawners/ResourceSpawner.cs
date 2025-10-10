using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [Space]
    [SerializeField] private Transform _spawnpointContainer;

    private Transform[] _spawnpoints;
    private List<Transform> _availableSpawnpoints;

    protected override void Awake()
    {
        base.Awake();

        _spawnpoints = new Transform[_spawnpointContainer.childCount];

        for (int i = 0; i < _spawnpoints.Length; i++)
            _spawnpoints[i] = _spawnpointContainer.GetChild(i);

        _availableSpawnpoints = new List<Transform>(_spawnpoints);
    }

    private void Start()
    {
        Spawn();
        Spawn();
        Spawn();
        Spawn();
    }

    public override Resource Spawn()
    {
        Resource spawned = base.Spawn();
        spawned.transform.position = GetRandomPosition();

        return spawned;
    }

    private Vector3 GetRandomPosition()
    {
        int randomIndex = Random.Range(0, _availableSpawnpoints.Count);
        Vector3 randomPosition = _availableSpawnpoints[randomIndex].position;
        _availableSpawnpoints.RemoveAt(randomIndex);

        return randomPosition;
    }
}
