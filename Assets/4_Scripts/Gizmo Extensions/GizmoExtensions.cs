using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoExtensions {

    public static void DrawWireCircle(Vector3 position, float radius) {
        List<Vector3> points = new List<Vector3>();

        for (float theta = 0f; theta < 2f * Mathf.PI; theta += 0.157f)
            points.Add(new Vector3(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta)) + position);

        points.Add(points[0]);

        for (int i = 0; i < points.Count - 1; i++)
            Gizmos.DrawLine(points[i], points[i + 1]);
    }

}