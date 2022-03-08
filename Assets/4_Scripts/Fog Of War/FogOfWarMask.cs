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
    private void Start() {
        FogOfWarManager.instance.AddNewShipMask(gameObject.transform, maskRadius);
    }

    public void MarkAsEnemy() {
        FogOfWarManager.instance.RemoveShipMask(gameObject.transform);
    }

    //-----GIZMOS-----
    public bool drawGizmos;

    private void OnDrawGizmos() {
        if (drawGizmos) {
            Gizmos.color = Color.white;
            GizmoExtensions.DrawWireCircle(transform.position, maskRadius);
        }
    }
}
