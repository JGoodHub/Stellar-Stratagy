using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using DG.Tweening;
using UnityEngine;

public class PlayerCombatController : SceneSingleton<PlayerCombatController>
{
    [SerializeField] private List<CombatShipController> _playerShips;

    private CombatShipController _focusedShip;

    private bool _ourTurn = false;

    public bool OurTurn => _ourTurn;

    public CombatShipController FocusedShip => _focusedShip;

    public event Action<CombatShipController> OnFocusedShipChanged;

    private void Awake()
    {
        TurnController.OnPlayersTurnStarted += UnlockInput;
        TurnController.OnPlayersTurnEnded += LockInput;

        TurnController.OnRealtimeStarted += PlayActions;

        SelectionController.Instance.OnSelectionChanged += OnSelectionChanged;
    }

    private void OnSelectionChanged(object sender, Entity oldEntity, Entity newEntity)
    {
        if (_focusedShip != null && newEntity == null)
        {
            _focusedShip = null;
            OnFocusedShipChanged?.Invoke(null);
        }

        if (_playerShips.Contains(newEntity as CombatShipController))
        {
            _focusedShip = (CombatShipController) newEntity;
            OnFocusedShipChanged?.Invoke(_focusedShip);
        }
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
        foreach (CombatShipController playerShip in _playerShips)
        {
            playerShip.FlightController.FollowFlightPath();
            playerShip.WeaponsController.ProcessWeaponActions();
        }
    }

    public void SetSelectedShip(CombatShipController shipController)
    {
        if (shipController == _focusedShip)
            return;

        _focusedShip = shipController;
        OnFocusedShipChanged?.Invoke(shipController);
    }
}