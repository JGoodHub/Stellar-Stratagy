using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[Header("Entity")]
	public new string name;
	public Type type;
	public bool isStatic;
	[Space]
	[SerializeField] private GameObject selectionRing;
	[SerializeField] private float selectionRadius;
	[SerializeField] private float orbitRadius;

	public float SelectionRadiusSqrd => selectionRadius * selectionRadius;

	[Header("Factions")]
	public GameManager.Faction alignment;

	private void OnEnable()
	{
		SelectionController.RegisterEntity(this);
	}

	private void OnDisable()
	{
		SelectionController.UnregisterEntity(this);
	}

	protected virtual void Start()
	{
		SetSelected(false);
	}

	public void SetSelected(bool state)
	{
		if (selectionRing != null)
			selectionRing.SetActive(state);
	}

	private void OnDrawGizmos()
	{
		switch (alignment)
		{
			case GameManager.Faction.FRIENDLY:
				Gizmos.color = Color.green;
				break;
			case GameManager.Faction.ENEMY:
				Gizmos.color = Color.red;
				break;
			case GameManager.Faction.NEUTRAL:
				Gizmos.color = Color.yellow;
				break;
			case GameManager.Faction.NONE:
				Gizmos.color = Color.white;
				break;
		}

		GizmoExtensions.DrawWireCircle(transform.position, selectionRadius);
	}

	public enum Type
	{
		SHIP,
		STATION,
		ASTEROID
	}
}
