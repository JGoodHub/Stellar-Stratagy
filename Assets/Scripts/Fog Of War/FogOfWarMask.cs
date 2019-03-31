using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarMask : MonoBehaviour {

    //-----VARIABLES-----

	public float maskRadius;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    public void Start () {
        FogOfWarManager.instance.AddNewShipMask(gameObject.transform, maskRadius);
	}
	
    /// <summary>
    /// 
    /// </summary>
	void Update () {
		Collider[] overlapColliders = Physics.OverlapSphere(transform.position, maskRadius);
		for (int i = 0; i < overlapColliders.Length; i++) {
			if (overlapColliders [i].tag == "Enemy Root") {
				FogOfWarEnemy FoWEnemy = overlapColliders [i].GetComponent<FogOfWarEnemy> ();
				if (FoWEnemy.IsVisible == false) {
					FoWEnemy.Reveal ();
				}

				FoWEnemy.ResetHideCountdown ();
			}

		}
		
	}

    //-----GIZMOS-----
    public bool drawGizmos;
	void OnDrawGizmos () {
        if (drawGizmos) {
            Gizmos.color = Color.white;
            GizmoExtras.GetWireCircle(maskRadius, transform.position);            
        }
	}
}
