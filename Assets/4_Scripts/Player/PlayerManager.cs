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

    [Header("Debug Variables")]
    public GameObject[] preExisitingShips;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    public void Initialise() {
        alliedShips = new HashSet<ShipController>();
        selectedShips = new HashSet<ShipController>();

        //alliedShips.Add(ShipFactory.instance.SpawnShip(Vector3.zero, Quaternion.identity, ShipFactory.SpawnRestriction.PLAYER, "Laser Frigate"));

        foreach (GameObject ship in preExisitingShips) {
            ShipController shipControl = ship.GetComponent<ShipController>();
            if (shipControl == null) {
                Debug.LogError("Non ship object placed in pre exisiting ships array");
            } else {
                alliedShips.Add(shipControl);
            }
        }

        foreach (ShipController shipControl in alliedShips) {
            shipControl.Initialise();
            shipControl.isAlliedShip = true;

            shipControl.transform.SetParent(gameObject.transform);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            CheckForNewShipSelection();
        } else if (Input.GetMouseButtonDown(1)) {
            MoveSelectedShips();
        }
    }

    #region Selection Management
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
    #endregion

    #region Ship Movement
    public void MoveSelectedShips () {
        int navPlaneLayerMask = 1 << 8;
        Physics.Raycast(CameraManager.instance.GetCameraRay(), out RaycastHit rayHit, 1000f, navPlaneLayerMask);

        if (Input.GetKey(KeyCode.LeftShift)) {
            foreach (ShipController shipControl in selectedShips) {
                shipControl.MovementController.AddMoveToPosition(rayHit.point, true);
            }
        } else {
            foreach (ShipController shipControl in selectedShips) {
                shipControl.MovementController.AddMoveToPosition(rayHit.point, false);
            }
        }        
    }
    #endregion

    //-----GIZMOS-----
    //public bool drawGizmos;
    void OnDrawGizmos() {

    }

}
