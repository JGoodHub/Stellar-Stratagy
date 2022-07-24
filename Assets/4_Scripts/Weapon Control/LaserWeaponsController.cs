using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaserWeaponsController : ShipComponent
{
	public float range = 150;
	public float damage = 1;
	public float firingInterval = 1.5f;
	[HideInInspector] public float firingCooldown;
	public bool readyToFire = false;

	public GameObject beamPrefab;
	public CombatProjectileLaserBolt _bolt;

	public event Action<LaserWeaponsController> OnLaserFired;
	public event Action<LaserWeaponsController> OnLaserReady;

	public event Action<LaserWeaponsController> OnTargetInRange;
	public event Action<LaserWeaponsController> OnTargetOutOfRange;
	private bool targetInRange = false;

	private void Start()
	{
		GameObject beamObject = Instantiate(beamPrefab, Vector3.zero, Quaternion.identity);
		_bolt = beamObject.GetComponent<CombatProjectileLaserBolt>();
		//beam.Initialise(transform);

		readyToFire = false;
	}

	private void Update()
	{
		ReduceCooldown();

		CheckForTargetInRange();
	}

	public void FireAtTarget()
	{
		ShipController target = Ship.Targeter.target;

		if (readyToFire == false || target == null || targetInRange == false)
			return;

		// beam.SetBeamEnd(target.transform);
		// beam.SetBeamVisible(true);
		// beam.HideAfterSeconds(0.67f);

		firingCooldown = firingInterval;
		readyToFire = false;

		//target.Stats.Modify(StatType.HULL, -damage);

		OnLaserFired?.Invoke(this);
	}

	public void ReduceCooldown()
	{
		if (readyToFire)
			return;

		firingCooldown -= Time.deltaTime;

		if (firingCooldown <= 0f)
		{
			readyToFire = true;
			OnLaserReady?.Invoke(this);
		}
	}

	private void CheckForTargetInRange()
	{
		if (Ship.Targeter.TargetDistance <= range)
		{
			if (targetInRange == false)
			{
				targetInRange = true;
				OnTargetInRange?.Invoke(this);
			}
		}
		else
		{
			if (targetInRange == true)
			{
				targetInRange = false;
				OnTargetOutOfRange?.Invoke(this);
			}
		}
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
