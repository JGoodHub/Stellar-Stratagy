using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
	//-----VARIABLES-----

	public ShipController Ship => GetComponent<ShipController>();

	[Header("Resources")]
	[SerializeField] private int _hullHP = 100;
	[SerializeField] private int _hullMaximum = 100;
	[SerializeField] private int _shieldHP = 100;
	[SerializeField] private int _shieldMaximum = 100;

	[SerializeField] private GameObject _shieldParent;
	[SerializeField] private GameObject _shipExplosionPrefab;

	private bool destroyed;

	public event Action<float> OnHullValueChanged;
	public event Action<float> OnShieldValueChanged;

	public float HullHP => _hullHP;
	public float ShieldHP => _shieldHP;

	//-----METHODS-----

	private void Start()
	{
		_hullHP = _hullMaximum;
		_shieldHP = _shieldMaximum;
	}

	public void DealDamage(int damage)
	{
		if (_shieldHP > 0)
		{
			if (_shieldHP > damage)
			{
				_shieldHP -= damage;
				damage = 0;
				OnShieldValueChanged?.Invoke(_shieldHP);
			}
			else
			{
				damage -= _shieldHP;
				DropShield();
			}
		}

		if (damage > 0)
		{
			if (_hullHP > damage)
			{
				_hullHP -= damage;
				damage = 0;
				OnHullValueChanged?.Invoke(_hullHP);
			}
			else
			{
				damage -= _hullHP;
				Destruct();
			}
		}
	}

	public void Heal(int amount)
	{ }

	private void RaiseShield(int amount)
	{
		_shieldHP = amount;
		_shieldParent.SetActive(true);

		OnShieldValueChanged?.Invoke(_shieldHP);
	}

	private void DropShield()
	{
		_shieldHP = 0;
		_shieldParent.SetActive(false);

		OnShieldValueChanged?.Invoke(0);
	}

	private void Destruct()
	{
		destroyed = true;
		_hullHP = 0;

		OnHullValueChanged?.Invoke(0);

		GameObject shipExplosionObject = Instantiate(_shipExplosionPrefab, transform.position, Quaternion.identity);

		Destroy(shipExplosionObject, 5f);

		Destroy(gameObject);

		foreach (Collider collider in GetComponentsInChildren<Collider>())
		{
			collider.enabled = false;
		}
	}
}

[Serializable]
public class ResourceStat
{
	public float current;
	public float max;

	public float Normalised => current / max;

	public ResourceStat(int current, int max)
	{
		this.current = current;
		this.max = max;
	}
}

public enum StatType
{
	HULL,
	SHIELD,
	CREW
}
