using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoExtras : MonoBehaviour {
	
    /// <summary>
    /// 
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
	public static void GetWireCircle (float radius, Vector3 offset) {
		List<Vector3> points = new List<Vector3> ();
		for (float theta = 0f; theta < 2f * Mathf.PI; theta += 0.1f) {
			float x = radius * Mathf.Cos (theta);
			float y = radius * Mathf.Sin (theta);

			Vector3 point = new Vector3 (x, 0f, y);
			points.Add (point + offset);
		}

		points.Add (points [0]);

        for (int i = 0; i < points.Count - 1; i++) {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }


}
