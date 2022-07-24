using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragController : SceneSingleton<CameraDragController>
{
	private bool _dragEnabled = true;
	private Vector3 _dragStartPosition;
	private Vector3 _dragPosition;

	[SerializeField] private Bounds _cameraBoundary;

	public bool DragEnabled
	{
		get => _dragEnabled;
		set => _dragEnabled = value;
	}
	
	private void Start()
	{
		TouchInput.TouchDown += TouchInputOnTouchDown;
		TouchInput.TouchDragStay += TouchInputOnTouchDragStay;
	}

	private void OnDestroy()
	{
		TouchInput.TouchDown -= TouchInputOnTouchDown;
		TouchInput.TouchDragStay -= TouchInputOnTouchDragStay;
	}

	private void TouchInputOnTouchDown(TouchInput.TouchData touch)
	{
		if (CameraHelper.IsMouseOverUI() || DragEnabled == false)
			return;

		Vector3 planeIntersectionPoint = NavigationPlane.RaycastNavPlane();
		_dragStartPosition = planeIntersectionPoint;
	}

	private void TouchInputOnTouchDragStay(TouchInput.TouchData touch)
	{
		if (CameraHelper.IsMouseOverUI() || DragEnabled == false)
			return;

		Vector3 planeIntersectionPoint = NavigationPlane.RaycastNavPlane();
		Vector3 dragDirection = _dragStartPosition - planeIntersectionPoint;

		_dragPosition = transform.position + dragDirection;
	}

	private void Update()
	{
		transform.position = Vector3.Lerp(transform.position, _dragPosition, 0.5f);
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, _cameraBoundary.min.x, _cameraBoundary.max.x), 0f, Mathf.Clamp(transform.position.z, _cameraBoundary.min.z, _cameraBoundary.max.z));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_cameraBoundary.center, _cameraBoundary.size);
	}
}
