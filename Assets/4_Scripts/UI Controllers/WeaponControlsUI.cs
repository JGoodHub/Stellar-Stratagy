using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControlsUI : SceneSingleton<WeaponControlsUI>
{

	public Image missileClipImage;
	public Sprite[] missileClipSizeSprites;

	private LaserWeaponsController playerLaserController;

	public Button frontLaserBtn;
	public Button rightLaserBtn;
	public Button rearLaserBtn;
	public Button leftLaserBtn;

	public void Start()
	{
		frontLaserBtn.onClick.AddListener(() => FireDirectionalLaser(LaserDirection.FRONT));
		rightLaserBtn.onClick.AddListener(() => FireDirectionalLaser(LaserDirection.RIGHT));
		rearLaserBtn.onClick.AddListener(() => FireDirectionalLaser(LaserDirection.REAR));
		leftLaserBtn.onClick.AddListener(() => FireDirectionalLaser(LaserDirection.LEFT));

		playerLaserController = PlayerManager.Instance.playerShip.LaserWeaponsController;

		foreach (LaserStatus laser in playerLaserController.lasers)
		{
			laser.OnLaserReady += OnLaserReady;
			laser.OnLaserFired += LaserFired;
		}
	}

	public void FireDirectionalLaser(LaserDirection direction)
	{
		PlayerManager.Instance.playerShip.LaserWeaponsController.FireDirectionalLaser(direction);
	}

	public void OnLaserReady(LaserStatus laser)
	{
		GetButtonForDirection(laser.direction).interactable = true;
	}

	public void LaserFired(LaserStatus laser)
	{
		GetButtonForDirection(laser.direction).interactable = false;
	}

	private Button GetButtonForDirection(LaserDirection direction)
	{
		switch (direction)
		{
			case LaserDirection.FRONT:
				return frontLaserBtn;
			case LaserDirection.RIGHT:
				return rightLaserBtn;
			case LaserDirection.REAR:
				return rearLaserBtn;
			case LaserDirection.LEFT:
				return leftLaserBtn;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
		}
	}

}
