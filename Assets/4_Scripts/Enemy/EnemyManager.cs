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

    [Header("Debug Variables")]
    public GameObject[] preExisitingShips;

    //-----METHODS-----

    public void Initialise() {
        axisShips = new HashSet<ShipController>();

        //axisShips.Add(ShipFactory.instance.SpawnShip(new Vector3(300, 0, 0), Quaternion.identity, ShipFactory.SpawnRestriction.ENEMY, "Dreadnought"));

        foreach (GameObject ship in preExisitingShips) {
            ShipController shipControl = ship.GetComponent<ShipController>();
            if (shipControl == null) {
                Debug.LogError("Non ship object placed in pre exisiting ships array");
            } else {
                axisShips.Add(shipControl);
            }
        }

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
