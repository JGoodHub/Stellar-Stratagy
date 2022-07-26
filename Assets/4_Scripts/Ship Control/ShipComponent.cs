using UnityEngine;


public class ShipComponent : MonoBehaviour
{

	private CombatShipController _shipController;
	
	protected CombatShipController ShipController => _shipController ??= GetComponent<CombatShipController>();
}
