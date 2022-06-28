using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : SceneSingleton<SelectionController>
{

	private static HashSet<Entity> allEntities = new HashSet<Entity>();
	private Entity selectedEntity;

	public static Entity SelectedEntity => Instance.selectedEntity;

	private EventSystem eventSystem;

	//Events
	public delegate void SelectionChange(object sender, Entity oldEntity, Entity newEntity);
	public delegate void NavPlaneQuery(object sender, Vector2 navPlanePoint);

	public event SelectionChange OnSelectionChanged;
	public event NavPlaneQuery OnNavPlaneQuery;

	private void Start()
	{
		eventSystem = FindObjectOfType<EventSystem>();
	}

	public static void RegisterEntity(Entity entity)
	{
		if (allEntities.Contains(entity) == false)
			allEntities.Add(entity);
	}

	public static void UnregisterEntity(Entity entity)
	{
		if (entity == SelectedEntity)
			Instance.ClearCurrentSelection();

		if (allEntities.Contains(entity))
			allEntities.Remove(entity);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && eventSystem.IsPointerOverGameObject() == false)
		{
			Vector2 navPlanePoint = NavigationPlane.Instance.RaycastNavPlane();
			Entity newlySelectedEntity = CheckForChangeInSelection(navPlanePoint);

			if (selectedEntity != null && newlySelectedEntity == null)
			{
				selectedEntity.SetSelected(false);

				OnSelectionChanged?.Invoke(this, selectedEntity, null);
				selectedEntity = null;
			}
			else if (newlySelectedEntity != selectedEntity)
			{
				if (selectedEntity != null)
					selectedEntity.SetSelected(false);

				newlySelectedEntity.SetSelected(true);

				OnSelectionChanged?.Invoke(this, selectedEntity, newlySelectedEntity);
				selectedEntity = newlySelectedEntity;
			}

			OnNavPlaneQuery?.Invoke(this, navPlanePoint);
		}
	}

	public static Entity CheckForChangeInSelection(Vector2 selectionPoint)
	{
		foreach (Entity entity in allEntities)
		{
			float pointToEntitySqrd = (new Vector2(entity.transform.position.x, entity.transform.position.z) - selectionPoint).sqrMagnitude;

			if (pointToEntitySqrd <= entity.SelectionRadiusSqrd)
			{
				return entity;
			}
		}

		return null;
	}

	public static List<Entity> GetNearbyEntities(Vector3 queryPosition, float radius)
	{
		List<Entity> nearbyEntities = new List<Entity>();

		foreach (Entity entity in allEntities)
		{
			if (Vector3.Distance(queryPosition, entity.transform.position) <= radius)
				nearbyEntities.Add(entity);
		}

		return nearbyEntities;
	}

	public void ClearCurrentSelection()
	{
		if (selectedEntity != null)
		{
			selectedEntity.SetSelected(false);
			OnSelectionChanged?.Invoke(this, selectedEntity, null);
		}

		selectedEntity = null;
	}

}
