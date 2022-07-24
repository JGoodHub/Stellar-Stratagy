using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatShipController : Entity
{
	public StatsController Stats => GetComponent<StatsController>();

	public TargetingController Targeter => GetComponent<TargetingController>();
	
	public CombatWeaponsController Weapons => GetComponent<CombatWeaponsController>();
	public CombatFlightController Flight => GetComponent<CombatFlightController>();
	
}
