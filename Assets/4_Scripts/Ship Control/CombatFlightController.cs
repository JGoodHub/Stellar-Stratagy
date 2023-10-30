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
    [SerializeField] protected float _minDistancePerTurn = 20;
    [SerializeField] protected float _maxDistancePerTurn = 140;
    [SerializeField] protected float _maxPrimaryAngleDeltaPerTurn = 50;
    [SerializeField] protected float _maxSecondaryAngleDeltaPerTurn = 20;

    [SerializeField] protected AnimationCurve _pathEasingCurve;

    protected PathUtils.BezierCurve4 _flightPath;

    public float MaxSecondaryAngleDeltaPerTurn => _maxSecondaryAngleDeltaPerTurn;

    public PathUtils.BezierCurve4 FlightPath => _flightPath;

    public void FollowFlightPath()
    {
        if (_flightPath == null)
            return;

        ClearTemporaryMarkers();

        Tweener tweener = DOVirtual.Float(0f, 1f, TurnController.Singleton.TurnRealtimeDuration, t =>
        {
            transform.position = _flightPath.GetPointOnCurve(t);
            transform.forward = _flightPath.GetDirectionOnCurve(t);
        });
        tweener.SetEase(_pathEasingCurve);
        tweener.OnComplete(() =>
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
                Vector3 endPoint = GetRotatedVector(origin, forward, angle, dist);
                flightPathEndPoints.Add(endPoint);
            }
        }

        return flightPathEndPoints;
    }

    public List<Vector3> GetPrimaryTrajectoryArea(Vector3 origin, Vector3 forward, int angleDivisions)
    {
        List<Vector3> outlinePositions = new List<Vector3>();

        float step = 1f / angleDivisions;
        for (float angle = -1f; angle <= 1.001f; angle += step)
        {
            Vector3 endPoint = GetRotatedVector(origin, forward, angle * _maxPrimaryAngleDeltaPerTurn, _maxDistancePerTurn, _maxPrimaryAngleDeltaPerTurn);
            outlinePositions.Add(endPoint);
        }

        for (float angle = 1f; angle >= -1.001f; angle -= step)
        {
            Vector3 endPoint = GetRotatedVector(origin, forward, angle * _maxPrimaryAngleDeltaPerTurn, 0f, _maxSecondaryAngleDeltaPerTurn);
            outlinePositions.Add(endPoint);
        }

        outlinePositions.Add(outlinePositions[0]);

        return outlinePositions;
    }

    public List<Vector3> GetSecondaryTrajectoryArea(Vector3 origin, Vector3 forward, int angleDivisions)
    {
        List<Vector3> outlinePositions = new List<Vector3>();

        float step = 1f / angleDivisions;
        for (float angle = -1f; angle <= 1.001f; angle += step)
        {
            Vector3 endPoint = GetRotatedVector(origin, forward, angle * _maxSecondaryAngleDeltaPerTurn, 0.3f * _maxDistancePerTurn, _maxSecondaryAngleDeltaPerTurn);
            outlinePositions.Add(endPoint);
        }

        for (float angle = 1f; angle >= -1.001f; angle -= step)
        {
            Vector3 endPoint = GetRotatedVector(origin, forward, angle * _maxSecondaryAngleDeltaPerTurn, 0f, _maxSecondaryAngleDeltaPerTurn);
            outlinePositions.Add(endPoint);
        }

        outlinePositions.Add(outlinePositions[0]);

        return outlinePositions;
    }

    public List<Vector3> GetTrajectoryLine()
    {
        return _flightPath == null ? new List<Vector3>() : _flightPath.Curve.ToList();
    }

    public Vector3 GetRotatedVector(Vector3 origin, Vector3 forward, float angleNormalised, float distanceNormalised)
    {
        angleNormalised = Mathf.Clamp(angleNormalised, -1f, 1f);
        distanceNormalised = Mathf.Clamp(distanceNormalised, _minDistancePerTurn / _maxDistancePerTurn, 1f);

        float targetAngle = _maxPrimaryAngleDeltaPerTurn * angleNormalised;
        float targetDistance = _maxDistancePerTurn * distanceNormalised;

        Vector3 targetDirection = Quaternion.Euler(0, targetAngle, 0) * forward.normalized;
        return origin + targetDirection * targetDistance;
    }

    public Vector3 GetRotatedVector(Vector3 origin, Vector3 forward, float angle, float distance, float maxAngle)
    {
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        distance = Mathf.Clamp(distance, _minDistancePerTurn, _maxDistancePerTurn);

        Vector3 targetDirection = Quaternion.Euler(0, angle, 0) * forward.normalized;
        return origin + targetDirection * distance;
    }

    public PathUtils.BezierCurve4 GetManualFlightPath(Vector3 origin, Vector3 forward, float angleNormalised, float distanceNormalised)
    {
        Vector3 end = GetRotatedVector(origin, forward, angleNormalised, distanceNormalised);

        return GetTargetedFlightPath(origin, forward, end);
    }

    public PathUtils.BezierCurve4 GetTargetedFlightPath(Vector3 origin, Vector3 forward, Vector3 target)
    {
        // Work out and clamp the angle and distance to the target
        float targetAngleDelta = Vector3.SignedAngle(forward, target - origin, Vector3.up);
        targetAngleDelta = Mathf.Clamp(targetAngleDelta, -_maxPrimaryAngleDeltaPerTurn, _maxPrimaryAngleDeltaPerTurn);

        float targetDistance = Mathf.Clamp((target - origin).magnitude, _minDistancePerTurn, _maxDistancePerTurn);

        if (Vector3.Dot(forward, target - origin) < 0f)
            targetDistance = _minDistancePerTurn;

        // Handles for the bezier curve
        Vector3 targetDirection = Quaternion.Euler(0, targetAngleDelta, 0) * forward.normalized;
        Vector3 end = origin + targetDirection * targetDistance;

        float handleOffset = (end - origin).magnitude * 0.34f;
        Vector3 startHandleOffset = forward * handleOffset;
        Vector3 endHandleOffset = (origin - end).normalized * handleOffset;

        PathUtils.BezierCurve4 flightPath = new PathUtils.BezierCurve4(origin, origin + startHandleOffset, end + endHandleOffset, end, 12);
        return flightPath;
    }

    // public void SetFlightPath(PathUtils.BezierCurve4 flightPath)
    // {
    //     _flightPath = flightPath;
    // }
    //
    // public bool IsOtherShipGoingRightRelative(CombatShipController otherShip)
    // {
    //     return Vector3.Dot(transform.right, otherShip.transform.forward) > 0f;
    // }
}