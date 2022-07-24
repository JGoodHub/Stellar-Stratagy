using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NavigationPlane
{

	private static Plane? _navPlane;

	public static Plane NavPlane => _navPlane ??= new Plane(Vector3.up, 0);

	public static Vector3 RaycastNavPlane()
	{
		Ray mouseRay = CameraHelper.GetMouseRay();
		NavPlane.Raycast(mouseRay, out float distance);

		return mouseRay.GetPoint(distance);
	}

	public static Vector2 RaycastNavPlane2D()
	{
		Vector3 navPlanePoint = RaycastNavPlane();
		return new Vector2(navPlanePoint.x, navPlanePoint.z);
	}

}
