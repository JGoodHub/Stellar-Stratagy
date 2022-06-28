using UnityEngine;

public class OrbitableNavigationWaypoint : NavigationWaypoint
{

	public float orbitRadius;
	[SerializeField] private Transform rotatingAnchor;
	[SerializeField] private float rotatingAnchorSpeed;

	private void Update()
	{
		rotatingAnchor.Rotate(Vector3.up, rotatingAnchorSpeed * Time.deltaTime);
	}

	public override void GetTargetPositionAndAlignment(out Vector3 position, out Vector3 forward, Transform source = null)
	{
		if (source == null)
		{
			position = forward = Vector3.zero;
			return;
		}
		
		bool clockwiseInsertion = true;
		
		Vector3 shipToCentreDirection = transform.position - source.position;
		float shipTheta = Mathf.Asin(orbitRadius / shipToCentreDirection.magnitude) * Mathf.Rad2Deg;
		float orbitTheta = 90f - shipTheta;
		Vector3 pointOnOrbit = shipToCentreDirection.normalized * -orbitRadius;
		pointOnOrbit = Quaternion.Euler(0f, clockwiseInsertion ? orbitTheta : -orbitTheta, 0f) * pointOnOrbit;
		pointOnOrbit += transform.position;

		position = pointOnOrbit;
		forward = Vector3.Cross(transform.position - pointOnOrbit, Vector3.up).normalized;
	}
	
	public void AddOrbitingEntity(Entity entity, bool clockwise)
	{
		
	}

}
