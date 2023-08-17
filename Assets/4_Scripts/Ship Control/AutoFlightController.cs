using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class AutoFlightController : ShipComponent
{
	//-----VARIABLES-----

	[Header("Thrust")]
	[SerializeField] private float maxSpeed;
	private float currentSpeed;
	private float targetSpeed;

	[Header("Rotation")]
	[SerializeField] private GameObject directionRingObject;

	[SerializeField] private AnimationCurve rotationDecelerationCurve;
	[SerializeField] private AnimationCurve rotationCurve;
	[SerializeField] private float rotSpeed;

	[Header("Debug")]
	public bool drawGizmos;

	private NavigationPath path;
	private NavigationWaypoint waypoint;
	private List<Vector3> flightPath;
	
	private Transform waypointTransform;
	private Quaternion targetRotation;

	private Transform shipCompass;
	private Rigidbody _rigidbody;

	// PROPERTIES

	public Vector3 TargetDirection => targetRotation * Vector3.forward;

	// METHODS

	private void Awake()
	{
		shipCompass = new GameObject(name + "_[COMPASS]").transform;
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void FlyToPositionAndAlignToDirection(Vector3 position, Vector3 forward)
	{
		
	}

	private void Update()
	{
		
		
		
		
		//Update our target direction to point to the waypoint
		if (waypoint != null)
		{
			Vector3 direction = waypointTransform.position - transform.position;
			direction.y = 0;

			transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
		}

		//Turning
		if (Quaternion.Angle(transform.rotation, targetRotation) >= 0.025f)
		{
			Quaternion rotation = transform.rotation;
			float degreesToTarget = Quaternion.Angle(rotation, targetRotation);

			//currentRotSpeed = Mathf.Clamp(currentRotSpeed + (rotationAcceleration * Time.deltaTime), 0, peakRotationSpeed);

			//rotation = Quaternion.RotateTowards(rotation, targetRotation, currentRotSpeed * rotationDecelerationCurve.Evaluate(degreesToTarget) * Time.deltaTime);
			transform.rotation = rotation;
		}
		else
		{
			transform.rotation = targetRotation;
			//currentRotSpeed = 0;
		}

		if (directionRingObject != null)
			directionRingObject.transform.rotation = Quaternion.Lerp(directionRingObject.transform.rotation, Quaternion.LookRotation(targetRotation * Vector3.forward, Vector3.up), 0.34f);

		
	}

	public void FollowNavigationPath(NavigationPath path)
	{
		this.path = path;
		waypoint = path.Waypoints[1];
		waypoint.GetTargetPositionAndAlignment(out Vector3 targetPosition, out Vector3 targetForward);
		// flightPath = PathUtils.GetBezierCurve(
		// 	transform.position, 
		// 	transform.position + transform.forward * 200, 
		// 	targetPosition - targetForward * 200, 
		// 	targetPosition, 
		// 	10);

		Debug.DrawRay(transform.position, Vector3.up * 5, Color.green, 5f);
		Debug.DrawRay(transform.position + transform.forward * 50, Vector3.up * 5, Color.green, 5f);
		Debug.DrawRay(targetPosition - targetForward * 50, Vector3.up * 5, Color.green, 5f);
		Debug.DrawRay(targetPosition, Vector3.up * 5, Color.green, 5f);

		for (int i = 0; i < flightPath.Count - 1; i++)
		{
			Debug.DrawLine(flightPath[i], flightPath[i + 1], Color.red, 5f);
		}

		transform.DOPath(flightPath.ToArray(), 15f, PathType.Linear, PathMode.Full3D);
	}

	public void SetHeadingPosition(Vector3 worldPosition)
	{
		Vector3 direction = (worldPosition - transform.position).normalized;
		SetHeadingDirection(direction);
	}

	public void SetHeadingDirection(Vector3 direction)
	{
		direction.y = 0;
		direction *= 9999f;

		shipCompass.position = transform.position + direction;
		SetWaypoint(shipCompass);
	}

	public void SetWaypoint(Transform newWaypoint)
	{
		waypointTransform = newWaypoint;

		if (waypoint != null)
		{
			Vector3 direction = waypointTransform.position - transform.position;
			direction.y = 0;
			targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		}
	}

	public void SetDirectionRingVisible(bool isVisible)
	{
		directionRingObject.SetActive(isVisible);
	}

	//-----GIZMOS-----

	private void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(transform.position, TargetDirection * 20f);

		}
	}
}
