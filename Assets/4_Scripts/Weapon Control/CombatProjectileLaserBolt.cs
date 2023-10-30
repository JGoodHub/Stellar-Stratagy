using System;
using DG.Tweening;
using GoodHub.Core.Runtime;
using UnityEngine;

public class CombatProjectileLaserBolt : MonoBehaviour, IFreezeable
{
	[SerializeField] private GameObject _shieldHitEffect;
	[SerializeField] private GameObject _hullHitEffect;

	[SerializeField] private int _turnLifetime = 4;
	[SerializeField] private int damage = 1;

	private float _speed;
	private float _timeAlive;
	private int _sourceId;
	private bool _frozen;

	private Vector3 _previousFramePosition;

	public void Initialise(Vector3 origin, Vector3 direction, float speed, int sourceId)
	{
		transform.position = origin;
		transform.forward = direction;

		_speed = speed;
		_sourceId = sourceId;

		ProjectileController.RegisterProjectile(this);
	}

	private void Update()
	{
		if (_frozen)
			return;

		_timeAlive += Time.deltaTime;

		if (_timeAlive >= TurnController.Singleton.TurnRealtimeDuration * _turnLifetime)
		{
			Destroy(gameObject);
			return;
		}

		transform.Translate(transform.forward * (_speed * Time.deltaTime), Space.World);

		Vector3 rayDir = transform.position - _previousFramePosition;
		if (Physics.Raycast(_previousFramePosition, rayDir, out RaycastHit rayHit, rayDir.magnitude))
		{
			GameObject hitGO = rayHit.collider.gameObject;
			if (hitGO.layer == 10) // SelectableEntity Layer
			{
				SelectableEntity entity = hitGO.GetComponentInParent<SelectableEntity>();

				if (entity.id != _sourceId)
				{
					StatsController stats = entity.GetComponent<StatsController>();

					if (hitGO.CompareTag("Hull"))
					{
						stats.DealDamage(damage);
						Destroy(Instantiate(_hullHitEffect, transform.position, Quaternion.LookRotation(-transform.forward)), 5f);
					}
					else if (hitGO.CompareTag("Shield"))
					{
						stats.DealDamage(damage);
						Destroy(Instantiate(_shieldHitEffect, transform.position, Quaternion.LookRotation(rayHit.normal)), 5f);
					}
					
					Destroy(gameObject);
				}
			}
			
			return;
		}

		_previousFramePosition = transform.position;
	}

	private void OnDestroy()
	{
		ProjectileController.UnregisterProjectile(this);
	}

	public void Freeze()
	{
		_frozen = true;
	}

	public void Unfreeze()
	{
		_frozen = false;
	}

}
