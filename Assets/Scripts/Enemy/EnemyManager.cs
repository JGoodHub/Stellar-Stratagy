using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    //-----SINGLETON SETUP-----

	public static EnemyManager instance = null;
	
	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

    //-----VARIABLES-----

    private HashSet<ShipController> axisShips;

    //-----METHODS-----
	
	public void Initialise() {
        axisShips = new HashSet<ShipController>();

        axisShips.Add(ShipFactory.instance.SpawnShip(new Vector3(100, 0, 0), Quaternion.identity, ShipFactory.SpawnRestriction.ENEMY, "Dreadnought"));

        foreach (ShipController shipControl in axisShips) {
            shipControl.Initialise();
            shipControl.isAlliedShip = false;
            shipControl.FogMask.MarkAsEnemy();

            shipControl.transform.SetParent(gameObject.transform);
        }
    }
	
	//-----GIZMOS-----
	//public bool drawGizmos;
	void OnDrawGizmos() {
	
	}
	
}
