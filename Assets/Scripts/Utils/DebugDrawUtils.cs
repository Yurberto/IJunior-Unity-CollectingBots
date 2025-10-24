using UnityEngine;

public static class DebugDrawUtils
{
    public static void DrawBox(Vector3 center, Vector3 size, Color color, float duration = 0f)
    {
        Debug.Log("DRAW");
        Vector3 halfSize = size * 0.5f;

        DrawSquare(center + new Vector3(0, -halfSize.y, 0),
                  new Vector3(size.x, 0, size.z), color, duration);

        DrawSquare(center + new Vector3(0, halfSize.y, 0),
                  new Vector3(size.x, 0, size.z), color, duration);

        Vector3[] corners = GetBoxCorners(center, halfSize);

        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 4], color, duration);
        }
    }

    private static void DrawSquare(Vector3 center, Vector3 size, Color color, float duration)
    {
        Vector3 halfSize = size * 0.5f;
        Vector3 topLeft = center + new Vector3(-halfSize.x, 0, -halfSize.z);
        Vector3 topRight = center + new Vector3(halfSize.x, 0, -halfSize.z);
        Vector3 bottomLeft = center + new Vector3(-halfSize.x, 0, halfSize.z);
        Vector3 bottomRight = center + new Vector3(halfSize.x, 0, halfSize.z);

        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }

    private static Vector3[] GetBoxCorners(Vector3 center, Vector3 halfSize)
    {
        return new Vector3[]
        {
            center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
            center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z),
            center + new Vector3(halfSize.x, -halfSize.y, halfSize.z),
            center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z),
            center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z),
            center + new Vector3(halfSize.x, halfSize.y, -halfSize.z),
            center + new Vector3(halfSize.x, halfSize.y, halfSize.z),
            center + new Vector3(-halfSize.x, halfSize.y, halfSize.z)
        };
    }
}