using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
	//-----VARIABLES-----

	public ShipController ShipController => GetComponent<ShipController>();


	[Header("Resources")]
	public ResourceStat hullStat = new ResourceStat(ResourceType.HULL, 0, 10);
	public ResourceStat shieldStat = new ResourceStat(ResourceType.SHIELD, 0, 10);
	public ResourceStat fuelStat = new ResourceStat(ResourceType.FUEL, 0, 10);
	public ResourceStat missileStat = new ResourceStat(ResourceType.MISSILE, 0, 10);
	public ResourceStat crewStat = new ResourceStat(ResourceType.CREW, 0, 10);

	public event Action<ResourceType, float, float> OnResourceValueChanged;
	public event Action<ResourceType> OnResourceMinimumReached;
	public event Action<ResourceType> OnResourceMaximumReached;


	public GameObject shieldParent;
	public GameObject shipExplosionPrefab;

	public bool isFriendly;

	//-----METHODS-----

	private void Start()
	{
		foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
		{
			ResetResource(type);
		}
	}

	public ResourceStat GetResource(ResourceType type)
	{
		switch (type)
		{
			case ResourceType.HULL:
				return hullStat;
			case ResourceType.SHIELD:
				return shieldStat;
			case ResourceType.FUEL:
				return fuelStat;
			case ResourceType.MISSILE:
				return missileStat;
			case ResourceType.CREW:
				return crewStat;
			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, null);
		}
	}

	public void ModifyResource(ResourceType type, float amount)
	{
		ResourceStat stat = GetResource(type);
		if (stat != null)
		{
			//Update the stat
			float oldValue = stat.current;
			stat.current = Mathf.Clamp(stat.current + amount, 0, stat.max);

			//Handle ship reactions to stat changes
			switch (type)
			{
				case ResourceType.HULL: //Should be made into a toggle
					if (stat.current <= 0)
						SelfDestruct();
					break;
				case ResourceType.SHIELD: //Should be placed in a shield controller for more parameters
					if (stat.current > 0)
						RaiseShield();
					else
						DropShield();
					break;
				case ResourceType.FUEL:
					if (stat.current <= 0)
						ShipController.Helm.SetSpeedCap(2);
					else
						ShipController.Helm?.SetSpeedCap(FlightController.SPEED_DIVISIONS);
					break;

			}
			
			//Call events
			OnResourceValueChanged?.Invoke(type, oldValue, stat.current);

			if (stat.current == 0)
				OnResourceMinimumReached?.Invoke(type);
			else if (Mathf.Approximately(stat.current, stat.max))
				OnResourceMaximumReached?.Invoke(type);
		}
	}

	public void ClearResource(ResourceType type)
	{
		ModifyResource(type, -999999999);
	}

	public void ResetResource(ResourceType type)
	{
		ModifyResource(type, 999999999);
	}

	private void RaiseShield()
	{
		shieldParent.SetActive(true);
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


[Serializable]
public class ResourceStat
{
	public ResourceType type;
	public float current;
	public float max;

	public float Normalised => current / max;

	public ResourceStat(ResourceType type, int current, int max)
	{
		this.type = type;
		this.current = current;
		this.max = max;
	}
}

public enum ResourceType
{
	HULL,
	SHIELD,
	FUEL,
	MISSILE,
	CREW
}
