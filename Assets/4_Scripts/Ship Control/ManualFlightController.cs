using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ManualFlightController : ShipComponent
{
	//-----VARIABLES-----

	[Header("Thrust")]
	public const int SPEED_DIVISIONS = 10;

	public int speedSetting = 0;
	private int speedSettingCap = SPEED_DIVISIONS;

	private float currentSpeed;
	private float targetSpeed;

	[SerializeField] private float maxSpeed;
	[SerializeField] private float acceleration;
	[SerializeField] private float deceleration;

	[Header("Rotation")]
	[SerializeField] private GameObject directionRingObject;

	[SerializeField] private float rotationAcceleration;
	[FormerlySerializedAs("rotationSpeedZeroOutCurve"),SerializeField] private AnimationCurve rotationDecelerationCurve;

	[SerializeField] private float peakRotationSpeed;
	private float currentRotSpeed;

	private Quaternion targetRotation;
	private Transform waypoint;


	[Header("Fuel Rate")]
	public float maxFuelRate = 1f;

	[Header("Debug")]
	public bool drawGizmos;

	private Transform shipCompass;
	
	// PROPERTIES
	
	public Vector3 TargetDirection => targetRotation * Vector3.forward;

	// METHODS

	private void Awake()
	{
		shipCompass = new GameObject(name + "_[COMPASS]").transform;
	}

	private void Update()
	{
		//Update our target direction to point to the waypoint
		if (waypoint != null)
		{
			Vector3 direction = waypoint.position - transform.position;
			direction.y = 0;

			targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		}

		//Turning
		if (Quaternion.Angle(transform.rotation, targetRotation) >= 0.025f)
		{
			Quaternion rotation = transform.rotation;
			float degreesToTarget = Quaternion.Angle(rotation, targetRotation);

			currentRotSpeed = Mathf.Clamp(currentRotSpeed + (rotationAcceleration * Time.deltaTime), 0, peakRotationSpeed);

			rotation = Quaternion.RotateTowards(rotation, targetRotation, currentRotSpeed * rotationDecelerationCurve.Evaluate(degreesToTarget) * Time.deltaTime);
			transform.rotation = rotation;
		}
		else
		{
			transform.rotation = targetRotation;
			currentRotSpeed = 0;
		}

		if (directionRingObject != null)
			directionRingObject.transform.rotation = Quaternion.Lerp(directionRingObject.transform.rotation, Quaternion.LookRotation(targetRotation * Vector3.forward, Vector3.up), 0.34f);

		//Speed
		if (currentSpeed < targetSpeed)
		{
			currentSpeed += acceleration * Time.deltaTime;
		}
		else if (currentSpeed > targetSpeed)
		{
			currentSpeed -= deceleration * Time.deltaTime;
		}
		else if (targetSpeed == 0 && currentSpeed < 0.02f)
		{
			currentSpeed = 0;
		}

		transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime), Space.Self);

		//Fuel
		float currentFuelRate = maxFuelRate * (currentSpeed / maxSpeed);
		Ship.Stats.ModifyResource(ResourceType.FUEL, -currentFuelRate * Time.deltaTime);
	}

	public void ModifySpeed(int modifier)
	{
		speedSetting = Mathf.Clamp(speedSetting + modifier, 0, speedSettingCap);
		UpdateTargetSpeed();
	}

	public void SetSpeed(int speed)
	{
		speedSetting = Mathf.Clamp(speed, 0, speedSettingCap);

		UpdateTargetSpeed();
	}

	public void SetSpeedAsPercentage(float percentage)
	{
		speedSetting = Mathf.RoundToInt(Mathf.Clamp(percentage * SPEED_DIVISIONS, 0, speedSettingCap));
		UpdateTargetSpeed();
	}

	public void SetSpeedCap(int tempSpeedCap)
	{
		speedSettingCap = Mathf.Clamp(tempSpeedCap, 0, SPEED_DIVISIONS);
		speedSetting = Mathf.Clamp(speedSetting, 0, tempSpeedCap);

		UpdateTargetSpeed();
	}

	private void UpdateTargetSpeed()
	{
		targetSpeed = speedSetting / (float)SPEED_DIVISIONS * maxSpeed;
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
		waypoint = newWaypoint;

		if (waypoint != null)
		{
			Vector3 direction = waypoint.position - transform.position;
			direction.y = 0;
			targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		}
	}

	public void SetDirectionRingVisible(bool isVisible)
	{
		directionRingObject.SetActive(isVisible);
	}

	public enum FlightMode
	{
		MANUAL,
		ORBITAL,
		COMBAT,
		FOLLOW,
		PATROL
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
