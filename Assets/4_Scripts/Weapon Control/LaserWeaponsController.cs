using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaserWeaponsController : ShipComponent
{
	public float range;

	public List<LaserStatus> lasers = new List<LaserStatus>();
	public LaserBeam beam;

	public event Action<LaserStatus> OnLaserFired;

	private void Start()
	{
		beam.Initialise(transform);
	}

	private void Update()
	{
		foreach (LaserStatus laser in lasers)
		{
			laser.ReduceCooldown();
		}
	}

	public void FireDirectionalLaser(LaserDirection direction)
	{
		LaserStatus laser = lasers.Find(las => las.direction == direction);

		if (laser == null || laser.readyToFire == false || Targeter.HasTarget == false)
			return;

		//Check the direction of the laser can hit the target
		Vector3 directionVector;
		switch (direction)
		{
			case LaserDirection.FRONT:
				directionVector = transform.forward;
				break;
			case LaserDirection.RIGHT:
				directionVector = transform.right;
				break;
			case LaserDirection.REAR:
				directionVector = -transform.right;
				break;
			case LaserDirection.LEFT:
				directionVector = -transform.forward;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
		}

		if (Vector3.Angle(directionVector, Targeter.target.transform.position - transform.position) > 45f)
			return;

		beam.SetBeamEnd(Targeter.target.transform);
		beam.SetBeamVisible(true);
		beam.HideAfterSeconds(0.67f);

		laser.Fire();

		OnLaserFired?.Invoke(laser);
	}

	//-----GIZMOS-----
	public bool drawGizmos;

	private void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.color = Color.red;
			GizmoExtensions.DrawWireCircle(transform.position, range);
		}
	}
}

[Serializable]
public class LaserStatus
{
	public bool enabled;
	public LaserDirection direction;
	public float firingInterval;
	[HideInInspector] public float firingCooldown;
	public bool readyToFire;

	public event Action<LaserStatus> OnLaserReady;
	public event Action<LaserStatus> OnLaserFired;

	public void ReduceCooldown()
	{
		if (readyToFire)
			return;

		firingCooldown -= Time.deltaTime;

		if (firingCooldown < 0f)
		{
			OnLaserReady?.Invoke(this);
			readyToFire = true;
		}
	}

	public void Fire()
	{
		firingCooldown = firingInterval;
		readyToFire = false;

		OnLaserFired?.Invoke(this);
	}
}

public enum LaserDirection
{
	FRONT,
	RIGHT,
	REAR,
	LEFT,
	UNI
}
