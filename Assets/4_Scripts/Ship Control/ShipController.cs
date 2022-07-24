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
	public AutoFlightController Helm => GetComponent<AutoFlightController>();
	public OrbitalController OrbitalController => GetComponent<OrbitalController>();

	[Header("Ship Controller")]
	public GameObject explosionPrefab;

	protected override void Start()
	{
		base.Start();

		//Stats.OnResourceMinimumReached += OnHullDestroyed;
	}
	
	private void OnHullDestroyed(StatsController sender, StatType resType)
	{
		if (resType != StatType.HULL)
			return;

		GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Destroy(explosion, 5f);

		Destroy(gameObject);
	}

}
