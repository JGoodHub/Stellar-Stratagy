using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipData : MonoBehaviour {

    //-----VARIABLES-----

	public float hullStrength;
	private float currentHull;

	public float shieldStrength;
	private float currentShield;

	public GameObject shieldParent;
	public GameObject shipExplosionPrefab;

	private bool isFriendly;
	public bool IsFriendly { set => isFriendly = value; }

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
	public void Initialise () {
		currentHull = hullStrength;
		currentShield = shieldStrength;

        SelfDestruct();
	}

    /// <summary>
    /// Return the current amount of the specified resource
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetResourceOfType(StatType type) {
        switch (type) {
            case StatType.HULL:
                return currentHull;
            case StatType.SHIELD:
                return currentShield;
            default:
                Debug.LogError("Attempting to get a resource that doesn't exist");
                return 0;
        }
    }

    //Reduce the specified resource by the specified amount in the cost
    /// <summary>
    /// 
    /// </summary>
    /// <param name="change"></param>
    public void ApplyChangeToData(StatChange change) {
        switch (change.Resource) {
            case StatType.HULL:
                currentHull += change.Amount;
                if (currentHull <= 0) {
                    SelfDestruct();
                }
                break;
            case StatType.SHIELD:
                currentShield += change.Amount;

                if (currentShield <= 0) {
                    shieldParent.SetActive(false);
                }
                break;           
            default:
                Debug.LogError("Attempting to change a resource that doesn't exist");
                break;
        }
    }


    /// <summary>
    /// 
    /// </summary>
	private void SelfDestruct () {
		GameObject shipExplosionObject = Instantiate(shipExplosionPrefab, transform.position, Quaternion.identity);
        RuntimeObjectsManager.instance.AddToCollection(shipExplosionObject, "Explosions");

		Destroy (shipExplosionObject, 5f);

		if (isFriendly) {
            FogOfWarManager.instance.RemoveShipMask(gameObject.transform);
		}

		Destroy (gameObject);
	}


    //-----RELATED DATA STRUCTURES-----

    public enum StatType { HULL, SHIELD };

    public struct StatChange {
        public StatType Resource;
        public float Amount;

        public StatChange(StatType _resource, int _amount) {
            Resource = _resource;
            Amount = _amount;
        }
    }

}
