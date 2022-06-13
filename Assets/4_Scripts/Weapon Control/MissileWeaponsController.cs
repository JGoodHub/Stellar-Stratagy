using System;
using UnityEngine;

public class MissileWeaponsController : ShipComponent
{
	public float range;

	public float firingInterval;
	[HideInInspector] public float firingCooldown;
	public bool readyToFire;

	public GameObject missilePrefab;

	private ResourceStat missileStat;
	
	public event Action<MissileWeaponsController> OnMissileReady;
	public event Action<MissileWeaponsController> OnMissileFired;

	private void Start()
	{
		missileStat = Ship.Stats.GetResource(ResourceType.MISSILE);
	}

	private void Update()
	{
		firingCooldown -= Time.deltaTime;

		if (readyToFire == false && firingCooldown <= 0f && missileStat.current > 0)
		{
			OnMissileReady?.Invoke(this);
			readyToFire = true;
		}
	}

	public void FireAtTarget()
	{
		if (Ship.Stats.GetResource(ResourceType.MISSILE).current <= 0 ||
			Ship.Targeter.target == null ||
			Vector3.Distance(Ship.Targeter.target.transform.position, transform.position) > range)
			return;

		GameObject missileObject = Instantiate(missilePrefab, transform.position, Quaternion.identity);
		missileObject.GetComponent<MissileProjectile>().SetTarget(Ship.Targeter.target);

		Ship.Stats.ModifyResource(ResourceType.MISSILE, -1);

		firingCooldown = firingInterval;
		readyToFire = false;

		OnMissileFired?.Invoke(this);
	}


}
