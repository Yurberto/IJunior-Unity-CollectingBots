using UnityEngine;

public class NoSpawner<T> where T : MonoBehaviour
{
    protected T Prefab;
    protected Transform ParentContainer;

    public NoSpawner(T prefab, Transform parentContainer)
    {
        Prefab = prefab;
        ParentContainer = parentContainer;
    }

    public virtual T Spawn()
    {
        T spawned = Object.Instantiate(Prefab, ParentContainer);

        return spawned;
    }
}
