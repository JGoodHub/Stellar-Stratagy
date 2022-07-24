using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserWeaponItem : WeaponCardItem
{
	private bool _validTarget;

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.OurTurn == false)
			return;
		
		PlayerCombatController.Instance.PlayerShip.Weapons.RemoveWeaponActionUsingID(_id);

		CameraDragController.Instance.DragEnabled = false;
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.OurTurn == false)
			return;

		if (CameraHelper.IsMouseOverUI())
		{
			_pointReticleObject.transform.position = Vector3.up * 1000;
		}
		else
		{
			Vector3 navPlanePoint = NavigationPlane.RaycastNavPlane();

			_pointReticleObject.transform.position = _targetPosition = navPlanePoint;

			List<Entity> nearbyEntities = SelectionController.GetNearbyEntities(navPlanePoint, 35f);

			if (nearbyEntities.Count > 0)
			{
				_targetEntity = nearbyEntities[0];
				_pointReticleObject.transform.position = _targetEntity.transform.position;
			}
			else
			{
				_targetEntity = null;
			}
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.OurTurn == false)
			return;
		
		_validTarget = CameraHelper.IsMouseOverUI() == false;

		if (_validTarget)
		{
			CombatWeaponsController.WeaponAction weaponAction = new CombatWeaponsController.WeaponAction(_id, _weaponConfig, _targetEntity, _targetPosition);
			PlayerCombatController.Instance.PlayerShip.Weapons.QueueWeaponAction(weaponAction);
		}
		else
		{
			_pointReticleObject.transform.position = Vector3.up * 1000;
			_targetEntity = null;
		}
		
		CameraDragController.Instance.DragEnabled = true;
	}

	
}
