using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Faction = GameManager.Faction;

public class ShipFactory : MonoBehaviour
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

    public Faction owner;
    public Ship[] ships;

    private Transform shipsParent;

    #region Inherited Method

    private void Start()
    {
        GameObject shipsParentObject = GameObject.Find("[SHIPS]");
        if (shipsParentObject == null)
        {
            Debug.LogError("No [SHIPS] game object found");
        }
        else
        {
            shipsParent = shipsParentObject.transform;
        }
    }

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
                return shipInstance.GetComponent<ShipController>();
            }
        }

        return null;

    }

    public static ShipFactory GetFactionFactory(Faction faction)
    {
        ShipFactory[] allFactories = FindObjectsOfType<ShipFactory>();

        foreach (ShipFactory factory in allFactories)
        {
            if (factory.owner == faction)
            {
                return factory;
            }
        }

        return null;
    }

    #endregion

}
