using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : Singleton<CameraController> {

    private new Camera camera;

    [Header("Follow Controls")]

    public float smoothTime;
    [SerializeField] private Transform target;

    private Vector3 velocity;

    public void Start() {
        camera = GetComponentInChildren<Camera>();
    }

    public Ray GetCameraRay() {
        return camera.ScreenPointToRay(Input.mousePosition);
    }

    public bool GetCameraRaycast(out RaycastHit rayHit) {
        return Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out rayHit, 100f);
    }

    private void LateUpdate() {
        if (target != null) {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        }
    }

    //-----GIZMOS-----
    [Header("Gizmo's")]
    public bool drawGizmos;

    private void OnDrawGizmos() {
        if (drawGizmos) {
            Vector3 yLockedForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 yLockedRight = new Vector3(transform.right.x, 0, transform.right.z).normalized;

            Gizmos.color = (Color.cyan + Color.blue) / 2f;
            Gizmos.DrawRay(transform.position, yLockedForward * 50f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, yLockedRight * 50f);
        }
    }
}
