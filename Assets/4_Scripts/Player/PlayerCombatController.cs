using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using DG.Tweening;
using UnityEngine;

public class PlayerCombatController : SceneSingleton<PlayerCombatController>
{
    [SerializeField] private CombatShipController _playerShip;

    private bool _ourTurn = false;

    public bool OurTurn => _ourTurn;

    public CombatShipController PlayerShip => _playerShip;

    private void Awake()
    {
        TurnController.OnPlayersTurnStarted += UnlockInput;
        TurnController.OnPlayersTurnEnded += LockInput;

        TurnController.OnRealtimeStarted += PlayActions;

        SelectionController.Instance.OnSelectionChanged += OnSelectionChanged;
    }

    private void OnSelectionChanged(object sender, Entity oldEntity, Entity newEntity)
    {
    }

    public void UnlockInput()
    {
        _ourTurn = true;
    }

    public void LockInput()
    {
        _ourTurn = false;
    }

    public void PlayActions()
    {
        _playerShip.FlightController.FollowFlightPath();
        _playerShip.WeaponsController.ProcessWeaponActions();
    }

    // public void SetSelectedShip(CombatShipController shipController)
    // {
    // 	if (shipController == _focusedShip)
    // 		return;
    //
    // 	_focusedShip = shipController;
    // 	OnFocusedShipChanged?.Invoke(shipController);
    // }
}