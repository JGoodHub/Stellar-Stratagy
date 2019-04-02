using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipData : MonoBehaviour {

    //-----VARIABLES-----

	public float maxHullStrength;
	private float currentHullStrength;

	public float maxShieldStrength;
	private float currentShieldStrength;

	public GameObject shieldParent;
	public GameObject shipExplosionPrefab;

	private GameObject uiStatsBarClone;
	private RectTransform uiStatsTransform;

	private bool isFriendly;
	public bool IsFriendly {
		set { isFriendly = value; }
	}

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
	public void Initialise () {
		currentHullStrength = maxHullStrength;
		currentShieldStrength = maxShieldStrength;

        SelfDestruct();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
	public void TakeDamage (float amount) {
		if (currentShieldStrength > 0) {
			currentShieldStrength -= amount;

			if (currentShieldStrength <= 0) {
				shieldParent.SetActive (false);
			}
		} else {			
			currentHullStrength -= amount;

			if (currentHullStrength <= 0) {
				SelfDestruct ();
			}	
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

}
