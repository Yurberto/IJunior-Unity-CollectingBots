using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerHandler : MonoBehaviour
{
    public event Action<Resource> ResourceHitted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
            ResourceHitted?.Invoke(resource);
    }
}
