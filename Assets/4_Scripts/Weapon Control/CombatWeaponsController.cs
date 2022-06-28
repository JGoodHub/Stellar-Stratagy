using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatWeaponsController : MonoBehaviour
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
		public WeaponConfig WeaponConfig;
		public Entity TargetEntity;
		public Vector3 TargetPosition;
	}

	public List<Hardpoint> Hardpoints;

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
		}
	}

	public void ProcessWeaponActions()
	{
		
		
		
		
	}
}


