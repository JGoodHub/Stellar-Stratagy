using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponCardHolder : SceneSingleton<WeaponCardHolder>
{
    private Dictionary<CombatShipController, List<WeaponCardItem>> _weaponCards = new Dictionary<CombatShipController, List<WeaponCardItem>>();

    [SerializeField] private Transform _cardsParent;

    private void Start()
    {
        foreach (CombatShipController playerShip in PlayerCombatController.Instance.PlayerShips)
        {
            foreach (CombatWeaponsController.Hardpoint hardpoint in playerShip.WeaponsController.Hardpoints)
            {
                GameObject turretObject = Instantiate(hardpoint.WeaponConfig.TurretPrefab, hardpoint.Transform);
                hardpoint.Turret = turretObject.GetComponent<Turret>();

                CreateWeaponCard(playerShip, hardpoint.WeaponConfig);
            }
        }

        HideAllCards();

        PlayerCombatController.Instance.OnFocusedShipChanged += ActivateCardsForFocusedShip;
    }

    public void CreateWeaponCard(CombatShipController shipController, WeaponConfig weaponConfig)
    {
        GameObject weaponCardObject = Instantiate(weaponConfig.WeaponCardPrefab.gameObject, _cardsParent);
        WeaponCardItem weaponCardItem = weaponCardObject.GetComponent<WeaponCardItem>();
        weaponCardItem.Initialise(weaponConfig);

        if (_weaponCards.ContainsKey(shipController) == false)
            _weaponCards.Add(shipController, new List<WeaponCardItem>());

        _weaponCards[shipController].Add(weaponCardItem);
    }

    private void ActivateCardsForFocusedShip(CombatShipController shipController)
    {
        // Deactivate all cards
        HideAllCards();

        if (shipController == null || _weaponCards.ContainsKey(shipController) == false)
            return;

        // Activate juts the selected ship cards
        foreach (WeaponCardItem cardItem in _weaponCards[shipController])
            cardItem.gameObject.SetActive(true);
    }

    public void HideAllCards()
    {
        foreach (List<WeaponCardItem> cardItems in _weaponCards.Values)
        foreach (WeaponCardItem cardItem in cardItems)
            cardItem.gameObject.SetActive(false);
    }
}