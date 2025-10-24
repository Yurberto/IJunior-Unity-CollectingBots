using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResourceSpawnSystem : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private Transform _parentContainer;
    [Space]
    [SerializeField] private Transform _spawnpointsContainer;
    [SerializeField, Range(0.0f, 50.0f)] private float _spawnDelay = 1.0f;

    PoolSpawner<Resource> _resourceSpawner;

    private Hub<Resource> _availableResources = new();
    private Hub<Vector3> _availableSpawnpoints = new();

    private CancellationTokenSource _cancellationTokenSource;

    public Hub<Resource> AvailableResources => _availableResources;

    private void Awake()
    {
        _resourceSpawner = new PoolSpawner<Resource>(_prefab, _parentContainer);

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

    private void Spawn()
    {
        if (_availableSpawnpoints.IsEmpty)
            return;

        Resource spawned = _resourceSpawner.Spawn();
        spawned.Initialize(_availableSpawnpoints.GetRandom());
        _availableResources.Add(spawned);

        spawned.ReleaseTimeCome += Release;
    }

    private void Release(Resource objectToRelease)
    {
        _resourceSpawner.Release(objectToRelease);
        _availableSpawnpoints.Add(objectToRelease.SpawnPosition);

        objectToRelease.ReleaseTimeCome -= Release;
    }
}
