using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResourceMonitor
{
    private int _maxCount;
    private float _checkDelay;

    private int _count = 0;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public event Action CreateRobotAvailable;
    public event Action CreateBaseAvailable;

    public int Count => _count;

    public ResourceMonitor(float checkMoneyAmountDelay = 0.5f, int count = 0, int maxCount = 1000)
    {
        _checkDelay = checkMoneyAmountDelay;
        _count = count;
        _maxCount = maxCount;
    }

    public void AddResource()
    {
        if (_count >= _maxCount)
            return;

        _count++;

        Debug.Log(Count);
    }

    public bool TrySpend(int cost)
    {
        if (cost < 0 || cost > _count)
            return false;

        _count -= cost;

        return true;
    }

    public async UniTaskVoid CheckMoneyAmountAsync()
    {
        while (_cancellationTokenSource.IsCancellationRequested == false)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_checkDelay), cancellationToken: _cancellationTokenSource.Token);
            CheckMoneyAmount();
        }
    }

    public void StopCheckMoneyAmount()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    private void CheckMoneyAmount()
    {
        if (_count - CreateCostData.RobotCost >= 0)
            CreateRobotAvailable?.Invoke();

        if (_count - CreateCostData.BaseCost >= 0)
            CreateBaseAvailable?.Invoke();
    }
}
