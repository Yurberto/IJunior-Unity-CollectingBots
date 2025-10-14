using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    private Vector3 _spawnPosition;

    private bool _canBeCollect = true;

    public event Action<Resource> ReleaseTimeCome;

    public Vector3 SpawnPosition => _spawnPosition;
    public bool CanBeCollect => _canBeCollect;

    public void Initialize(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
        transform.position = spawnPosition;
    }

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void OnChasing()
    {
        _canBeCollect = false;
    }

    public void OnCollect()
    {
        ReleaseTimeCome?.Invoke(this);
        _canBeCollect = true;
    }

    public void MakeCollectable()
    {
        _canBeCollect = true;
    }
}
