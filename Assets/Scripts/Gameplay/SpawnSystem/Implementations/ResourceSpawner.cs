using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ResourceSpawner : PoolSpawner<Resource>
{
    [Space(16)]
    [SerializeField] private SpawnpointContainer _spawnpointsContainer;
    [Space]
    [SerializeField, Range(0.0f, 50.0f)] private float _spawnDelay = 1.0f;

    private List<Vector3> _availableSpawpoints = new List<Vector3>();
    private Hub<Resource> _hub = new Hub<Resource>();

    private CancellationTokenSource _cancellationTokenSource;

    public Hub<Resource> Hub => _hub;

    private void OnEnable()
    {
        _availableSpawpoints = _spawnpointsContainer.Spawnpoints;

        StartSpawn();

        ObjectInstantiated += _hub.Add;
    }

    private void OnDisable()
    {
        StopSpawn();

        ObjectInstantiated -= _hub.Add;
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

    private void StartSpawn()
    {
        if (_cancellationTokenSource != null)
            return;

        _cancellationTokenSource = new CancellationTokenSource();
        StartSpawnAsync().Forget();
    }

    private void StopSpawn()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();

        _cancellationTokenSource = null;
    }

    private async UniTaskVoid StartSpawnAsync()
    {
        while (_cancellationTokenSource.IsCancellationRequested == false)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay), cancellationToken: _cancellationTokenSource.Token);

            Spawn();
        }
    }
}
