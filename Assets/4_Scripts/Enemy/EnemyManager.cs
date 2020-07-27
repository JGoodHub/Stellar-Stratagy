using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{

    private HashSet<ShipController> enemyShips;

    [Header("Debug Variables")]
    public GameObject[] preExisitingShips;


    public void Start()
    {
        enemyShips = new HashSet<ShipController>();

        foreach (GameObject ship in preExisitingShips)
        {
            ShipController shipControl = ship.GetComponent<ShipController>();
            if (shipControl == null)
            {
                Debug.LogError("Non ship object placed in pre existing ships array");
            }
            else
            {
                enemyShips.Add(shipControl);
            }
        }

        foreach (ShipController shipControl in enemyShips)
        {
            shipControl.faction = GameManager.Faction.ENEMY;
            shipControl.FogMask.MarkAsEnemy();
        }
    }

}
