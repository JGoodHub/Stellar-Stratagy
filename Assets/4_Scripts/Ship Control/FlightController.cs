using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : ShipComponent
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
	[SerializeField] private AnimationCurve rotationSpeedZeroOutCurve;

	[SerializeField] private float peakRotationSpeed;
	private float currentRotSpeed;

	private Quaternion targetRotation;

	private Transform waypoint;

	[Header("Fuel Rate")]
	public float maxFuelRate = 1f;

	[Header("Debug")]
	public bool showVisualisations;

	private Transform shipCompass;

	//-----METHODS-----

	private void Awake()
	{
		shipCompass = new GameObject(name + "_[COMPASS]").transform;
	}

	private void Update()
	{
		if (waypoint != null)
		{
			Vector3 direction = waypoint.position - transform.position;
			direction.y = 0;

			targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		}

		//Turning
		if (Quaternion.Angle(transform.rotation, targetRotation) >= 0.01f)
		{
			Quaternion rotation = transform.rotation;
			float degreesToTarget = Quaternion.Angle(rotation, targetRotation);

			float newRotSpeed = Mathf.Clamp(currentRotSpeed + (rotationAcceleration * Time.deltaTime), 0, peakRotationSpeed);
			currentRotSpeed = newRotSpeed * rotationSpeedZeroOutCurve.Evaluate(degreesToTarget);

			rotation = Quaternion.RotateTowards(rotation, targetRotation, currentRotSpeed * Time.deltaTime);
			transform.rotation = rotation;
		}
		else
		{
			transform.rotation = targetRotation;
		}

		if (directionRingObject != null)
			directionRingObject.transform.rotation = Quaternion.Lerp(directionRingObject.transform.rotation, Quaternion.LookRotation(targetRotation * Vector3.forward, Vector3.up), 0.2f);

		//Speed
		if (currentSpeed < targetSpeed)
		{
			currentSpeed += acceleration * Time.deltaTime;
		}
		else if (currentSpeed > targetSpeed)
		{
			currentSpeed -= deceleration * Time.deltaTime;
		}
		else if (targetSpeed == 0 && currentSpeed < 0.01f)
		{
			currentSpeed = 0;
		}

		transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime), Space.Self);

		//Fuel
		float currentFuelRate = maxFuelRate * (currentSpeed / maxSpeed);
		Ship.Stats.ModifyResource(ResourceType.FUEL, -currentFuelRate * Time.deltaTime);
	}

	public void IncreaseSpeed()
	{
		speedSetting++;
		speedSetting = Mathf.Clamp(speedSetting, 0, speedSettingCap);

		UpdateTargetSpeed();
	}

	public void DecreaseSpeed()
	{
		speedSetting--;
		speedSetting = Mathf.Clamp(speedSetting, 0, speedSettingCap);

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

	public void SetSpeedCap(int speedSettingCap)
	{
		this.speedSettingCap = Mathf.Clamp(speedSettingCap, 0, SPEED_DIVISIONS);
		speedSetting = Mathf.Clamp(speedSetting, 0, speedSettingCap);

		UpdateTargetSpeed();
	}

	private void UpdateTargetSpeed()
	{
		targetSpeed = (speedSetting / (float)SPEED_DIVISIONS) * maxSpeed;
	}

	public void SetHeading(Vector3 position)
	{
		Vector3 direction = (position - transform.position).normalized * 99999f;
		direction.y = 0;

		shipCompass.position = transform.position + direction;
		SetWaypoint(shipCompass);
	}

	public void SetWaypoint(Transform newWaypoint)
	{
		waypoint = newWaypoint;
	}

	public void SetDirectionRingVisible(bool isVisible)
	{
		directionRingObject.SetActive(isVisible);
	}

	//-----GIZMOS-----

	private void OnDrawGizmos()
	{
		if (showVisualisations)
		{
			//Player directional gizmos
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(transform.position, transform.forward * 100f);

			//Player position gizmos
			Gizmos.DrawCube(transform.position + (Vector3.up * 10f), Vector3.one * 2f);
		}
	}
}
