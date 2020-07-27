using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{

    public ShipController playerShip;
    private ShipController selectedShip;

    public void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForNewSelection();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MoveShip();
        }
    }


    public void CheckForNewSelection()
    {
        RaycastHit rayHit = CameraController.Instance.FrameRayHit;

    }


    public void MoveShip()
    {
        int navPlaneLayerMask = 1 << LayerMask.NameToLayer("NavPlane");
        Physics.Raycast(CameraController.Instance.GetCameraRay(), out RaycastHit rayHit, 1000f, navPlaneLayerMask);

        playerShip.MovementController.AddMoveToPosition(rayHit.point, false);
    }

}
