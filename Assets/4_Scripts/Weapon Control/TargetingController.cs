using System;
using GoodHub.Core.Runtime;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class TargetingController : ShipComponent
{
    public float range;
    public ShipController target;

    public bool HasTarget => target != null;
    
    public float TargetDistance => target == null ? float.MaxValue : Vector3.Distance(transform.position, target.transform.position);

    private bool lockTarget = false;

    public bool debugLogs;

    public event Action<ShipController> OnTargetChanged;

    private void Start()
    {
        SelectionController.Singleton.OnSelectionChanged += SelectionChanged;
    }

    private void SelectionChanged(object sender, SelectableEntity oldEntity, SelectableEntity newEntity)
    {
        if (newEntity == null) // || newEntity.alignment == ShipController.alignment)
            return;

        SetTarget(newEntity.GetComponent<ShipController>());
    }

    public bool IsShipInRange(ShipController otherShip)
    {
        Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 otherPos = new Vector2(otherShip.transform.position.x, otherShip.transform.position.z);

        return Vector2.Distance(myPos, otherPos) <= range;
    }

    public bool SetTarget(ShipController other, bool forceSet = false)
    {
        if (other == null)
        {
            target = null;
            OnTargetChanged?.Invoke(target);
            return false;
        }

        bool targetInRange = IsShipInRange(other);

        if (targetInRange || forceSet)
        {
            target = other;
            OnTargetChanged?.Invoke(target);

            if (forceSet)
                lockTarget = true;

            Debug.Log($"Targeter: New target acquired {other.gameObject.name}");
        }

        return targetInRange;
    }

    private void Update()
    {
        if (target != null && (IsShipInRange(target) == false && lockTarget == false))
        {
            target = null;
            OnTargetChanged?.Invoke(target);
        }
    }

    //-----GIZMOS-----
    public bool drawGizmos;

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            GizmoExtensions.DrawWireCircle(transform.position, range, Color.red);
        }
    }
}