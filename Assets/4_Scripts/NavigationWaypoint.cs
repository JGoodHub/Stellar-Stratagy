using System;
using System.Collections.Generic;
using UnityEngine;

public class NavigationWaypoint : MonoBehaviour
{

	public string id;
	public List<NavigationWaypoint> connectedWaypoints;

	public bool drawGizmos = true;

	private void OnDrawGizmos()
	{
		if (drawGizmos == false)
			return;

		Gizmos.color = Color.cyan;

		foreach (NavigationWaypoint waypoint in connectedWaypoints)
		{
			Gizmos.DrawLine(transform.position, waypoint.transform.position);
		}
	}

	public virtual void GetTargetPositionAndAlignment(out Vector3 position, out Vector3 forward, Transform source = null)
	{
		position = transform.position;
		forward = transform.forward;
	}

}
