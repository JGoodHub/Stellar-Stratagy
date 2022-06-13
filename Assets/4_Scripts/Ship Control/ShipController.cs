using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : Entity
{

	//Basic components
	public StatsController Stats => GetComponent<StatsController>();

	//Weapon controls
	public TargetingController Targeter => GetComponent<TargetingController>();
	public LaserWeaponsController LaserWeaponsController => GetComponent<LaserWeaponsController>();
	public MissileWeaponsController MissileWeaponsController => GetComponent<MissileWeaponsController>();

	//Flight controls
	public FlightController Helm => GetComponent<FlightController>();
	public OrbitalController OrbitalController => GetComponent<OrbitalController>();
	public FollowFlightController FollowFlightController => GetComponent<FollowFlightController>();


	[Header("Ship Controller")]
	public GameObject explosionPrefab;

	protected override void Start()
	{
		base.Start();

		Stats.OnResourceMinimumReached += OnHullDestroyed;
	}
	private void OnHullDestroyed(StatsController sender, ResourceType resType)
	{
		if (resType != ResourceType.HULL)
			return;

		GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Destroy(explosion, 5f);

		Destroy(gameObject);
	}

}
