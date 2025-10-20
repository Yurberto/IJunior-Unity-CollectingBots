using System;

public class ReactiveProperty<T>
{
    private T _value;

    public event Action<T> Changed;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            Changed?.Invoke(value);
        }
    }

    public ReactiveProperty(T value)
    {
        _value = value;
    }
}
