using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatFlightController : ShipComponent
{
    [SerializeField] protected float _maxDistancePerTurn = 200;
    [SerializeField] protected float _maxAngleDeltaPerTurn = 90;

    [SerializeField] protected AnimationCurve _pathEasingCurve;

    protected PathUtils.BezierCurve3 _flightPath;

    public void FollowFlightPath()
    {
        if (_flightPath == null)
            return;

        ClearTemporaryMarkers();


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

    public List<Vector3> GetPotentialTrajectoryArea(Vector3 origin, Vector3 forward, int angleDivisions)
    {
        List<Vector3> outlinePositions = new List<Vector3>();

        float step = 1f / angleDivisions;
        for (float angle = -1f; angle <= 1.001f; angle += step)
        {
            Vector3 endPoint = GetFlightPathEnd(origin, forward, angle, 1f);
            outlinePositions.Add(endPoint);
        }

        for (float angle = 1f; angle >= -1.001f; angle -= step)
        {
            Vector3 endPoint = GetFlightPathEnd(origin, forward, angle, 0f);
            outlinePositions.Add(endPoint);
        }

        outlinePositions.Add(outlinePositions[0]);

        return outlinePositions;
    }

    public List<Vector3> GetTrajectoryLine()
    {
        return _flightPath == null ? new List<Vector3>() : _flightPath.Curve.ToList();
    }

    public Vector3 GetFlightPathEnd(Vector3 origin, Vector3 forward, float angleDeltaNormalised, float distanceNormalised)
    {
        angleDeltaNormalised = Mathf.Clamp(angleDeltaNormalised, -1f, 1f);
        distanceNormalised = Mathf.Clamp(distanceNormalised, 0.1f, 1f);

        float targetAngle = _maxAngleDeltaPerTurn * angleDeltaNormalised;
        float targetDistance = _maxDistancePerTurn * distanceNormalised;

        Vector3 targetDirection = Quaternion.Euler(0, targetAngle, 0) * forward.normalized;
        return origin + targetDirection * targetDistance;
    }

    public PathUtils.BezierCurve3 GetManualFlightPath(Vector3 origin, Vector3 forward, float angleNormalised, float flightDistanceNormalised)
    {
        Vector3 end = GetFlightPathEnd(origin, forward, angleNormalised, flightDistanceNormalised);

        return GetTargetedFlightPath(origin, forward, end);
    }

    public PathUtils.BezierCurve3 GetTargetedFlightPath(Vector3 origin, Vector3 forward, Vector3 target)
    {
        float targetAngleDelta = Vector3.SignedAngle(forward, target - origin, Vector3.up);
        targetAngleDelta = Mathf.Clamp(targetAngleDelta, -_maxAngleDeltaPerTurn, _maxAngleDeltaPerTurn);

        float targetDistance = Mathf.Clamp((target - origin).magnitude, _maxDistancePerTurn * 0.1f, _maxDistancePerTurn);

        Vector3 targetDirection = Quaternion.Euler(0, targetAngleDelta, 0) * forward.normalized;
        Vector3 end = origin + targetDirection * targetDistance;

        float forwardHandleOffset = (end - origin).magnitude / 2f;
        PathUtils.BezierCurve3 flightPath = PathUtils.GetBezierCurve(origin, origin + forward * forwardHandleOffset, end, 12);

        return flightPath;
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