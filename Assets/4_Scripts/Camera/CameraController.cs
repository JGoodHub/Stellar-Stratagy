using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{

	private new Camera camera;

	[Header("Pan Controls")]
	public float smoothTime;
	[SerializeField] private Transform target;

	[Header("Zoom Controls")]
	public float maxZoom;
	public float minZoom;
	public float zoomSpeed;

	[Header("Orbit Controls")]
	public float orbitSpeedHorizontal;
	private Quaternion resetRotation;

	public void Start()
	{
		camera = GetComponentInChildren<Camera>();
		resetRotation = transform.rotation;
	}

	public Ray GetCameraRay()
	{
		return camera.ScreenPointToRay(Input.mousePosition);
	}

	public bool GetCameraRaycast(out RaycastHit rayHit)
	{
		return Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out rayHit, 2000f);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate(Vector3.up, orbitSpeedHorizontal * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(Vector3.up, -orbitSpeedHorizontal * Time.deltaTime, Space.World);
		}
		else if (Input.GetKeyDown(KeyCode.Home))
		{
			transform.rotation = resetRotation;
		}
	}

	private void LateUpdate()
	{
		if (target != null)
		{
			Vector3 velocity = Vector3.zero;
			transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
		}
	}

//-----GIZMOS-----
	[Header("Gizmo Controls")]
	public bool drawGizmos;

	private void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Vector3 yLockedForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
			Vector3 yLockedRight = new Vector3(transform.right.x, 0, transform.right.z).normalized;

			Gizmos.color = (Color.cyan + Color.blue) / 2f;
			Gizmos.DrawRay(transform.position, yLockedForward * 50f);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, yLockedRight * 50f);
		}
	}
}
