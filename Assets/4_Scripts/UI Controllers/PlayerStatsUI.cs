using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsUI : Singleton<PlayerStatsUI>
{

	[Header("Player Stats UI")]
	public Image hullFillImage;
	public TextMeshProUGUI hullFillText;

	[Space]
	public Image shieldFillImage;
	public TextMeshProUGUI shieldFillText;

	private void Start()
	{
		PlayerManager.PlayerShip.Stats.OnResourceValueChanged += OnHullOrShieldChanged;
	}

	private void OnHullOrShieldChanged(StatsController sender, ResourceType resType, float oldValue, float newValue)
	{
		if (resType != ResourceType.HULL && resType != ResourceType.SHIELD)
			return;

		if (resType == ResourceType.HULL)
		{
			hullFillImage.fillAmount = newValue / sender.hullStat.max;
			hullFillText.text = $"{newValue}/{sender.hullStat.max}";
		}
		else
		{
			shieldFillImage.fillAmount = newValue / sender.shieldStat.max;
			shieldFillText.text = $"{newValue}/{sender.shieldStat.max}";
		}
	}

}
