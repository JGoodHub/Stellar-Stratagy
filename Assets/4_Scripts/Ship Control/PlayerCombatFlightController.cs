using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombatFlightController : CombatFlightController, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private GameObject _ghostShipPrefab;

    private GameObject _ghostShipGO;

    private void Start()
    {
        _ghostShipGO = Instantiate(_ghostShipPrefab, Vector3.up * 5000, Quaternion.identity);
    }

    protected override void ClearTemporaryMarkers()
    {
        _ghostShipGO.transform.position = Vector3.up * 1000;
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

        List<Vector3> potentialTrajectoryArea = GetPotentialTrajectoryArea(transform.position, transform.forward, 14);
        MovementEffectsController.Instance.ShowMoveableArea(potentialTrajectoryArea);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Instance.OurTurn == false || PlayerCombatController.Instance.FocusedShip != ShipController)
            return;

        Vector3 navPlanePoint = NavigationPlane.RaycastNavPlane();

        _flightPath = GetTargetedFlightPath(transform.position, transform.forward, navPlanePoint);

        Vector3 _flightPathEndDirection = PathUtils.GetDirectionOnBezierCurve(_flightPath.Start, _flightPath.Mid, _flightPath.End, 1f);

        _ghostShipGO.transform.position = _flightPath.End;
        _ghostShipGO.transform.forward = _flightPathEndDirection;

        List<Vector3> trajectoryLine = GetTrajectoryLine();
        MovementEffectsController.Instance.ShowFlightTrajectory(trajectoryLine);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PlayerCombatController.Instance.OurTurn == false || PlayerCombatController.Instance.FocusedShip != ShipController)
            return;

        CameraDragController.Instance.DragEnabled = true;

        MovementEffectsController.Instance.ClearMoveableArea();
        MovementEffectsController.Instance.ClearPathLine();
    }

    private void OnDrawGizmos()
    {
        return;

        for (float angle = -1f; angle <= 1f; angle += 0.0999999f)
        {
            for (float distance = 0.1f; distance <= 1f; distance += 0.0999999f)
            {
                PathUtils.BezierCurve3 manualFlightPath = GetManualFlightPath(transform.position, transform.forward, angle, distance);

                Gizmos.color = Color.green;

                Gizmos.DrawSphere(manualFlightPath.End, 2f);

                Gizmos.DrawRay(manualFlightPath.End, manualFlightPath.GetTangent(1f).normalized * 4f);
            }
        }
    }
}