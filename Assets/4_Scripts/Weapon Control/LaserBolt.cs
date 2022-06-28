using System;
using DG.Tweening;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{

	private float _speed;
	private float _timeAlive;

	public bool _frozen;

	public void Initialise(Vector3 origin, Vector3 direction, float speed)
	{
		transform.position = origin;
		transform.forward = direction;

		_speed = speed;

		ProjectileTracker.RegisterLaserBolt(this);
	}

	private void Update()
	{
		if (_frozen)
			return;

		_timeAlive += Time.deltaTime;

		if (_timeAlive >= CombatTurnController.TURN_DURATION * 4)
		{
			ProjectileTracker.UnregisterLaserBolt(this);
			Destroy(gameObject);
			return;
		}

		transform.Translate(transform.forward * (_speed * Time.deltaTime), Space.World);
	}

	private void OnTriggerEnter(Collider other)
	{ }

}
