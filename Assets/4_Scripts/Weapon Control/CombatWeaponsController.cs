using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatWeaponsController : ShipComponent
{
	[Serializable]
	public class Hardpoint
	{
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

	[NonReorderable] public List<Hardpoint> Hardpoints;

	private List<WeaponAction> WeaponActions = new List<WeaponAction>();

	private void Start()
	{
		SetupWeapons();
	}

	private void SetupWeapons()
	{
		foreach (Hardpoint hardpoint in Hardpoints)
		{
			GameObject turretObject = Instantiate(hardpoint.WeaponConfig.TurretPrefab, hardpoint.Transform);
			hardpoint.Turret = turretObject.GetComponent<Turret>();

			WeaponCardHolder.Instance.CreateWeaponCard(ShipController, hardpoint.WeaponConfig);
		}
		
		WeaponCardHolder.Instance.HideAllCards();
	}

	public void ProcessWeaponActions()
	{
		foreach (WeaponAction action in WeaponActions)
		{
			action.WeaponConfig.Fire(ShipController, action.TargetEntity, action.TargetPosition);
		}
	}

	public void QueueWeaponAction(WeaponAction weaponAction)
	{
		WeaponActions.Add(weaponAction);
	}

	public void RemoveWeaponAction(WeaponAction weaponAction)
	{
		WeaponActions.Remove(weaponAction);
	}

	public void RemoveWeaponActionUsingID(int id)
	{
		WeaponActions.RemoveAll(action => action.ID == id);
	}
}
