using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    //-----VARIABLES-----

	public float range;

	private TurretManager turretManager;
    private ShipController shipController;
    public TurretController[] turretControllers;

	private ShipController target;
	private HashSet<ShipController> targetsInRange;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
	public void Initialise() {
        turretManager = GetComponentInChildren<TurretManager>();
        shipController = GetComponent<ShipController>();
        turretControllers = GetComponentsInChildren<TurretController>();

        targetsInRange = new HashSet<ShipController>();
        
        foreach (TurretController turretControl in turretControllers) {
            turretControl.Initialise();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update () {
		targetsInRange.Clear();
		Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, range);
		foreach (Collider collider in overlappingColliders) {
			if (collider.tag == "Ship") {
                ShipController otherShip = collider.gameObject.GetComponent<ShipController>();
                if (otherShip.isAlliedShip != shipController.isAlliedShip) {
                    targetsInRange.Add(otherShip);
                }
            }
		}

        if (targetsInRange.Contains(target) == false) {
            foreach (ShipController enemyShip in targetsInRange) {
                if (target == null) {
                    target = enemyShip;
                } else if (Vector3.Distance(gameObject.transform.position, enemyShip.transform.position) < Vector3.Distance(gameObject.transform.position, target.transform.position)) {
                    target = enemyShip;              
                }
            }
        }

        if (target != null && shipController.isAlliedShip) {
            //Check which turrets have line of sight
            foreach (TurretController turretControl in turretControllers) {                
                turretControl.target = target;
                if (turretControl.IsTargetInLineOfSight() == true) {
                    //Trigger the unblocked turrets to fire if they can
                    turretControl.Fire();
                }
            }
        }

    }

    //-----GIZMOS-----

    //public bool drawGizmos;
	void OnDrawGizmos () {	
        
	}

}
