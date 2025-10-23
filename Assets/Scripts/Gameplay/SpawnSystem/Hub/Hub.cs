using System.Collections.Generic;
using UnityEngine;

public class Hub<T>
{
    private List<T> _objects = new List<T>();

    public bool IsEmpty => _objects.Count == 0;
    public int Count => _objects.Count;

    public void Fill(List<T> other)
    {
        Debug.Log(other.Count + " " + typeof(T));
        for (int i = 0; i < other.Count; i++)
            _objects.Add(other[i]);
    }

    public void Add(T @object)
    {
        if (_objects.Contains(@object))
            return;

        _objects.Add(@object);
    }

    public T GetRandom()
    {
        if (_objects == null || _objects.Count == 0)
            throw new System.Exception(null);

        int randomIndex = Random.Range(0, _objects.Count);
        T randomObject = _objects[randomIndex];

        _objects.RemoveAt(randomIndex);

        return randomObject;
    }
}
