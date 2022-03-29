using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : ShipComponent
{

	public List<Transform> waypoints = new List<Transform>();

	private int waypointIndex;
	private Transform targetWaypoint;

	public bool loopPatrol = true;
	public bool startFromFirstWaypoint = false;
	private bool routeCompleted;

	private void Update()
	{
		if (routeCompleted)
			return;

		if (targetWaypoint == null)
		{
			targetWaypoint = startFromFirstWaypoint ? waypoints[0] : GetNearestWaypoint();
			waypointIndex = waypoints.IndexOf(targetWaypoint);
			Ship.Helm.SetWaypoint(targetWaypoint);
		}

		//Set course for the next waypoint in the loop
		if (Vector3.Distance(transform.position, targetWaypoint.position) < 60f)
		{
			if (loopPatrol)
			{
				waypointIndex = (waypointIndex + 1) % waypoints.Count;
				targetWaypoint = waypoints[waypointIndex];
			}
			else
			{
				waypointIndex++;
				if (waypointIndex == waypoints.Count)
				{
					targetWaypoint = null;
					routeCompleted = true;
				}
				else
				{
					targetWaypoint = waypoints[waypointIndex];
				}
			}

			Ship.Helm.SetWaypoint(targetWaypoint);
		}
	}

	private Transform GetNearestWaypoint()
	{
		KeyValuePair<Transform, float> nearestWaypoint = new KeyValuePair<Transform, float>(waypoints[0], float.MaxValue);

		foreach (Transform waypointTransform in waypoints)
		{
			float waypointDistance = Vector3.Distance(transform.position, waypointTransform.position);
			if (waypointDistance < nearestWaypoint.Value)
			{
				nearestWaypoint = new KeyValuePair<Transform, float>(waypointTransform, waypointDistance);
			}
		}

		return nearestWaypoint.Key;
	}

}
