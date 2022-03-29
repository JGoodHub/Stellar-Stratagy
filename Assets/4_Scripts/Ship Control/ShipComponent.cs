using UnityEngine;


public class ShipComponent : MonoBehaviour
{
	public ShipController Ship => GetComponent<ShipController>();
	
	public TargetingController Targeter => GetComponent<TargetingController>();
	public LaserWeaponsController Lasers => GetComponent<LaserWeaponsController>();

	
	
	
}
