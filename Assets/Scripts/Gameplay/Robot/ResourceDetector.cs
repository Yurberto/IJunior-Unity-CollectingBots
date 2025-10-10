using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ResourceDetector : MonoBehaviour
{
    public event Action<Resource> AvailableResourceHitted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && resource.CanBeCollect)
        {
            AvailableResourceHitted?.Invoke(resource);
        }
    }
}
