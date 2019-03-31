using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : MonoBehaviour {

	public float shipRadius;

	private MovementManager moveManager;

	private Vector3 finalTargetPosition = Vector3.zero;
	public Vector3 FinalTargetPosition {
		get { return finalTargetPosition; }
		set { finalTargetPosition = value; }
	}

	public bool drawAvoidencePath;

	void Start () {
		moveManager = GetComponentInParent<MovementManager> ();
		moveManager.ColAvoidList.Add (this);
	}

	public Vector3 UpdateFinalTargetPosition (Vector3 position) {
		finalTargetPosition = position;
		CollisionAvoidance conflictColAvoid = moveManager.IsPositionOccupied(position, shipRadius);

		if (conflictColAvoid != null) {
			Vector3 conflictToTargetDir = position - conflictColAvoid.FinalTargetPosition;

			int crashCount = 0;
			while (conflictColAvoid != null  && crashCount < 100) {
				position += conflictToTargetDir.normalized * (shipRadius * 2f);
				finalTargetPosition = position;
				conflictColAvoid = moveManager.IsPositionOccupied(position, shipRadius);

				if (drawAvoidencePath) {
					Debug.DrawRay (position, -(conflictToTargetDir.normalized * (shipRadius * 2f)), Color.cyan);
					Debug.DrawRay (position, Vector3.up * 25f, Color.red);
				}

				crashCount++;
			}

			if (crashCount == 100) {
				Debug.LogError ("Infinite While Loop Detected");
			}

		}

		return position;
	}


	void OnDrawGizmos () {		
		GizmoExtras.GetWireCircle (shipRadius, transform.position);		

		Gizmos.color = Color.red;
		GizmoExtras.GetWireCircle (shipRadius * 2, transform.position);

		Gizmos.DrawSphere (transform.position, 2.5f);

	}




}
