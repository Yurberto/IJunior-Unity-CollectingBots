using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] private Transform _parentContainer;

    private GameObjectPool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new GameObjectPool<T>(() => Instantiate(Prefab, _parentContainer));
    }

    public virtual T Spawn()
    {
        return _pool.Get();
    }

    protected virtual void Release(T objectToRelease)
    {
        _pool.Release(objectToRelease);
    }
}
