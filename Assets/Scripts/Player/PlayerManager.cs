using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //-----SINGLETON SETUP-----

    public static PlayerManager instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    //-----VARIABLES-----

    public HashSet<ShipController> alliedShips;
    public HashSet<ShipController> selectedShips;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    public void Initialise() {
        alliedShips = new HashSet<ShipController>();
        selectedShips = new HashSet<ShipController>();

        foreach (GameObject shipObject in GameObject.FindGameObjectsWithTag("Ship")) {
            if (shipObject.layer == 9) {
                alliedShips.Add(shipObject.GetComponent<ShipController>());
            }
        }

        foreach (ShipController ship in alliedShips) {
            ship.Initialise();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            CheckForNewShipSelection();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheckForNewShipSelection() {
        RaycastHit rayHit = CameraManager.instance.FrameRayHit;
        if (rayHit.collider.tag == "Ship") {
            ShipController newlySelectShip = rayHit.collider.gameObject.GetComponent<ShipController>();
            if (Input.GetKey(KeyCode.LeftShift)) {
                AddShipToSelection(newlySelectShip);
            } else if (Input.GetKey(KeyCode.LeftAlt)) {
                RemoveShipFromSelection(newlySelectShip);
            } else {
                ClearShipSelection();
                AddShipToSelection(newlySelectShip);
            }
        } else {
            ClearShipSelection();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newShip"></param>
    private void AddShipToSelection(ShipController newShip) {
        if (alliedShips.Contains(newShip)) {
            selectedShips.Add(newShip);
            newShip.SelectShip();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oldShip"></param>
    private void RemoveShipFromSelection(ShipController oldShip) {
        if (alliedShips.Contains(oldShip)) {
            selectedShips.Remove(oldShip);
            oldShip.DeselectShip();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearShipSelection() {
        foreach (ShipController ship in selectedShips) {
            ship.DeselectShip();
        }

        selectedShips.Clear();
    }

    //-----GIZMOS-----
    //public bool drawGizmos;
    void OnDrawGizmos() {

    }

}
