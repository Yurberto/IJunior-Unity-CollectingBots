using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerHandler : MonoBehaviour
{
    public event Action<Resource> AvailableResourceHitted;
    public event Action SpawnTrggierHitted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && resource.CanBeCollect)
        {
            AvailableResourceHitted?.Invoke(resource);
        }
        if (other.TryGetComponent(out Base _))
        {
            SpawnTrggierHitted?.Invoke();
        }
    }
}
