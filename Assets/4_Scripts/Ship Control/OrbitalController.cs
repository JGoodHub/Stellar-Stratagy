using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrbitalController : ShipComponent
{
	public float orbitRadius;
	public int pointsCount;
	public Transform pointsParent;
	private List<Transform> waypoints = new List<Transform>();

	private OrbitalController orbitTarget;
	private Transform targetWaypoint;

	private void Awake()
	{
		CreateOrbitWaypointObjects();
	}

	private void Update()
	{
		if (orbitTarget == null)
			return;

		if (targetWaypoint == null)
		{
			targetWaypoint = orbitTarget.GetOrbitInsertionPoint(transform, true).Key;

			Ship.Helm.SetWaypoint(targetWaypoint);
			//Ship.Helm.SetSpeed(3);
		}

		//Set course for the next waypoint in the loop
		if (Vector3.Distance(transform.position, targetWaypoint.position) < 60f)
		{
			targetWaypoint = orbitTarget.GetNextWaypoint(targetWaypoint, true);

			Ship.Helm.SetWaypoint(targetWaypoint);
			//Ship.Helm.SetSpeed(3);
		}
	}

	public void SetOrbitTarget(OrbitalController otherController)
	{
		orbitTarget = otherController;

		if (orbitTarget == null)
			targetWaypoint = null;
		else
			Update();
	}

	public KeyValuePair<Transform, float> GetOrbitInsertionPoint(Transform shipTransform, bool clockwiseInsertion)
	{
		Vector3 shipToCentreDirection = transform.position - shipTransform.position;
		float shipTheta = Mathf.Asin(orbitRadius / shipToCentreDirection.magnitude) * Mathf.Rad2Deg;
		float orbitTheta = 90f - shipTheta;
		Vector3 pointOnOrbit = shipToCentreDirection.normalized * -orbitRadius;
		pointOnOrbit = Quaternion.Euler(0f, clockwiseInsertion ? orbitTheta : -orbitTheta, 0f) * pointOnOrbit;
		pointOnOrbit += transform.position;

		KeyValuePair<Transform, float> closestWaypoint = new KeyValuePair<Transform, float>(waypoints[0], float.MaxValue);

		foreach (Transform waypoint in waypoints)
		{
			float dist = Vector3.Distance(pointOnOrbit, waypoint.position);
			if (dist < closestWaypoint.Value)
			{
				closestWaypoint = new KeyValuePair<Transform, float>(waypoint, dist);
			}
		}

		return closestWaypoint;
	}

	public Transform GetNextWaypoint(Transform currentWaypoint, bool clockwise)
	{
		for (int i = 0; i < waypoints.Count; i++)
		{
			if (currentWaypoint == waypoints[i])
			{
				return waypoints[(i + 1) % waypoints.Count];
			}
		}

		return null;
	}

	private void CreateOrbitWaypointObjects()
	{
		int index = 0;
		foreach (Vector3 orbitPointPosition in GetWorldSpaceOrbitPoints())
		{
			GameObject orbitPoint = new GameObject($"OrbitPoint({index})");
			orbitPoint.transform.parent = pointsParent;
			orbitPoint.transform.position = orbitPointPosition;

			waypoints.Add(orbitPoint.transform);
			index++;
		}
	}

	private IEnumerable<Vector3> GetWorldSpaceOrbitPoints()
	{
		List<Vector3> orbitPoints = new List<Vector3>();

		float angleStep = 360f / pointsCount;
		Vector3 startDirection = transform.forward * orbitRadius;

		for (int i = 0; i < pointsCount; i++)
		{
			Vector3 rotatedDirection = Quaternion.Euler(0, angleStep * i, 0) * startDirection;
			orbitPoints.Add(rotatedDirection + transform.position);
		}

		return orbitPoints;
	}


	//-----GIZMOS-----
	public bool drawGizmos;

	private void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.color = Color.blue;

			foreach (Vector3 orbitPointPosition in GetWorldSpaceOrbitPoints())
			{
				Gizmos.DrawSphere(orbitPointPosition, 1.5f);
			}
		}
	}

}
