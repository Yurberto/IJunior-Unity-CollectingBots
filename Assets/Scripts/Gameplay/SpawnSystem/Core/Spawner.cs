using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected Transform ParentContainer;

    public virtual T Spawn()
    {
        return Instantiate(Prefab, ParentContainer);
    }
}
