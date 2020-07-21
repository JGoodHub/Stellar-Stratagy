using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFactory : MonoBehaviour {

    //-----SINGLETON SETUP-----

	public static ShipFactory instance = null;
	
	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

    //-----ENUM-----

    public enum SpawnRestriction { PLAYER, ENEMY, EITHER };

    //-----STRUCTS-----

    [System.Serializable]
    public struct Ship {
        public string shipClass;
        public GameObject shipPrefab;
        public SpawnRestriction shipSpawnRestriction;
    }

    //-----VARIABLES-----

    public Ship[] ships;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    public void Initialise() {
	
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="sourceSpawner"></param>
    /// <param name="requestedShipClass"></param>
    /// <returns></returns>
    public ShipController SpawnShip(Vector3 position, Quaternion rotation, SpawnRestriction sourceSpawner, string requestedShipClass) {
        GameObject prefab = null;
        foreach(Ship ship in ships) {
            if (ship.shipClass == requestedShipClass && (ship.shipSpawnRestriction == sourceSpawner || ship.shipSpawnRestriction == SpawnRestriction.EITHER)) {
                prefab = ship.shipPrefab;
                break;
            }
        }

        if (prefab == null) {
            Debug.LogError("Searching for a non existent ship");
            return null;
        } else {
            GameObject shipInstance = Instantiate(prefab, position, rotation);
            return shipInstance.GetComponent<ShipController>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="sourceSpawner"></param>
    /// <param name="requestedShipClass"></param>
    /// <returns></returns>
    public ShipController SpawnShip(Vector3 position, Vector3 direction, SpawnRestriction sourceSpawner, string requestedShipClass) {
        return SpawnShip(position, Quaternion.LookRotation(direction, Vector3.up), sourceSpawner, requestedShipClass);
    }

    //-----GIZMOS-----
    //public bool drawGizmos;
    void OnDrawGizmos() {
	
	}
	
}
