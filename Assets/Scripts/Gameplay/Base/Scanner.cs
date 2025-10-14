using System;
using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10.0f)] private float _scanDelay = 0.2f;
    [Space]
    [SerializeField] private Vector3 _scanSize = Vector3.one;
    [SerializeField, Range(0, 50)] private int _overlapCollidersValue;

    private Coroutine _scanCoroutine;

    public event Action<Resource> ResourceScanned;

    public void StartScan()
    {
        _scanCoroutine = StartCoroutine(ScanCoroutine());
    }

    public bool TryScan(out Resource collectableResource)
    {
        Collider[] hitted = new Collider[_overlapCollidersValue];
        int hittedCount = Physics.OverlapBoxNonAlloc(transform.position, _scanSize, hitted, Quaternion.identity, LayerData.Resource);

        for (int i = 0; i < hittedCount; i++)
        {
            if (hitted[i].TryGetComponent(out Resource resource) && resource.CanBeCollect)
            {
                collectableResource = resource;
                return true;
            }
        }

        collectableResource = null;
        return false;
    }

    private IEnumerator ScanCoroutine()
    {
        var wait = new WaitForSeconds(_scanDelay);

        while (enabled)
        {
            if (TryScan(out Resource resource))
                ResourceScanned?.Invoke(resource);

            yield return wait;
        }
    }
}
