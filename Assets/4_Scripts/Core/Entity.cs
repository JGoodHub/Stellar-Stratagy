using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]

    [Header("Health Stats")]

    [SerializeField] private float hullStrength;
    [SerializeField] private float hullRegenerationRate;
    private float currentHull;

    [Space]

    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float shieldStrength;
    [SerializeField] private float shieldRegenerationRate;
    private float currentShield;

    [Space]

    [SerializeField] private GameObject destructionPrefab;

    [Header("Selection Variables")]

    [SerializeField] private GameObject selectionRing;

    [Header("Factions")]

    public GameManager.Faction owner;

    protected virtual void Start()
    {
        currentHull = hullStrength;
        currentShield = shieldStrength;

        SetRingActive(false);
    }

    public void SetRingActive(bool state)
    {
        selectionRing.SetActive(state);
    }

    public float GetResourceOfType(StatType type)
    {
        switch (type)
        {
            case StatType.HULL:
                return currentHull;
            case StatType.SHIELD:
                return currentShield;
            default:
                Debug.LogError("Attempting to get a resource that doesn't exist");
                return 0;
        }
    }


    public void ApplyChangeToData(StatChange change)
    {
        switch (change.Resource)
        {
            case StatType.HULL:
                currentHull += change.Amount;
                if (currentHull <= 0)
                {
                    SelfDestruct();
                }
                break;
            case StatType.SHIELD:
                currentShield += change.Amount;

                if (currentShield <= 0)
                {
                    shieldObject.SetActive(false);
                }
                break;
            default:
                Debug.LogError("Attempting to change a resource that doesn't exist");
                break;
        }
    }


    private void SelfDestruct()
    {
        GameObject shipExplosionObject = Instantiate(destructionPrefab, transform.position, Quaternion.identity);
        RuntimeObjectsManager.instance.AddToCollection(shipExplosionObject, "Explosions");

        Destroy(shipExplosionObject, 5f);
        Destroy(gameObject);
    }



    public enum StatType { HULL, SHIELD };

    public struct StatChange
    {
        public StatType Resource;
        public float Amount;

        public StatChange(StatType _resource, int _amount)
        {
            Resource = _resource;
            Amount = _amount;
        }
    }
}



