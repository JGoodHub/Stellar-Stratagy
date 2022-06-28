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

	public void AddCardItemToContainer(WeaponCardItem cardItemPrefab)
	{
		GameObject cardObject = Instantiate(cardItemPrefab.gameObject, _cardsParent);
		WeaponCardItem weaponCardItem = cardObject.GetComponent<WeaponCardItem>();
		
		_weaponCards.Add(weaponCardItem);
	}

}
