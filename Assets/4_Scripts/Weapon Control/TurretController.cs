using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    //-----VARIABLES-----

    [HideInInspector]
    public ShipController target;

    [HideInInspector]
    public bool losToTargetBlocked;

    public float damage;
    public GameObject laserPrefab;

	public Transform yawAxisParent;
	public Transform pitchAxisParent;

	public float rateOfFire;
	private float fireCountdown;
	public Transform[] barrelEnd;
	private int barrelIndex;

    public LayerMask layerMask;

    public GameObject shieldHitPrefab;
    public GameObject hullHitExplosionPrefab;

    //-----METHODS-----
    	
	/// <summary>
    /// 
    /// </summary>
	private void Update () {
		fireCountdown -= Time.deltaTime;

		if (target != null) {
			float distanceToLocalPlane = Vector3.Dot (transform.up, target.transform.position - yawAxisParent.position);
			Vector3 pointOnLocalPlane = target.transform.position - yawAxisParent.up * distanceToLocalPlane;

			if (Vector3.Angle (transform.up, (target.transform.position - pointOnLocalPlane).normalized) < 1f) {			
				yawAxisParent.LookAt (pointOnLocalPlane, transform.up);
				pitchAxisParent.LookAt (target.transform.position, transform.up);
			}
		}
	}


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsTargetInLineOfSight () {
        if (target != null) {
            Vector3 targetDirection = target.transform.position - transform.position;
            if (Physics.Raycast(transform.position, targetDirection, out RaycastHit rayHit, 1000f, layerMask)) {
                ShipController raycastShipController = rayHit.transform.parent.parent.GetComponent<ShipController>();
                if (raycastShipController != null && raycastShipController.Equals(target)) {
                    return true;
                }
            }
        }

        return false;
    }

    public RaycastHit GetTurretToTargetHit () {
        Vector3 targetDirection;
            
        if (target == null) {
            targetDirection = Vector3.zero - transform.position;
        } else {
            targetDirection = target.transform.position - transform.position;
        }

        Physics.Raycast(transform.position, targetDirection, out RaycastHit rayHit, 1000f, layerMask);        
        return rayHit;
    }

    public void Fire() {        
        if (target != null && fireCountdown <= 0) {
            RaycastHit rayHit = GetTurretToTargetHit();
            GameObject laserObjInstance = Instantiate(laserPrefab, (barrelEnd[barrelIndex].position + rayHit.point) / 2, Quaternion.identity);
            Vector3 targetDirection = rayHit.point - barrelEnd[barrelIndex].position;
            laserObjInstance.transform.forward = targetDirection.normalized;

            laserObjInstance.transform.localScale = new Vector3(1, 1, Vector3.Distance(barrelEnd[barrelIndex].position, rayHit.point));
            Destroy(laserObjInstance, 0.8f);

            if (rayHit.collider.CompareTag("Shield")) {
                GameObject shieldHitInstance = Instantiate(shieldHitPrefab, rayHit.point, Quaternion.identity);
                shieldHitInstance.transform.forward = rayHit.normal;
                Destroy(shieldHitInstance, 2f);
            } else if (rayHit.collider.CompareTag("Hull")) {
                GameObject hullHitExplosionInstance = Instantiate(hullHitExplosionPrefab, rayHit.point, Quaternion.identity);
                hullHitExplosionInstance.transform.forward = rayHit.normal;
                Destroy(hullHitExplosionInstance, 0.8f);
            }

            barrelIndex = (barrelIndex + 1) % barrelEnd.Length;
            fireCountdown = rateOfFire;
        }
    }

 	private GameObject GetProjectile (List<GameObject> targetList, string targetPath) {
		GameObject projectileClone;

		projectileClone = RuntimeObjects.GetObject (targetList);
		if (projectileClone == null) {
			projectileClone = Instantiate ((GameObject) Resources.Load (targetPath));
		}

		return projectileClone;
	}

    //-----GIZMOS-----
    //public bool drawGizmos;
    private void OnDrawGizmos () {
        
	}


}
