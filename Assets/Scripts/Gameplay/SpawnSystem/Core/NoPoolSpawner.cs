using UnityEngine;

public class NoPoolSpawner<T> : NoSpawner<T> where T : MonoBehaviour
{
    private GameObjectPool<T> _pool;

    public NoPoolSpawner(T prefab, Transform parentContainer) : base(prefab, parentContainer)
    {
        _pool = new GameObjectPool<T>(() => base.Spawn());
    }

    public override T Spawn()
    {
        return _pool.Get();
    }

    public void Release(T objectToRelease)
    {
        _pool.Release(objectToRelease);
        objectToRelease.transform.parent = ParentContainer;
    }
}
