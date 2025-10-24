using UnityEngine;

public class DistanceChecker
{
    private const int OverlapValue = 1;
    private const int HalfDivider = 2;
    private const float MultiplierY = 10;

    private Collider[] _results = new Collider[OverlapValue];

    public bool IsPlaceIzolated(Vector3 position, Vector3 boxExtents, LayerMask layerMask)
    {
        Vector3 correctExtents = new Vector3(boxExtents.x / HalfDivider, boxExtents.y * MultiplierY, boxExtents.z / HalfDivider);
        Debug.Log("proverka");
        Physics.OverlapBoxNonAlloc(position, correctExtents, _results, Quaternion.identity, layerMask);
        DebugDrawUtils.DrawBox(position, correctExtents, Color.red, 15); 

        return _results[0] == null;
    }
}
