using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResourceScanner : MonoBehaviour 
{
    [SerializeField, Range(0.0f, 10.0f)] private float _scanDelay = 0.2f;
    [SerializeField, Range(0, 50)] private int _overlapCount = 20;
    [SerializeField] private Vector3 _scanSize = Vector3.one;

    private Hub<Resource> _resourceHub;

    Collider[] _scanned;

    private CancellationTokenSource _cancellationTokenSource;

    public void Initialize(Hub<Resource> hub)
    {
        _resourceHub = hub;
    }

    private void Awake()
    {
        _scanned = new Collider[_overlapCount];
    }

    private void OnEnable()
    {
        StartScan();
    }

    private void OnDisable()
    {
        StopScan();
    }

    private void StartScan()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        StartScanAsync().Forget();
    }

    private void StopScan()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = null;
    }

    private async UniTaskVoid StartScanAsync()
    {
        while (_cancellationTokenSource.IsCancellationRequested == false)
        {
            Scan();

            await UniTask.Delay(TimeSpan.FromSeconds(_scanDelay), cancellationToken: _cancellationTokenSource.Token);
        }
    }

    private void Scan()
    {
        int hittedCount = Physics.OverlapBoxNonAlloc(transform.position, _scanSize, _scanned, Quaternion.identity, LayerData.Resource);
        DebugDrawUtils.DrawBox(transform.position, _scanSize, Color.red, 1);

        for (int i = 0; i < hittedCount; i++)
        {
            if (_scanned[i].TryGetComponent(out Resource resource))

            _resourceHub.SetAvailable(resource);
        }
    }
}
