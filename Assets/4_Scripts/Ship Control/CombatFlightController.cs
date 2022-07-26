using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatFlightController : ShipComponent, IBeginDragHandler, IEndDragHandler, IDragHandler
{

	[SerializeField] private float _maxFlightDistancePerTurn = 200;
	[SerializeField] private float _maxAngleDeltaPerTurn = 90;
	
	private PathUtils.BezierCurve3 _flightPath;
	private bool _isFlightPathValid = false;

	[SerializeField] private LineRenderer _flightPathRenderer;
	[SerializeField] private Material _validFlightPathMaterial;
	[SerializeField] private Material _invalidFlightPathMaterial;
	private float _forwardHandleOffset = 0;
	[SerializeField] private GameObject _ghostShipPrefab;
	private GameObject _ghostShipGO;

	[SerializeField] private AnimationCurve _pathEasingCurve;

	private void Start()
	{
		_ghostShipGO = Instantiate(_ghostShipPrefab, Vector3.up * 5000, Quaternion.identity);
	}

	public void FollowFlightPath()
	{
		if (_isFlightPathValid == false)
			return;

		_flightPathRenderer.positionCount = 0;
		_ghostShipGO.transform.position = Vector3.up * 1000;

		DOVirtual.Float(0f, 1f, TurnController.TURN_DURATION, t =>
		{
			transform.position = PathUtils.GetPointOnBezierCurve(_flightPath.Start, _flightPath.Mid, _flightPath.End, t);
			transform.forward = PathUtils.GetDirectionOnBezierCurve(_flightPath.Start, _flightPath.Mid, _flightPath.End, t);
		}).SetEase(_pathEasingCurve);

		_isFlightPathValid = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.OurTurn == false)
			return;

		if (PlayerCombatController.Instance.FocusedShip != ShipController)
		{
			SelectionController.Instance.SetSelection(ShipController);
		}
		
		_flightPath = new PathUtils.BezierCurve3();
		
		CameraDragController.Instance.DragEnabled = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.OurTurn == false || PlayerCombatController.Instance.FocusedShip != ShipController)
			return;

		Vector3 navPlanePoint = NavigationPlane.RaycastNavPlane();
		_flightPath = PathUtils.GetBezierCurve(transform.position, transform.position + transform.forward * _forwardHandleOffset, navPlanePoint, 12);
		_flightPathRenderer.positionCount = 12;
		_flightPathRenderer.SetPositions(_flightPath.curve);

		_forwardHandleOffset = _flightPath.Length() / 2f;
		Vector3 _flightPathEndDirection = PathUtils.GetDirectionOnBezierCurve(_flightPath.Start, _flightPath.Mid, _flightPath.End, 1f);

		_isFlightPathValid = _flightPath.Length() <= _maxFlightDistancePerTurn && Vector3.Angle(transform.forward, _flightPathEndDirection) <= _maxAngleDeltaPerTurn;

		_flightPathRenderer.material = _isFlightPathValid ? _validFlightPathMaterial : _invalidFlightPathMaterial;

		_ghostShipGO.transform.position = _flightPath.End;
		_ghostShipGO.transform.forward = _flightPathEndDirection;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (PlayerCombatController.Instance.OurTurn == false || PlayerCombatController.Instance.FocusedShip != ShipController)
			return;

		if (_isFlightPathValid == false)
		{
			_flightPath = new PathUtils.BezierCurve3();
			_flightPathRenderer.positionCount = 0;

			_ghostShipGO.transform.position = Vector3.up * 1000;
		}
		
		CameraDragController.Instance.DragEnabled = true;
	}

}
