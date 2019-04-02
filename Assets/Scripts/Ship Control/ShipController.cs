using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    //-----VARIABLES-----

    private MovementController movementController;
    public MovementController MovementController { get => movementController; }

    private ShipData shipData;

    private FogOfWarMask fogMask;
    public FogOfWarMask FogMask { get => fogMask; }

    [HideInInspector]
    public bool isAlliedShip;

    [SerializeField]
    private GameObject selectionRing;

    //-----METHODS-----
	
    /// <summary>
    /// 
    /// </summary>
	public void Initialise() {
        movementController = GetComponent<MovementController>();
        shipData = GetComponent<ShipData>();
        fogMask = GetComponent<FogOfWarMask>();

        fogMask.Initialise();

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
