using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatShipController : Entity
{
	public StatsController Stats => GetComponent<StatsController>();

	public TargetingController Targeter => GetComponent<TargetingController>();
	public LaserWeaponsController LaserWeaponsController => GetComponent<LaserWeaponsController>();
	public MissileWeaponsController MissileWeaponsController => GetComponent<MissileWeaponsController>();

	public CombatFlightController Helm => GetComponent<CombatFlightController>();

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
