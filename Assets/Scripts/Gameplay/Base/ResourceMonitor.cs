using System;

public class ResourceMonitor
{
    private int _maxCount;
    private int _count;

    public int Count
        {
        get 
        { 
            return _count;
        }
        private set
        {
            _count = value;
            CountChanged?.Invoke(_count); 
        }
    }

    public event Action<int> CountChanged;

    public ResourceMonitor(int count = 0, int maxCount = 1000)
    {
        _count = count;
        _maxCount = maxCount;
    }

    public void AddResource()
    {
        if (Count >= _maxCount)
            return;

        Count++;
    }

    public bool TrySpend(int cost)
    {
        if (cost < 0 || cost > Count) 
            return false;

        Count -= cost;

        return true;
    }
}
