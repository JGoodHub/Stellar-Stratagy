using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using DG.Tweening;
using UnityEngine;

public class PlayerCombatController : SceneSingleton<PlayerCombatController>
{

	[SerializeField] private CombatShipController playerShip;

	public CombatShipController PlayerShip => playerShip;

	public bool ourTurn = false;

	public void StartTurn()
	{
		playerShip.Helm._draggingLocked = false;
		ourTurn = true;
	}

	public void EndTurn()
	{
		playerShip.Helm._draggingLocked = true;
		ourTurn = false;

		playerShip.Helm.FollowFlightPath();

		FindObjectOfType<LaserWeaponItem>().FireLasersAtTarget();

		DOVirtual.DelayedCall(6f, StartTurn, false);
		
		
	}

}
