using UnityEngine;

public class DistanceChecker
{
    private const int OverlapValue = 1;
    private const float DividerXZ = 1.5f;
    private const float MultiplierY = 10;

    private Collider[] _results = new Collider[OverlapValue];

    public bool IsPlaceIzolated(Vector3 position, Vector3 boxExtents, LayerMask layerMask)
    {
        _results[0] = null;
        Vector3 correctExtents = new Vector3(boxExtents.x / DividerXZ, boxExtents.y * MultiplierY, boxExtents.z / DividerXZ);
        Physics.OverlapBoxNonAlloc(position, correctExtents, _results, Quaternion.identity, layerMask);

        return _results[0] == null;
    }
}
