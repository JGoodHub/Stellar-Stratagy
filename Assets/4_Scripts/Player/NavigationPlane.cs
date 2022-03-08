using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationPlane : Singleton<NavigationPlane> {

    private Plane navPlane;

    private void Start() {
        navPlane = new Plane(Vector3.up, 0);
    }

    public Vector3 RaycastNavPlane3D() {
        float distance;
        Ray cameraRay = CameraFollower.Instance.GetCameraRay();

        navPlane.Raycast(cameraRay, out distance);

        return cameraRay.GetPoint(distance);
    }

    public Vector2 RaycastNavPlane() {
        Vector3 navPlanePoint = RaycastNavPlane3D();
        return new Vector2(navPlanePoint.x, navPlanePoint.z);
    }

}
