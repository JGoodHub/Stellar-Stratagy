using System;
using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipGhostHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CombatShipController _shipController;

    private Vector3 _initialEndForward;

    private Vector3 _leftClampedDirection;
    private Vector3 _rightClampedDirection;

    private void Start()
    {
        SetInteractable(false);
    }

    public void Initialise(CombatShipController flightController)
    {
        _shipController = flightController;
    }

    public void SetInteractable(bool isInteractable)
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = isInteractable;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Singleton.OurTurn == false)
            return;

        if (PlayerCombatController.Singleton.FocusedShip != _shipController || SelectionController.Singleton.SelectedEntity == null)
        {
            SelectionController.Singleton.SetSelection(_shipController);
        }

        CameraDragController.Singleton.DragEnabled = false;

        List<Vector3> secondaryTrajectoryArea = _shipController.FlightController.GetSecondaryTrajectoryArea(transform.position, transform.forward, 14);
        TurnEffectsController.Singleton.ShowMoveableArea(secondaryTrajectoryArea);

        _initialEndForward = transform.forward;

        _leftClampedDirection = Quaternion.Euler(0, _shipController.FlightController.MaxSecondaryAngleDeltaPerTurn * -1f, 0) * _initialEndForward;
        _rightClampedDirection = Quaternion.Euler(0, _shipController.FlightController.MaxSecondaryAngleDeltaPerTurn, 0) * _initialEndForward;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Singleton.OurTurn == false || PlayerCombatController.Singleton.FocusedShip != _shipController)
            return;

        Vector3 navPlanePoint = NavigationPlane.RaycastNavPlane();

        PathUtils.BezierCurve4 flightPath = _shipController.FlightController.FlightPath;
        float handleDistance = flightPath.DirectLength() * 0.34f;
        Vector3 newEndHandleOffset = (flightPath.D - navPlanePoint).normalized * handleDistance;

        // Clamp within the max range
        float secondaryAngleDelta = Vector3.SignedAngle(_initialEndForward, navPlanePoint - transform.position, Vector3.up);

        if (secondaryAngleDelta < _shipController.FlightController.MaxSecondaryAngleDeltaPerTurn * -1f)
            flightPath.C = flightPath.D + (-_leftClampedDirection * handleDistance);
        else if (secondaryAngleDelta > _shipController.FlightController.MaxSecondaryAngleDeltaPerTurn)
            flightPath.C = flightPath.D + (-_rightClampedDirection * handleDistance);
        else
            flightPath.C = flightPath.D + newEndHandleOffset;

        flightPath.RecalculateCurve();

        transform.forward = flightPath.GetDirectionOnCurve(1f);

        List<Vector3> trajectoryLine = _shipController.FlightController.GetTrajectoryLine();
        TurnEffectsController.Singleton.ShowFlightTrajectory(trajectoryLine);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CameraDragController.Singleton.DragEnabled = true;

        TurnEffectsController.Singleton.ClearMoveableArea();
        TurnEffectsController.Singleton.ClearPathLine();
    }
}