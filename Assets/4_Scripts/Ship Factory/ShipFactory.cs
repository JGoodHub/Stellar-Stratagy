using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Faction = GameManager.Faction;

public class ShipFactory : Entity
{

    public enum Size
    {
        SHUTTLE,
        FIGHTER,
        CORVETTE,
        FRIGATE,
        CRUISER,
        DESTROYER,
        CARRIER
    }

    [System.Serializable]
    public struct Ship
    {
        public GameObject prefab;
        public Size size;
        public Faction sourceRestriction;
    }

    [Header("Ship Factory")]

    [SerializeField] private GameObject shipsParentObject;
    public Ship[] ships;

    #region Inherited Method

    #endregion

    #region Public Methods

    public ShipController RequestShip(Faction sourceFaction, Size requestedClass)
    {
        foreach (Ship ship in ships)
        {
            if (ship.size == requestedClass && (ship.sourceRestriction == Faction.NONE || ship.sourceRestriction == sourceFaction))
            {
                //Animate the ship flying into the battlefield
                GameObject shipInstance = Instantiate(ship.prefab, transform.position, transform.rotation);
                shipInstance.transform.parent = shipsParentObject.transform;
                return shipInstance.GetComponent<ShipController>();
            }
        }

        return null;

    }

    public static ShipFactory GetNearestFactionFactory(Faction faction)
    {
        ShipFactory[] allFactories = FindObjectsOfType<ShipFactory>();

        foreach (ShipFactory factory in allFactories)
        {
            if (factory.alignment == faction)
            {
                return factory;
            }
        }

        return null;
    }

    #endregion

}
