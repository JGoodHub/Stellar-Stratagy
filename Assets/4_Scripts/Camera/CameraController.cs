using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{

    private new Camera camera;

    [Header("Pan Controls")]
    public float lerpingFactor;
    [SerializeField] private Transform target;

    [Header("Zoom Controls")]
    public float maxZoom;
    public float minZoom;
    public float zoomSpeed;

    [Header("Orbit Controls")]
    public float orbitSpeedHorizontal;
    private Quaternion resetRotation;

    private RaycastHit frameRayHit;
    public RaycastHit FrameRayHit { get => frameRayHit; }


    public void Start()
    {
        camera = GetComponentInChildren<Camera>();
        resetRotation = transform.rotation;
    }

    /// <summary>
    /// Get the ray from the camera is the direction of the mouse
    /// </summary>
    /// <returns></returns>
    public Ray GetCameraRay()
    {
        return camera.ScreenPointToRay(Input.mousePosition);
    }

    void Update()
    {
        Physics.Raycast(GetCameraRay(), out frameRayHit, 1000f);

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
            transform.position = Vector3.Lerp(transform.position, target.position, lerpingFactor * Time.deltaTime);
        }
    }

    //-----GIZMOS-----
    [Header("Gizmo Controls")]
    public bool drawGizmos;
    void OnDrawGizmos()
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
