using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

	public float range;

	private TurretManager turretManager;
    private ShipController shipController;

	private ShipController currentTarget;
	private HashSet<ShipController> targetsInRange;

	void Start () {
        turretManager = GetComponentInChildren<TurretManager>();
        shipController = GetComponent<ShipController>();

        targetsInRange = new HashSet<ShipController>();
    }

    void Update () {
		targetsInRange.Clear();
		Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, range);
		foreach (Collider collider in overlappingColliders) {
			if (collider.tag == "Ship") {
                ShipController otherShip = collider.gameObject.GetComponent<ShipController>();
                if (otherShip.isAlliedShip != shipController.isAlliedShip) {
                    targetsInRange.Add(otherShip);
                }

                if (currentTarget == null) {
                    currentTarget = otherShip;
                }
			}
		}

        if (targetsInRange.Contains(currentTarget) == false) {
            //Pick a new target
        } else {
            //Fire at the target
        }    
        

    }

    public bool drawGizmos;
	void OnDrawGizmos () {	
        if (drawGizmos) {
            Gizmos.color = Color.white;
            GizmoExtras.DrawWireCircle(range, transform.position);
        }
		


	}



}
