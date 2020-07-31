using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{

    public ShipController playerShip;
    private Entity selectedEntity;

    public void Start()
    {
        SelectEntity(playerShip);
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
        RaycastHit rayHit;
        if (CameraController.Instance.GetCameraRaycast(out rayHit))
        {
            selectedEntity?.SetRingActive(false);
            selectedEntity = null;

            if (rayHit.transform.TryGetComponent(out selectedEntity))
            {
                selectedEntity.SetRingActive(true);
            }
            else
            {
                selectedEntity = playerShip;
            }
        }
        else
        {
            selectedEntity = playerShip;
        }
    }

    public void SelectEntity(Entity entity)
    {
        if (selectedEntity != null)
        {
            selectedEntity.SetRingActive(false);
        }

        selectedEntity = entity;
        selectedEntity.SetRingActive(true);
    }


    public void MoveShip()
    {
        int navPlaneLayerMask = 1 << LayerMask.NameToLayer("NavPlane");
        Physics.Raycast(CameraController.Instance.GetCameraRay(), out RaycastHit rayHit, 1000f, navPlaneLayerMask);

        playerShip.MovementController.AddMoveToPosition(rayHit.point, false);
    }

}
