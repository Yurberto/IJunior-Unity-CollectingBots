using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class ResourceMonitor
{
    private int _maxCount;
    private float _checkDelay;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public event Action CreateRobotAvailable;

    public ReactiveProperty<int> Count { get; private set; } = new();

    public ResourceMonitor(float checkMoneyAmountDelay = 0.5f, int count = 0, int maxCount = 1000)
    {
        _checkDelay = checkMoneyAmountDelay;
        Count.Value = count;
        _maxCount = maxCount;
    }

    public void AddResource()
    {
        if (Count.Value >= _maxCount)
            return;

        Count.Value++;
    }

    public bool TrySpend(int cost)
    {
        if (cost < 0 || cost > Count.Value)
            return false;

        Count.Value -= cost;

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
        if (Count.Value - CreateCostData.RobotCost >= 0)
            CreateRobotAvailable?.Invoke();
    }
}
