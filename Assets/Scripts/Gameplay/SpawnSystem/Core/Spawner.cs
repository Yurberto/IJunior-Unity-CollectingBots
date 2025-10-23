using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected Transform ParentContainer;

    public virtual T Spawn()
    {
        T spawned = Instantiate(Prefab, ParentContainer);

        return spawned;
    }
}
