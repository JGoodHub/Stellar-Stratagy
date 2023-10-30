using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GoodHub.Core.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombatFlightController : CombatFlightController, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private GameObject _ghostShipPrefab;

    private ShipGhostHandler _ghostShip;

    private void Start()
    {
        _ghostShip = Instantiate(_ghostShipPrefab, Vector3.up * 5000, Quaternion.identity).GetComponent<ShipGhostHandler>();
        _ghostShip.Initialise(ShipController);
    }

    protected override void ClearTemporaryMarkers()
    {
        _ghostShip.transform.position = Vector3.up * 1000;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Singleton.OurTurn == false)
            return;

        if (PlayerCombatController.Singleton.FocusedShip != ShipController || SelectionController.Singleton.SelectedEntity == null)
        {
            SelectionController.Singleton.SetSelection(ShipController);
        }

        _flightPath = null;

        CameraDragController.Singleton.DragEnabled = false;

        List<Vector3> potentialTrajectoryArea = GetPrimaryTrajectoryArea(transform.position, transform.forward, 14);
        TurnEffectsController.Singleton.ShowMoveableArea(potentialTrajectoryArea);

        _ghostShip.SetInteractable(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Singleton.OurTurn == false || PlayerCombatController.Singleton.FocusedShip != ShipController)
            return;

        Vector3 navPlanePoint = NavigationPlane.RaycastNavPlane();

        _flightPath = GetTargetedFlightPath(transform.position, transform.forward, navPlanePoint);

        Vector3 _flightPathEndDirection = _flightPath.GetDirectionOnCurve(1f);

        _ghostShip.transform.position = _flightPath.D;
        _ghostShip.transform.forward = _flightPathEndDirection;

        List<Vector3> trajectoryLine = GetTrajectoryLine();
        TurnEffectsController.Singleton.ShowFlightTrajectory(trajectoryLine);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Singleton.OurTurn == false || PlayerCombatController.Singleton.FocusedShip != ShipController)
            return;

        CameraDragController.Singleton.DragEnabled = true;

        TurnEffectsController.Singleton.ClearMoveableArea();
        TurnEffectsController.Singleton.ClearPathLine();

        _ghostShip.SetInteractable(true);
    }
}