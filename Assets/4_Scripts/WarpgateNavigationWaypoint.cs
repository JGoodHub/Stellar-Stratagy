using UnityEngine;

public class WarpgateNavigationWaypoint : NavigationWaypoint
{

	[SerializeField] private Transform _departureTransform;
	
	public override void GetTargetPositionAndAlignment(out Vector3 position, out Vector3 forward, Transform source = null)
	{
		position = _departureTransform.position;
		forward = _departureTransform.forward;
	}

}
