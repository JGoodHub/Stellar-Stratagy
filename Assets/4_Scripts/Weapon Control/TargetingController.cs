using System;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class TargetingController : ShipComponent
{

	public float range;
	public ShipController target;

	public bool HasTarget => target != null;

	public bool debugLogs;
	
	public event Action<ShipController> OnTargetChanged;
	
	private void Start()
	{
		SelectionController.Instance.OnSelectionChanged += SelectionChanged;
	}
	private void SelectionChanged(object sender, Entity oldEntity, Entity newEntity)
	{
		if (newEntity == null || newEntity.alignment == Ship.alignment)
			return;

		SetTarget(newEntity.GetComponent<ShipController>());
	}

	public bool IsShipInRange(ShipController otherShip)
	{
		Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
		Vector2 otherPos = new Vector2(otherShip.transform.position.x, otherShip.transform.position.z);

		return Vector2.Distance(myPos, otherPos) <= range;
	}

	public bool SetTarget(ShipController other)
	{
		if (other == null)
		{
			target = null;
			OnTargetChanged?.Invoke(target);
			return false;
		}
		
		bool targetInRange = IsShipInRange(other);

		if (targetInRange)
		{
			target = other;
			OnTargetChanged?.Invoke(target);
			
			Debug.Log($"Targeter: New target acquired {other.gameObject.name}");
		}

		return targetInRange;
	}

	private void Update()
	{
		if (target != null && IsShipInRange(target) == false)
		{
			target = null;
			OnTargetChanged?.Invoke(target);
		}
	}
	
	//-----GIZMOS-----
	public bool drawGizmos;

	private void OnDrawGizmos() {
		if (drawGizmos) {
			Gizmos.color = Color.green;
			GizmoExtensions.DrawWireCircle(transform.position, range);
		}
	}
}