using System.Collections.Generic;
using UnityEngine;

public class Hub<T> where T : MonoBehaviour
{
    private List<T> _availableObjects = new List<T>();
    private HashSet<T> _busyObjects = new HashSet<T>();

    public void Add(T @object)
    {
        _availableObjects.Add(@object);
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
