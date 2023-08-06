using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatFlightController : ShipComponent
{
    [SerializeField] protected float _maxForwardFlightDistancePerTurn = 200;
    [SerializeField] protected float _maxTurnFlightDistancePerTurn = 50;
    [SerializeField] protected float _maxAngleDeltaPerTurn = 90;

    [SerializeField] protected AnimationCurve _pathEasingCurve;

    protected PathUtils.BezierCurve3 _flightPath;

    public void FollowFlightPath()
    {
        if (_flightPath == null)
            return;

        ClearTemporaryMarkers();

        //_flightPathRenderer.positionCount = 0;
        //_ghostShipGO.transform.position = Vector3.up * 1000;

        DOVirtual.Float(0f, 1f, TurnController.Instance.TurnRealtimeDuration, t =>
        {
            transform.position = PathUtils.GetPointOnBezierCurve(_flightPath.Start, _flightPath.Mid, _flightPath.End, t);
            transform.forward = PathUtils.GetDirectionOnBezierCurve(_flightPath.Start, _flightPath.Mid, _flightPath.End, t);
        }).SetEase(_pathEasingCurve).OnComplete(() =>
        {
            _flightPath = null;
        });
    }

    protected virtual void ClearTemporaryMarkers()
    {
    }

    public List<Vector3> GetAllEndPositions(Vector3 origin, Vector3 forward)
    {
        List<Vector3> flightPathEndPoints = new List<Vector3>();

        for (float angle = -1f; angle <= 1f; angle += 0.25f)
        {
            for (float dist = 0f; dist <= 1f; dist += 0.25f)
            {
                Vector3 endPoint = GetFlightPathEnd(origin, forward, angle, dist);
                flightPathEndPoints.Add(endPoint);
            }
        }

        return flightPathEndPoints;
    }

    public List<Vector3> GetFlightPathOutline(Vector3 origin, Vector3 forward, int angleDivisions)
    {
        List<Vector3> outlinePositions = new List<Vector3>();

        outlinePositions.Add(transform.position);

        float step = 1f / angleDivisions;
        for (float angle = -1f; angle <= 1.001f; angle += step)
        {
            Vector3 endPoint = GetFlightPathEnd(origin, forward, angle, 1f);
            outlinePositions.Add(endPoint);
        }

        outlinePositions.Add(transform.position);

        return outlinePositions;
    }

    public PathUtils.BezierCurve3 GetManualFlightPath(Vector3 origin, Vector3 forward, float angleNormalised, float flightDistanceNormalised)
    {
        Vector3 end = GetFlightPathEnd(origin, forward, angleNormalised, flightDistanceNormalised);

        float forwardHandleOffset = (end - origin).magnitude / 2f;
        PathUtils.BezierCurve3 flightPath = PathUtils.GetBezierCurve(origin, origin + forward * forwardHandleOffset, end, 12);

        return flightPath;
    }

    // public PathUtils.BezierCurve3 GetManualFlightPath(Vector3 origin, Vector3 forward, Vector3 end)
    // {
    //     float forwardHandleOffset = (end - origin).magnitude / 2f;
    //     PathUtils.BezierCurve3 flightPath = PathUtils.GetBezierCurve(origin, origin + forward * forwardHandleOffset, end, 12);
    //
    //     return flightPath;
    // }

    public Vector3 GetFlightPathEnd(Vector3 origin, Vector3 forward, float angleNormalised, float flightDistanceNormalised)
    {
        angleNormalised = Mathf.Clamp(angleNormalised, -1f, 1f);
        flightDistanceNormalised = Mathf.Clamp01(flightDistanceNormalised);

        float targetAngle = _maxAngleDeltaPerTurn * angleNormalised;
        float maxPossibleFlightDistance = GetMaxFlightDistanceForNormalisedAngle(angleNormalised);
        float targetDistance = maxPossibleFlightDistance * flightDistanceNormalised;

        Vector3 targetDirection = Quaternion.Euler(0, targetAngle, 0) * forward.normalized;
        return origin + targetDirection * targetDistance;
    }

    public PathUtils.BezierCurve3 GetTargetedFlightPath(Vector3 origin, Vector3 forward, Vector3 target)
    {
        float targetAngle = Vector3.SignedAngle(forward, target - origin, Vector3.up);
        targetAngle = Mathf.Clamp(targetAngle, -_maxAngleDeltaPerTurn, _maxAngleDeltaPerTurn);

        float targetDistance = Mathf.Min((target - origin).magnitude, GetMaxFlightDistanceForNormalisedAngle(targetAngle / _maxAngleDeltaPerTurn));

        Vector3 targetDirection = Quaternion.Euler(0, targetAngle, 0) * forward.normalized;
        Vector3 end = origin + targetDirection * targetDistance;

        float forwardHandleOffset = (end - origin).magnitude / 2f;
        PathUtils.BezierCurve3 flightPath = PathUtils.GetBezierCurve(origin, origin + forward * forwardHandleOffset, end, 12);

        return flightPath;
    }

    public float GetMaxFlightDistanceForNormalisedAngle(float angleNormalised)
    {
        return _maxTurnFlightDistancePerTurn + ((_maxForwardFlightDistancePerTurn - _maxTurnFlightDistancePerTurn) * (1f - Mathf.Abs(angleNormalised)));
    }

    public void SetFlightPath(PathUtils.BezierCurve3 flightPath)
    {
        _flightPath = flightPath;
    }

    public bool IsOtherShipGoingRightRelative(CombatShipController otherShip)
    {
        return Vector3.Dot(transform.right, otherShip.transform.forward) > 0f;
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