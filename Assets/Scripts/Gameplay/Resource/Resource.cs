using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    private Vector3 _spawnPosition;

    public event Action<Resource> ReleaseTimeCome;

    public Vector3 SpawnPosition => _spawnPosition;

    public void Initialize(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
        transform.position = spawnPosition;
    }

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void InvokeRelease() => ReleaseTimeCome?.Invoke(this);
}
