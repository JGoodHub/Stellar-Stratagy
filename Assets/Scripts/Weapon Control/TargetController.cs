using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

	public float range;

	public bool isFriendly;

	public TurretManager turretManager;

	private Transform target;
	private List<Transform> enemiesInRange;

	void Start () {
		enemiesInRange = new List<Transform> ();

		turretManager.IsFriendly = isFriendly;

		GetComponent<ShipData> ().IsFriendly = isFriendly;
	}
	
	void Update () {
		enemiesInRange.Clear ();
		Collider[] overlapColliders = Physics.OverlapSphere (transform.position, range);
		for (int i = 0; i < overlapColliders.Length; i++) {
			if (overlapColliders [i].tag == "Enemy Root") {
				enemiesInRange.Add(overlapColliders[i].transform);

				if (target == null) {
					target = overlapColliders [i].transform;
					turretManager.SetTarget (target);
				}
			}
		}

		if (target != null && !enemiesInRange.Contains(target)) {
			target = null;

			turretManager.CeaseFire ();

		}
			

	}

	void OnDrawGizmos () {		
		GizmoExtras.GetWireCircle (range, transform.position);


	}



}
