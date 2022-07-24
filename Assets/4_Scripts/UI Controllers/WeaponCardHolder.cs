using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCardHolder : SceneSingleton<WeaponCardHolder>
{

	private List<WeaponCardItem> _weaponCards = new List<WeaponCardItem>();

	[SerializeField] private Transform _cardsParent;

	public void CreateAndAddWeaponCard(WeaponConfig weaponConfig)
	{
		GameObject weaponCardObject = Instantiate(weaponConfig.WeaponCardPrefab.gameObject, _cardsParent);
		WeaponCardItem weaponCardItem = weaponCardObject.GetComponent<WeaponCardItem>();
		weaponCardItem.Initialise(weaponConfig);

		_weaponCards.Add(weaponCardItem);
	}

}
