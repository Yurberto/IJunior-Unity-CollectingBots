using System;
using System.Collections.Generic;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourceViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField, Range(0.0f, 10.0f)] private float _countDelay = 0.2f;

    private int _allResources = 0;

    private List<ResourceMonitor> _monitors = new List<ResourceMonitor>();

    private CancellationTokenSource _cancellationTokenSource;

    private void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        CountAsync().Forget();
    }

    private void OnDisable()
    {
        StopCount();
    }

    public void Add(ResourceMonitor monitor)
    {
        _monitors.Add(monitor);
    }

    private async UniTaskVoid CountAsync()
    {

        while (enabled)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_countDelay), cancellationToken: _cancellationTokenSource.Token);

            _allResources = 0;

            for (int i = 0; i < _monitors.Count; i++)
                _allResources += _monitors[i].Count;

            _textMesh.text = _allResources.ToString();
        }
    }

    private void StopCount()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
}
