using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class ResourceSpawner : Spawner<Resource>
{
    [Space(16)]
    [SerializeField] private SpawnpointContainer _spawnpointsContainer;
    [Space]
    [SerializeField, Range(0.0f, 50.0f)] private float _spawnDelay = 1.0f;

    private List<Vector3> _availableSpawpoints = new List<Vector3>();

    private Coroutine _spawnCoroutine;

    private void Start()
    {
        _availableSpawpoints = _spawnpointsContainer.Spawnpoints;

        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    public override Resource Spawn()
    {
        if (_availableSpawpoints.Count == 0)
            return null;

        Resource spawned = base.Spawn();
        spawned.Initialize(SpawnUtils.GetSpawnPosition(_availableSpawpoints));

        spawned.ReleaseTimeCome += Release;

        return spawned;
    }

    protected override void Release(Resource objectToRelease)
    {
        base.Release(objectToRelease);
        objectToRelease.transform.parent = ParentContainer;
        _availableSpawpoints.Add(objectToRelease.SpawnPosition);

        objectToRelease.ReleaseTimeCome -= Release;
    }

    private IEnumerator SpawnCoroutine()
    {
        var wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            yield return wait;
            Spawn();
        }
    }
}
