using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {

    public ShipController playerShip;


    public void Start() {
        SelectionController.Instance.OnSelectionTriggered += OnSelectionTriggered;
    }

    private void OnSelectionTriggered(object sender, Entity selectedEntity) {
        if (selectedEntity == null) {
            MoveShip();
        }
    }

    private void Update() {

    }

    public void MoveShip() {
        Vector3 target = NavigationPlane.Instance.RaycastNavPlane3D();
        playerShip.FlightController.SetHeading(target);
    }

}
