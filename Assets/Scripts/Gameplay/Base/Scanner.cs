using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Vector3 _scanBoxSize;
    [Space]
    [SerializeField, Range(0, 100)] private int _maxScannedColliders = 20;

    private Collider[] _scanned;

    private void OnValidate()
    {
        _scanBoxSize.x = Mathf.Max(0, _scanBoxSize.x);
        _scanBoxSize.y = Mathf.Max(0, _scanBoxSize.y);
        _scanBoxSize.z = Mathf.Max(0, _scanBoxSize.z);
    }

    private void Awake()
    {
        _scanned = new Collider[_maxScannedColliders];
    }

    public bool TryScanResources(out Resource[] resourcesToScan)
    {
        List<Resource> scannedResources = new List<Resource>();

        Vector3 scanBoxExtents = _scanBoxSize / 2;
        int hittedCount = Physics.OverlapBoxNonAlloc(transform.position, scanBoxExtents, _scanned);

        for (int i = 0; i < hittedCount; i++)
        {
            if (_scanned[i].TryGetComponent(out Resource resource) && resource.CanBeCollect)
                scannedResources.Add(resource);
        }

        if (scannedResources.Count > 0)
        {
            resourcesToScan = scannedResources.ToArray();
            return true;
        }
        else
        {
            resourcesToScan = null;
            return false;
        }
    }
}
