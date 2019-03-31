using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    //-----VARIABLES-----

    private MovementController movementController;
    private ShipData shipData;

    [SerializeField]
    private GameObject selectionRing;

    //-----METHODS-----
	
    /// <summary>
    /// 
    /// </summary>
	public void Initialise() {
        movementController = GetComponent<MovementController>();
        shipData = GetComponent<ShipData>();
        DeselectShip();
	}

    public void SelectShip () {
        selectionRing.SetActive(true);
    }

    public void DeselectShip () {
        selectionRing.SetActive(false);
    }
	
	//-----GIZMOS-----
	//public bool drawGizmos;
	void OnDrawGizmos() {
	
	}
	
}
