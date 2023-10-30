using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatShipController : SelectableEntity
{
	public StatsController StatsController => GetComponent<StatsController>();

	public TargetingController Targeter => GetComponent<TargetingController>();
	
	public CombatWeaponsController WeaponsController => GetComponent<CombatWeaponsController>();
	
	public CombatFlightController FlightController => GetComponent<CombatFlightController>();
	
}
