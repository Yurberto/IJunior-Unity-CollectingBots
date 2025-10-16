using UnityEngine;

public abstract class PoolSpawner<T> : Spawner<T> where T : MonoBehaviour
{
    private GameObjectPool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new GameObjectPool<T>(() => base.Spawn());
    }

    public override T Spawn()
    {
        return _pool.Get();
    }

    protected virtual void Release(T objectToRelease)
    {
        _pool.Release(objectToRelease);
    }
}
