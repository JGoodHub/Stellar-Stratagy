using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        if (PlayerCombatController.Instance.OurTurn == false)
            return;

        if (PlayerCombatController.Instance.FocusedShip != ShipController || SelectionController.Instance.SelectedEntity == null)
        {
            SelectionController.Instance.SetSelection(ShipController);
        }

        _flightPath = null;

        CameraDragController.Instance.DragEnabled = false;

        List<Vector3> potentialTrajectoryArea = GetPrimaryTrajectoryArea(transform.position, transform.forward, 14);
        TurnEffectsController.Instance.ShowMoveableArea(potentialTrajectoryArea);

        _ghostShip.SetInteractable(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Instance.OurTurn == false || PlayerCombatController.Instance.FocusedShip != ShipController)
            return;

        Vector3 navPlanePoint = NavigationPlane.RaycastNavPlane();

        _flightPath = GetTargetedFlightPath(transform.position, transform.forward, navPlanePoint);

        Vector3 _flightPathEndDirection = _flightPath.GetDirectionOnCurve(1f);

        _ghostShip.transform.position = _flightPath.D;
        _ghostShip.transform.forward = _flightPathEndDirection;

        List<Vector3> trajectoryLine = GetTrajectoryLine();
        TurnEffectsController.Instance.ShowFlightTrajectory(trajectoryLine);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Instance.OurTurn == false || PlayerCombatController.Instance.FocusedShip != ShipController)
            return;

        CameraDragController.Instance.DragEnabled = true;

        TurnEffectsController.Instance.ClearMoveableArea();
        TurnEffectsController.Instance.ClearPathLine();

        _ghostShip.SetInteractable(true);
    }
}