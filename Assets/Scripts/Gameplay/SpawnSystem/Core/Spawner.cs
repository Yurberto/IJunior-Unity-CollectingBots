using System;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected Transform ParentContainer;

    public event Action<T> ObjectInstantiated;

    public virtual T Spawn()
    {
        T spawned = Instantiate(Prefab, ParentContainer);
        ObjectInstantiated?.Invoke(spawned);

        return spawned;
    }
}
