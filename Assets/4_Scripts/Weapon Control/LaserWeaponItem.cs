using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserWeaponItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	public GameObject _targeterPrefab;
	private GameObject _targeterObject;

	public GameObject _laserPrefab;
	public int _fireCount;
	[Range(0.2f, 0.8f)] public float _firingPeriod;
	public float _laserSpeed;
	public float _arcSpread = 5f;

	private bool _validTarget;
	private Entity _targetEntity;
	private Vector3 _targetPosition;

	private void Start()
	{
		_targeterObject = Instantiate(_targeterPrefab, Vector3.up * 1000, Quaternion.identity);
	}

	public void FireLasersAtTarget()
	{
		if (_validTarget)
			StartCoroutine(FireLasersAtTargetCoroutine());
	}

	private IEnumerator FireLasersAtTargetCoroutine()
	{
		float fireInterval = (CombatTurnController.TURN_DURATION * _firingPeriod) / _fireCount;
		float time = 0f;
		
		WaitForSeconds fireIntervalYield = new WaitForSeconds(fireInterval);

		while (time < CombatTurnController.TURN_DURATION * _firingPeriod)
		{
			GameObject laserObject = Instantiate(_laserPrefab, PlayerCombatController.Instance.PlayerShip.transform.position, Quaternion.identity);
			LaserBolt laserBolt = laserObject.GetComponent<LaserBolt>();

			Vector3 origin = PlayerCombatController.Instance.PlayerShip.transform.position;
			Vector3 direction = (_targetEntity == null ? _targetPosition : _targetEntity.transform.position) - origin;

			float arcTheta = Random.Range(-_arcSpread, _arcSpread);
			direction = Quaternion.Euler(0f, arcTheta, 0f) * direction;
			Debug.DrawRay(origin, direction * 30f, Color.green, 60f);

			laserBolt.Initialise(origin, direction, _laserSpeed);

			time += fireInterval;
			yield return fireIntervalYield;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.ourTurn == false)
			return;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.ourTurn == false)
			return;

		if (OverUI())
		{
			_targeterObject.transform.position = Vector3.up * 1000;
		}
		else
		{
			Vector3 navPlanePoint = NavigationPlane.Instance.RaycastNavPlane3D();

			_targeterObject.transform.position = _targetPosition = navPlanePoint;

			List<Entity> nearbyEntities = SelectionController.GetNearbyEntities(navPlanePoint, 35f);

			if (nearbyEntities.Count > 0)
			{
				_targetEntity = nearbyEntities[0];
				_targeterObject.transform.position = _targetEntity.transform.position;
			}
			else
			{
				_targetEntity = null;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.ourTurn == false)
			return;

		_validTarget = OverUI() == false;

		if (_validTarget == false)
		{
			_targeterObject.transform.position = Vector3.up * 1000;
			_targetEntity = null;
		}
	}

	private static bool OverUI()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;

		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, raycastResults);

		foreach (RaycastResult raycastResult in raycastResults)
			if (raycastResult.gameObject.layer == 5)
				return true;

		return false;
	}
}
