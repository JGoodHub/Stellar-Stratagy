using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[Header("Entity")]
	public new string name;
	public Type type;
	[Space]
	[SerializeField] private GameObject selectionRing;
	[SerializeField] private float selectionRadius;

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

	public enum Type
	{
		SHIP,
		STATION,
		ASTEROID,
		WARPGATE
	}
}
