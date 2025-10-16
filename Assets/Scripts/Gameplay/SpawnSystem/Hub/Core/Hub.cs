using System.Collections.Generic;
using UnityEngine;

public abstract class Hub<T> where T : MonoBehaviour
{
    private List<T> _availableObjects = new List<T>();
    private List<T> _busyObjects = new List<T>();

    public void Add(T resource)
    {
        _availableObjects.Add(resource);
    }

    public bool TryGetAvailable(out T available)
    {
        if (_availableObjects.Count == 0)
        {
            available = null;
            return false;
        }

        int randomindex = Random.Range(0, _availableObjects.Count);
        available = _availableObjects[randomindex];

        _availableObjects.RemoveAt(randomindex);
        _busyObjects.Add(available);

        return true;
    }
}
