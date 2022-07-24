using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using DG.Tweening;
using UnityEngine;

public class PlayerCombatController : SceneSingleton<PlayerCombatController>
{

	[SerializeField] private CombatShipController playerShip;

	private bool _ourTurn = false;

	public bool OurTurn => _ourTurn;
	public CombatShipController PlayerShip => playerShip;

	private void Awake()
	{
		TurnController.OnPlayersTurnStarted += UnlockInput;
		TurnController.OnPlayersTurnEnded += LockInput;

		TurnController.OnRealtimeStarted += PlayActions;
	}

	public void UnlockInput()
	{
		playerShip.Flight.DraggingLocked = false;
		_ourTurn = true;
	}

	public void LockInput()
	{
		playerShip.Flight.DraggingLocked = true;
		_ourTurn = false;
	}

	public void PlayActions()
	{
		playerShip.Flight.FollowFlightPath();
		playerShip.Weapons.ProcessWeaponActions();
	}

}
