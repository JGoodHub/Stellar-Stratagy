using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInput : Singleton<TouchInput>
{
	public struct TouchData
	{
		public Vector2 DownPosition;
		public Vector2 CurrentPosition;
		public Vector2 UpPosition;

		public bool IsDown;
		public bool IsDragging;

		public bool DownOverUI;
		public bool CurrentlyOverUI;
		public bool UpOverUI;
	}

	public static event Action<TouchData> TouchDown;
	public static event Action<TouchData> TouchUp;
	
	public static event Action<TouchData> TouchDragEnter;
	public static event Action<TouchData> TouchDragStay;
	public static event Action<TouchData> TouchDragEnd;

	public float _relativeDistanceToDrag = 0.04f;
	private float _pixelDistanceToDrag;
	private float _currentPixelDragDistance;

	private static TouchData _frameTouchData;
	public static TouchData FrameTouchData => _frameTouchData;

	public bool debugEvents;

	private void Awake()
	{
		_pixelDistanceToDrag = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2)) * _relativeDistanceToDrag;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			TriggerTouchDown();
		}

		if (Input.GetMouseButton(0))
		{
			_frameTouchData.IsDown = true;

			_currentPixelDragDistance = Vector2.Distance(_frameTouchData.DownPosition, Input.mousePosition);

			if (_frameTouchData.IsDragging || _currentPixelDragDistance >= _pixelDistanceToDrag)
			{
				if (!_frameTouchData.IsDragging)
				{
					TriggerTouchDragEnter();
				}

				TriggerTouchDragStay();
			}
		}
		else
		{
			_frameTouchData.IsDown = false;

			if (_frameTouchData.IsDragging)
			{
				TriggerTouchDragExit();
			}

			_currentPixelDragDistance = 0;
		}

		if (Input.GetMouseButtonUp(0))
		{
			TriggerTouchUp();
		}
	}

	private void TriggerTouchDown()
	{
		_frameTouchData.DownPosition = Input.mousePosition;
		_frameTouchData.DownOverUI = EventSystem.current.IsPointerOverGameObject();

		TouchDown?.Invoke(_frameTouchData);

		if (debugEvents)
			Debug.Log("[DragCameraController]: Touch down triggered");
	}

	private void TriggerTouchUp()
	{
		_frameTouchData.UpPosition = Input.mousePosition;
		_frameTouchData.UpOverUI = EventSystem.current.IsPointerOverGameObject();

		TouchUp?.Invoke(_frameTouchData);

		if (debugEvents)
			Debug.Log("[DragCameraController]: Touch up triggered");
	}

	private void TriggerTouchDragEnter()
	{
		_frameTouchData.IsDragging = true;

		TouchDragEnter?.Invoke(_frameTouchData);

		if (debugEvents)
			Debug.Log("[DragCameraController]: Touch drag enter triggered");
	}

	private void TriggerTouchDragStay()
	{
		_frameTouchData.CurrentPosition = Input.mousePosition;
		_frameTouchData.CurrentlyOverUI = EventSystem.current.IsPointerOverGameObject();

		TouchDragStay?.Invoke(_frameTouchData);

		if (debugEvents)
			Debug.Log("[DragCameraController]: Touch drag stay triggered");
	}

	private void TriggerTouchDragExit()
	{
		_frameTouchData.IsDragging = false;

		TouchDragEnd?.Invoke(_frameTouchData);

		if (debugEvents)
			Debug.Log("[DragCameraController]: Touch drag exit triggered");
	}
}
