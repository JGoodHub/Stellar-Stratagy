using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour, IFreezeable
{

	public float speed;
	public float damage;
	private ShipController targetShip;

	public GameObject explosionPrefab;

	public void SetTarget(ShipController target)
	{
		targetShip = target;
	}

	private void Update()
	{
		transform.LookAt(targetShip.transform);
		transform.Translate(Vector3.forward * (speed * Time.deltaTime));

		if (Vector3.Distance(transform.position, targetShip.transform.position) <= 5f)
		{
			//targetShip.Stats.Modify(StatType.HULL, -damage);
			
			GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			Destroy(explosion, 5f);
			
			Destroy(gameObject);
		}
	}

	public void Freeze()
	{
		throw new NotImplementedException();
	}
	public void Unfreeze()
	{
		throw new NotImplementedException();
	}
}
