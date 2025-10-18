using System.Collections.Generic;
using UnityEngine;

public class Hub<T> where T : MonoBehaviour
{
    private List<T> _availableObjects = new List<T>();
    private List<T> _busyObjects = new List<T>();

    public bool HasAvailable => _availableObjects.Count > 0;

    public void Add(T @object)
    {
        if (_availableObjects.Contains(@object) || _busyObjects.Contains(@object))
            return;

        _availableObjects.Add(@object);
    }

    public T GetAvailable()
    {
        int randomIndex = Random.Range(0, _availableObjects.Count);
        T available = _availableObjects[randomIndex];

        _availableObjects.RemoveAt(randomIndex);
        _busyObjects.Add(available);

        return available;
    }

    public void SetAvailable(T @object)
    {
        if (_busyObjects.Remove(@object))
        {
            _availableObjects.Add(@object);
        }
    }
}
