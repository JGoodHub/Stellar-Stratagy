using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : Singleton<SelectionController>
{

    public delegate void SelectionChange(object sender, Entity oldEntity, Entity newEntity);
    public delegate void SelectionTriggered(object sender, Entity selectedEntity);

    public event SelectionChange OnSelectionChanged;
    public event SelectionTriggered OnSelectionTriggered;

    private static HashSet<Entity> allEntities = new HashSet<Entity>();
    private Entity selectedEntity;

    private EventSystem eventSystem;

    public static void RegisterEntity(Entity entity)
    {
        if (allEntities.Contains(entity) == false)
            allEntities.Add(entity);
    }

    public static void UnregisterEntity(Entity entity)
    {
        if (allEntities.Contains(entity))
            allEntities.Remove(entity);
    }

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && eventSystem.IsPointerOverGameObject() == false)
        {
            Vector2 selectionPoint = NavigationPlane.Instance.RaycastNavPlane();
            Entity newSelectedEntity = CheckForSelection(selectionPoint);

            if (selectedEntity != null && newSelectedEntity == null)
            {
                selectedEntity.SetSelected(false);
                selectedEntity = null;
            }
            else
            {
                if (newSelectedEntity != selectedEntity)
                {
                    OnSelectionChanged?.Invoke(this, selectedEntity, newSelectedEntity);

                    selectedEntity?.SetSelected(false);
                    newSelectedEntity.SetSelected(true);

                    selectedEntity = newSelectedEntity;
                }
            }

            OnSelectionTriggered?.Invoke(this, selectedEntity);
        }
    }

    public static Entity CheckForSelection(Vector2 selectionPoint)
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

}