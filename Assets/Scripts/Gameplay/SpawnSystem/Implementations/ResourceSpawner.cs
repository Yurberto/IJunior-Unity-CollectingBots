using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class ResourceSpawner : PoolSpawner<Resource>
{
    [Space(16)]
    [SerializeField] private Transform _spawnpointsContainer;
    [Space]
    [SerializeField, Range(0.0f, 50.0f)] private float _spawnDelay = 1.0f;

    private Hub<Resource> _availableResources = new Hub<Resource>();
    private Hub<Vector3> _availableSpawnpoints = new Hub<Vector3>();

    private CancellationTokenSource _cancellationTokenSource;

    public Hub<Resource> AvailableResources => _availableResources;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < _spawnpointsContainer.childCount; i++)
            _availableSpawnpoints.Add(_spawnpointsContainer.GetChild(i).position);
    }

    private void OnEnable()
    {
        StartSpawn();
    }

    private void OnDisable()
    {
        StopSpawn();
    }

    public override Resource Spawn()
    {
        if (_availableSpawnpoints.IsEmpty)
            return null;

        Resource spawned = base.Spawn();
        spawned.Initialize(_availableSpawnpoints.GetRandom());
        _availableResources.Add(spawned);

        spawned.ReleaseTimeCome += Release;

        return spawned;
    }

    protected override void Release(Resource objectToRelease)
    {
        base.Release(objectToRelease);
        objectToRelease.transform.parent = ParentContainer;
        _availableSpawnpoints.Add(objectToRelease.SpawnPosition);

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
