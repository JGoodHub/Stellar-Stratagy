using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatWeaponsController : ShipComponent
{
    [Serializable]
    public class Hardpoint
    {
        public string ID;
        public Transform Transform;
        public WeaponConfig WeaponConfig;

        [HideInInspector] public Turret Turret;
    }

    [Serializable]
    public class WeaponAction
    {
        public int ID;
        public WeaponConfig WeaponConfig;
        public Entity TargetEntity;
        public Vector3 TargetPosition;

        public WeaponAction(int id, WeaponConfig weaponConfig, Entity targetEntity, Vector3 targetPosition)
        {
            ID = id;
            WeaponConfig = weaponConfig;
            TargetEntity = targetEntity;
            TargetPosition = targetPosition;
        }
    }

    [NonReorderable, SerializeField] private List<Hardpoint> _hardpoints;

    private List<WeaponAction> _weaponActionsQueue = new List<WeaponAction>();

    public List<Hardpoint> Hardpoints => _hardpoints;

    private void Start()
    {
        SetupWeapons();
    }

    private void SetupWeapons()
    {
        // if (ShipController.alignment != GameManager.Faction.FRIENDLY)
        //     return;
        //
        // foreach (Hardpoint hardpoint in _hardpoints)
        // {
        //     GameObject turretObject = Instantiate(hardpoint.WeaponConfig.TurretPrefab, hardpoint.Transform);
        //     hardpoint.Turret = turretObject.GetComponent<Turret>();
        //
        //     WeaponCardHolder.Instance.CreateWeaponCard(ShipController, hardpoint.WeaponConfig);
        // }

        //WeaponCardHolder.Instance.HideAllCards();
    }

    public void ProcessWeaponActions()
    {
        foreach (WeaponAction action in _weaponActionsQueue)
        {
            Hardpoint matchingHardpoint = _hardpoints.Find(hardpoint => hardpoint.WeaponConfig.Id == action.WeaponConfig.Id);

            action.WeaponConfig.Fire(ShipController, action.TargetEntity, matchingHardpoint.Turret.transform, action.TargetPosition);
        }
    }

    public void QueueWeaponAction(WeaponAction weaponAction)
    {
        _weaponActionsQueue.Add(weaponAction);
    }

    public void RemoveWeaponAction(WeaponAction weaponAction)
    {
        _weaponActionsQueue.Remove(weaponAction);
    }

    public void RemoveWeaponActionUsingID(int id)
    {
        _weaponActionsQueue.RemoveAll(action => action.ID == id);
    }
}