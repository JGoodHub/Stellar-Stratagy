using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{

	private HashSet<ShipController> enemyShips;

	[Header("Debug Variables")]
	public GameObject[] preExistingShips;

	public void Start()
	{
		enemyShips = new HashSet<ShipController>();

		foreach (GameObject ship in preExistingShips)
		{
			ShipController shipControl = ship.GetComponent<ShipController>();
			if (shipControl == null)
			{
				Debug.LogError("Non ship object placed in pre existing ships array");
			}
			else
			{
				enemyShips.Add(shipControl);
			}
		}

		foreach (ShipController ship in enemyShips)
		{
			ship.alignment = GameManager.Faction.ENEMY;
			ship.Stats.OnResourceValueChanged += OnShipAttacked;
		}
	}

	private void OnShipAttacked(StatsController sender, ResourceType resType, float oldValue, float newValue)
	{
		if (resType != ResourceType.HULL || oldValue < newValue)
			return;

		sender.OnResourceValueChanged -= OnShipAttacked;

		ShipController ship = sender.Ship;
		ship.Targeter.SetTarget(PlayerManager.PlayerShip, true);
		ship.GetComponent<PatrolController>().enabled = false;
		ship.FollowFlightController.SetTarget(PlayerManager.PlayerShip);
		ship.Helm.SetSpeed(4);

		ship.LaserWeaponsController.OnTargetInRange += FireLaserAtTarget;
		ship.LaserWeaponsController.OnLaserReady += FireLaserAtTarget;
	}

	private void FireLaserAtTarget(LaserWeaponsController laserController)
	{
		laserController.FireAtTarget();
	}


}
