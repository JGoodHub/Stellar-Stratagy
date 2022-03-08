using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipStats : MonoBehaviour
{
    public delegate void ResourceChanged(ResourceType resource, float amount, float newValue);

    public event ResourceChanged OnHullValueChanged;
    public event ResourceChanged OnShieldValueChanged;
    public event ResourceChanged OnFuelValueChanged;
    
    //-----VARIABLES-----

    public ShipController ShipController => GetComponent<ShipController>();

    public ShipTemplate template;
    
    
    
    private float currentHull;
    private float currentShield;
    private float currentFuel;

    public GameObject shieldParent;

    private bool isFriendly;
    public bool IsFriendly
    {
        set => isFriendly = value;
    }

    //-----METHODS-----

    private void Start()
    {
        currentHull =  hullStrength;
        currentShield = shieldStrength;
        currentFuel = fuelCapacity;
    }

    public float GetResourceOfType(ResourceType type, bool percentage)
    {
        switch (type)
        {
            case ResourceType.HULL:
                return percentage ? Mathf.Clamp01(currentHull / hullStrength) : currentHull;
            case ResourceType.SHIELD:
                return percentage ? Mathf.Clamp01(currentShield / shieldStrength) : currentShield;
            case ResourceType.FUEL:
                return percentage ? Mathf.Clamp01(currentFuel / fuelCapacity) : currentFuel;
            default:
                Debug.LogError("Attempting to get a resource that doesn't exist");
                return 0;
        }
    }

    public void ApplyChangeToResources(ResourceType resource, float amount)
    {
        switch (resource)
        {
            case ResourceType.HULL:
                currentHull += amount;

                OnHullValueChanged?.Invoke(ResourceType.HULL, amount, currentHull);

                if (currentHull <= 0)
                    SelfDestruct();

                break;
            case ResourceType.SHIELD:
                currentShield += amount;

                OnShieldValueChanged?.Invoke(ResourceType.SHIELD, amount, currentShield);

                if (currentShield <= 0)
                    DropShield();

                break;
            case ResourceType.FUEL:
                currentFuel += amount;

                OnFuelValueChanged?.Invoke(ResourceType.FUEL, amount, currentFuel);

                if (currentFuel <= 0)
                    ShipController.FlightController.SetSpeedCap(2);
                else
                    ShipController.FlightController.SetSpeedCap(ManualFlightController.SPEED_DIVISIONS);

                break;
            default:
                Debug.LogError("Attempting to change a resource that doesn't exist");
                break;
        }
    }

    private void DropShield()
    {
        shieldParent.SetActive(false);
    }

    private void SelfDestruct()
    {
        GameObject shipExplosionObject = Instantiate(shipExplosionPrefab, transform.position, Quaternion.identity);
        RuntimeObjectsManager.instance.AddToCollection(shipExplosionObject, "Explosions");

        Destroy(shipExplosionObject, 5f);

        if (isFriendly)
        {
            FogOfWarManager.instance.RemoveShipMask(gameObject.transform);
        }

        Destroy(gameObject);
    }
}