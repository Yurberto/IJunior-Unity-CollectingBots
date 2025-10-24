using UnityEngine;

public class Spawner<T> where T : MonoBehaviour
{
    protected T Prefab;
    protected Transform ParentContainer;

    public Spawner(T prefab, Transform parentContainer)
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
