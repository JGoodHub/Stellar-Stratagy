using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowFlightController : ShipComponent
{

	// public float turnAngleThreshold = 30;
	// public float turnAwayDistance = 80;
	// public float turnTowardsDistance = 100;

	public ShipController targetShip;
	public Transform targetWaypoint;
	public Transform leftWaypoint;
	public Transform rightWaypoint;

	private void Start()
	{
		//Create the waypoint for the other ship to track
		leftWaypoint = new GameObject($"[LEFT_WAYPOINT]_{Ship.name}").transform;
		leftWaypoint.parent = transform;
		leftWaypoint.localPosition = Vector3.left * 80f + Vector3.back * 30f;
		leftWaypoint.localRotation = Quaternion.identity;

		rightWaypoint = new GameObject($"[RIGHT_WAYPOINT]_{Ship.name}").transform;
		rightWaypoint.parent = transform;
		rightWaypoint.localPosition = Vector3.right * 80f + Vector3.back * 30f;
		rightWaypoint.localRotation = Quaternion.identity;
	}

	private void Update()
	{
		if (targetShip == null)
			return;

		// if (targetWaypoint == null)
		// {
		// 	//Check if the other ship is already targeting us
		// 	if (targetShip.FollowFlightController.targetShip == Ship)
		// 	{
		// 		if (targetShip.FollowFlightController.targetWaypoint == leftWaypoint)
		// 		{
		// 			targetWaypoint = targetShip.FollowFlightController.leftWaypoint;
		// 		}
		// 		else
		// 		{
		// 			targetWaypoint = targetShip.FollowFlightController.rightWaypoint;
		// 		}
		// 	}
		// 	else
		// 	{
		// 		float dot = Vector3.Dot(transform.right, targetShip.Helm.TargetDirection);
		//
		// 		if (Mathf.Abs(dot) < 0.1f)
		// 		{
		// 			targetWaypoint = Random.Range(-1f, 1f) > 0f ? targetShip.FollowFlightController.rightWaypoint : targetShip.FollowFlightController.leftWaypoint;
		// 		}
		// 		else
		// 		{
		// 			targetWaypoint = dot > 0f ? targetShip.FollowFlightController.rightWaypoint : targetShip.FollowFlightController.leftWaypoint;
		// 		}
		// 	}
		// 	
		// 	Ship.Helm.SetWaypoint(targetWaypoint);
		//
		// }

		/*
		float angleToShip = Vector3.Angle(transform.forward, targetShip.transform.position - transform.position);
		float distanceToShip = Vector3.Distance(transform.position, targetShip.transform.position);

		if (distanceToShip < turnAwayDistance)
			Ship.Helm.SetSpeedCap(3);
		else if (distanceToShip < turnTowardsDistance)
			Ship.Helm.SetSpeedCap(4);

		if (distanceToShip > turnTowardsDistance)
		{
			Ship.Helm.SetWaypoint(targetShip.transform);
		}
		else if (distanceToShip < turnAwayDistance)
		{
			if (Mathf.Abs(angleToShip) < turnAngleThreshold)
			{
				float dot = Vector3.Dot(transform.right, targetShip.Helm.TargetDirection);

				if (Mathf.Abs(dot) < 0.15f)
				{
					Vector3 turnDirection = Random.Range(-1f, 1f) > 0f ? transform.right : -transform.right;
					Ship.Helm.SetHeadingDirection(turnDirection);
				}
				else
				{

					Vector3 turnDirection = dot > 0f ? -transform.right : transform.right;
					Ship.Helm.SetHeadingDirection(turnDirection);
				}
			}
		}
		*/
	}

	public void SetTarget(ShipController ship)
	{
		targetShip = ship;
	}

}
