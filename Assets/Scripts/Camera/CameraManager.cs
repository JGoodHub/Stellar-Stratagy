using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    //-----SINGLETON SETUP-----

    public static CameraManager instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    //-----VARIABLES-----

    [Header("Scene Camera Refrence")]
    public Camera mainCamera;

    [Header("Pan Controls")]
    public float panSpeed;

    [Header("Zoom Controls")]
    public float maxZoom;
    public float minZoom;
    public float zoomSpeed;

    [Header("Rotation Controls")]
    public float orbitSpeedHorizontal;
    private Quaternion resetRotation;

    private RaycastHit frameRayHit;
    public RaycastHit FrameRayHit { get => frameRayHit; }

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    public void Initalise () {
		resetRotation = transform.rotation;
	}

    /// <summary>
    /// Get the ray from the camera is the direction of the mouse
    /// </summary>
    /// <returns></returns>
    public Ray GetCameraRay() {
        return mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    /// <summary>
    /// Fire a raycast using the camera ray and return what it hit, if anything
    /// </summary>
    /// <returns></returns>
    public RaycastHit FireRaycastFromMousePosition() {
        Physics.Raycast(GetCameraRay(), out RaycastHit hit, 1000f);
        return hit;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update () {
        frameRayHit = FireRaycastFromMousePosition();

        if (Input.GetKey (KeyCode.Delete)) {
			transform.Rotate (Vector3.up, orbitSpeedHorizontal * Time.deltaTime, Space.World);
		} else if (Input.GetKey (KeyCode.PageDown)) {
			transform.Rotate (Vector3.up, -orbitSpeedHorizontal * Time.deltaTime, Space.World);
		}

		if (Input.GetKeyDown (KeyCode.End)) {
			transform.rotation = resetRotation;
		}

		Vector3 localPosTemp = Camera.main.transform.localPosition;
		localPosTemp.z = Mathf.Clamp (localPosTemp.z + Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
		Camera.main.transform.localPosition = localPosTemp;		
	}

    /// <summary>
    /// 
    /// </summary>
	void FixedUpdate () {
		Vector3 velocity = Vector3.zero;

		Vector3 yLockedForward = new Vector3 (transform.forward.x, 0, transform.forward.z).normalized;
		Vector3 yLockedRight = new Vector3 (transform.right.x, 0, transform.right.z).normalized;

		float maxVelocity = (yLockedForward * panSpeed * Time.fixedDeltaTime).magnitude;
        
		if (Input.GetKey (KeyCode.D)) {
			velocity += yLockedRight * panSpeed * Time.fixedDeltaTime;
		} 

		if (Input.GetKey (KeyCode.A)) {
			velocity += -yLockedRight * panSpeed * Time.fixedDeltaTime;
		}

		if (Input.GetKey (KeyCode.W)) {
			velocity += yLockedForward * panSpeed * Time.fixedDeltaTime;
		}

		if (Input.GetKey (KeyCode.S)) {
			velocity += -yLockedForward * panSpeed * Time.fixedDeltaTime;
		}

		transform.position += Vector3.ClampMagnitude (velocity, maxVelocity);
	}

    //-----GIZMOS-----
    [Header("Gizmo Controls")]
    public bool drawGizmos;
	void OnDrawGizmos () {
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
