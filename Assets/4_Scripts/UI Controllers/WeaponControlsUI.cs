using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControlsUI : SceneSingleton<WeaponControlsUI>
{

	private LaserWeaponsController laserController;
	public Button laserFireBtn;
	public Image laserCooldownFill;

	private MissileWeaponsController missileController;
	public Button missileFireBtn;
	public Image missileClipFill;

	public void Start()
	{
		laserController = PlayerManager.PlayerShip.LaserWeaponsController;
		laserFireBtn.onClick.AddListener(() => laserController.FireAtTarget());

		laserController.OnLaserReady += LaserReady;
		laserController.OnLaserFired += LaserFired;

		missileController = PlayerManager.PlayerShip.MissileWeaponsController;
		missileFireBtn.onClick.AddListener(() => missileController.FireAtTarget());

		missileController.OnMissileFired += MissileFired;
		missileController.OnMissileReady += MissileReady;

		PlayerManager.PlayerShip.Stats.OnResourceValueChanged += MissileResourceChanged;
	}

	public void LaserReady(LaserWeaponsController laser)
	{
		laserFireBtn.interactable = true;
		laserCooldownFill.fillAmount = 1f;
	}

	public void LaserFired(LaserWeaponsController laser)
	{
		laserFireBtn.interactable = false;

		DOVirtual.Float(0f, 1f, laser.firingInterval, percent =>
		{
			laserCooldownFill.fillAmount = percent;
		});
	}

	private void MissileReady(MissileWeaponsController obj)
	{
		missileFireBtn.interactable = true;
	}

	private void MissileFired(MissileWeaponsController obj)
	{
		missileFireBtn.interactable = false;
	}

	private void MissileResourceChanged(StatsController stats, ResourceType type, float oldValue, float newValue)
	{
		if (type != ResourceType.MISSILE)
			return;

		missileClipFill.fillAmount = newValue / stats.GetResource(ResourceType.MISSILE).max;
	}

}
