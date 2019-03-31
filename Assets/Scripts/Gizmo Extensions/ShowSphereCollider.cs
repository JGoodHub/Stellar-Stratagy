using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSphereCollider : MonoBehaviour {

	public bool showWireFrame;
	public bool showSolid;

	void OnDrawGizmos () {
		Gizmos.color = Color.green;
		if (showWireFrame) {			
			SphereCollider[] sphereCols = GetComponents<SphereCollider> ();
			foreach (SphereCollider col in sphereCols) {
				Gizmos.DrawWireSphere (col.center + transform.position, col.radius);
			}
		}

		if (showSolid) {			
			SphereCollider[] sphereCols = GetComponents<SphereCollider> ();
			foreach (SphereCollider col in sphereCols) {
				Gizmos.DrawSphere (col.center + transform.position, col.radius);
			}
		}

	}
}
