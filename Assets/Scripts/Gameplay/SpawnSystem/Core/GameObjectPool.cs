using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : MonoBehaviour 
{
    private Func<T> _createFunction;
    
    private Queue<T> _queue = new Queue<T>();

    public GameObjectPool(Func<T> createFunction)
    {
        _createFunction = createFunction;
    }

    public virtual T Get()
    {
        T spawned = _queue.Count > 0 ? _queue.Dequeue() : _createFunction();
        spawned.gameObject.SetActive(true);

        return spawned;
    }

    public virtual void Release(T objectToRelease)
    {
        objectToRelease.gameObject.SetActive(false);
        _queue.Enqueue(objectToRelease);
    }
}
