using UnityEngine;

public static class DebugRayCast
{

    public static void DrawSphereCast(Ray ray, float radius, float distance, Color color, float duration = 0f)
    {
        int segments = 10;
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = ray.origin + ray.direction * distance * t;

        Debug.DrawLine(pos + Vector3.up * radius, pos - Vector3.up * radius, color, duration);
            Debug.DrawLine(pos + Vector3.right * radius, pos - Vector3.right * radius, color, duration);
            Debug.DrawLine(pos + Vector3.forward * radius, pos - Vector3.forward * radius, color, duration);
        }
    }

}
