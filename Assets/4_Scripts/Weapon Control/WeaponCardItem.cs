using System;
using GoodHub.Core.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponCardItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] protected GameObject _pointReticlePrefab;
	protected GameObject _pointReticleObject;

	[SerializeField] protected GameObject _entityReticlePrefab;
	protected GameObject _entityReticleObject;

	protected int _id;
	protected WeaponConfig _weaponConfig;
	protected SelectableEntity _targetEntity;
	protected Vector3 _targetPosition;

	public virtual void OnBeginDrag(PointerEventData eventData)
	{ }

	public virtual void OnDrag(PointerEventData eventData)
	{ }

	public virtual void OnEndDrag(PointerEventData eventData)
	{ }

	public void Initialise(WeaponConfig weaponConfig)
	{
		_id = int.Parse($"{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}");

		_weaponConfig = weaponConfig;

		_pointReticleObject = Instantiate(_pointReticlePrefab, Vector3.up * 1000, Quaternion.identity);
		_entityReticleObject = Instantiate(_entityReticlePrefab, Vector3.up * 1000, Quaternion.identity);
	}
}
