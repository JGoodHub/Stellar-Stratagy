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
    public void Initialise () {
        FogOfWarManager.instance.AddNewShipMask(gameObject.transform, maskRadius);
	}

    public void MarkAsEnemy () {
        FogOfWarManager.instance.RemoveShipMask(gameObject.transform);
    }

    //-----GIZMOS-----
    public bool drawGizmos;
	void OnDrawGizmos () {
        if (drawGizmos) {
            Gizmos.color = Color.white;
            GizmoExtras.DrawWireCircle(maskRadius, transform.position);            
        }
	}
}
