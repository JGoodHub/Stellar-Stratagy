using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatShipController : Entity
{
	public StatsController StatsController => GetComponent<StatsController>();

	public TargetingController Targeter => GetComponent<TargetingController>();
	
	public CombatWeaponsController WeaponsController => GetComponent<CombatWeaponsController>();
	
	public CombatFlightController FlightController => GetComponent<CombatFlightController>();
	
}
