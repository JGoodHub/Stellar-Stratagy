using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrbitalController : ShipComponent
{
	// public int pointsCount;
	// public Transform pointsParent;
	// private List<Transform> waypoints = new List<Transform>();
	//
	// public OrbitalController orbitTarget;
	// private Transform targetWaypoint;
	// public bool orbitDirectionClockwise = true;
	//
	// public float nextNodeThreshold = 50f;
	
	public float orbitRadius;
	[SerializeField] private Transform rotatingAnchor;
	[SerializeField] private float rotatingAnchorSpeed;

	private void Update()
	{
		rotatingAnchor.Rotate(Vector3.up, rotatingAnchorSpeed * Time.deltaTime);
	}

	public Vector3 GetOrbitInsertionPoint(Transform shipTransform, bool clockwiseInsertion)
	{
		Vector3 shipToCentreDirection = transform.position - shipTransform.position;
		float shipTheta = Mathf.Asin(orbitRadius / shipToCentreDirection.magnitude) * Mathf.Rad2Deg;
		float orbitTheta = 90f - shipTheta;
		Vector3 pointOnOrbit = shipToCentreDirection.normalized * -orbitRadius;
		pointOnOrbit = Quaternion.Euler(0f, clockwiseInsertion ? orbitTheta : -orbitTheta, 0f) * pointOnOrbit;
		pointOnOrbit += transform.position;

		return pointOnOrbit;
	}

	

	//-----GIZMOS-----
	public bool drawGizmos;

	private void OnDrawGizmos()
	{
		
		
		
		
	}

}
