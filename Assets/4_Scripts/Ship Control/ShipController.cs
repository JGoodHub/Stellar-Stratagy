using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    //-----VARIABLES-----

    private MovementController movementController;
    public MovementController MovementController { get => movementController; }

    private TargetController targetController;

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
        targetController = GetComponent<TargetController>();
        shipData = GetComponent<ShipData>();
        fogMask = GetComponent<FogOfWarMask>();

        targetController.Initialise();
        fogMask.Initialise();

        DeselectShip();
	}

    /// <summary>
    /// 
    /// </summary>
    public void SelectShip () {
        selectionRing.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void DeselectShip () {
        selectionRing.SetActive(false);
    }
	
	//-----GIZMOS-----
	//public bool drawGizmos;
	void OnDrawGizmos() {
	
	}
	
}
