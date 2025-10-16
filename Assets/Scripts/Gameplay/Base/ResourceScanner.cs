using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResourceScanner
{
    [SerializeField, Range(0.0f, 10.0f)] private float _scanDelay = 0.2f;

    private Hub<Resource> _resourceHub;

    private CancellationTokenSource _cancellationTokenSource;

    public event Action<Resource> ResourceScanned;

    public ResourceScanner(Hub<Resource> resourceHub)
    {
        _resourceHub = resourceHub;
    }

    public async UniTaskVoid StartScan()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        while (_cancellationTokenSource.IsCancellationRequested == false)
        {
            if (TryScan(out Resource scanned))
                ResourceScanned?.Invoke(scanned);

            await UniTask.Delay(TimeSpan.FromSeconds(_scanDelay), cancellationToken: _cancellationTokenSource.Token);
        }
    }

    public void StopScan()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = null;
    }

    public bool TryScan(out Resource collectableResource)
    {
        if (_resourceHub.TryGetAvailable(out collectableResource))
            return true;

        collectableResource = null;
        return false;
    }
}
