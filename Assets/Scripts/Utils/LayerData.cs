using UnityEngine;

public static class LayerData
{
    public static readonly LayerMask Resource = LayerMask.GetMask(nameof(Resource));
    public static readonly LayerMask Base = LayerMask.GetMask(nameof(Base));
    public static readonly LayerMask Map = LayerMask.GetMask(nameof(Map));
}
