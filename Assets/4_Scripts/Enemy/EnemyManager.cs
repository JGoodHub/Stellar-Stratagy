using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<CombatShipController> _ships;

    public float followDistance = 300;

    public void Start()
    {
        foreach (CombatShipController ship in _ships)
        {
            ship.alignment = GameManager.Faction.ENEMY;
            //ship.Stats.OnResourceValueChanged += OnShipAttacked;
        }

        TurnController.OnEnemyTurnStarted += PlanEnemyAction;

        TurnController.OnRealtimeStarted += RealtimeStarted;
    }

    private void RealtimeStarted()
    {
        foreach (CombatShipController shipController in _ships)
        {
            shipController.FlightController.FollowFlightPath();
            shipController.WeaponsController.ProcessWeaponActions();
        }
    }

    private void PlanEnemyAction()
    {
        TurnController.Instance.EnemyActionSubmitted();
        return;

        CombatShipController playerShip = PlayerCombatController.Instance.FocusedShip;

        foreach (CombatShipController shipController in _ships)
        {
            List<Vector3> allEndPositions = shipController.FlightController.GetAllEndPositions(shipController.transform.position, shipController.transform.forward);

            Vector3 bestEndPosition = allEndPositions[0];
            float bestError = float.MaxValue;

            foreach (Vector3 endPosition in allEndPositions)
            {
                float error = Mathf.Abs(Vector3.Distance(endPosition, playerShip.transform.position) - followDistance);

                if (bestError < error)
                    continue;

                bestError = error;
                bestEndPosition = endPosition;
            }

            // PathUtils.BezierCurve3 flightPath = shipController.FlightController.GetManualFlightPath(shipController.transform.position, shipController.transform.forward, bestEndPosition);
            // shipController.FlightController.SetFlightPath(flightPath);

            //WeaponConfig weaponConfig = shipController.WeaponsController.Hardpoints[0].WeaponConfig;
            // CombatWeaponsController.WeaponAction weaponAction = new CombatWeaponsController.WeaponAction(weaponConfig.Id, weaponConfig, PlayerCombatController.Instance.PlayerShip, PlayerCombatController.Instance.PlayerShip.transform.position);
            // shipController.WeaponsController.QueueWeaponAction(weaponAction);
        }

        TurnController.Instance.EnemyActionSubmitted();
    }

    private void OnShipAttacked(StatsController sender, StatType resType, float oldValue, float newValue)
    {
        if (resType != StatType.HULL || oldValue < newValue)
            return;

        //sender.OnResourceValueChanged -= OnShipAttacked;

        ShipController ship = sender.Ship;
        //ship.Targeter.SetTarget(PlayerCombatController.PlayerShip, true);
        ship.GetComponent<PatrolController>().enabled = false;
        //ship.FollowFlightController.SetTarget(PlayerManager.PlayerShip);
        //ship.Helm.SetSpeed(4);

        ship.LaserWeaponsController.OnTargetInRange += FireLaserAtTarget;
        ship.LaserWeaponsController.OnLaserReady += FireLaserAtTarget;
    }

    private void FireLaserAtTarget(LaserWeaponsController laserController)
    {
        laserController.FireAtTarget();
    }
}