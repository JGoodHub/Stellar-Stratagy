using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : SceneSingleton<SelectionController>
{
    private HashSet<Entity> _allEntities = new HashSet<Entity>();

    private Entity _selectedEntity;

    // Properties

    public Entity SelectedEntity => _selectedEntity;

    // Events
    
    public delegate void SelectionChange(object sender, Entity oldEntity, Entity newEntity);
    
    public event SelectionChange OnSelectionChanged;

    private void Start()
    {
        TouchInput.OnTouchClick += OnTouchClick;
    }

    private void OnDestroy()
    {
        TouchInput.OnTouchClick -= OnTouchClick;
    }
    
    
    public void RegisterEntity(Entity entity)
    {
        if (_allEntities.Contains(entity) == false)
            _allEntities.Add(entity);
    }

    public void UnregisterEntity(Entity entity)
    {
        if (entity == SelectedEntity)
            Instance.ClearSelection();

        if (_allEntities.Contains(entity))
            _allEntities.Remove(entity);
    }

    private void OnTouchClick(TouchInput.TouchData touchData)
    {
        if (touchData.DownOverUI || touchData.UpOverUI)
            return;

        Vector2 selectionPoint = NavigationPlane.RaycastNavPlane2D();
        Entity newlySelectedEntity = GetFirstSelectedEntity(selectionPoint);

        SetSelection(newlySelectedEntity);
    }
    
    public void SetSelection(Entity newSelection)
    {
        // Empty to empty so no change
        if (_selectedEntity == null && newSelection == null)
            return;

        // Selecting an entity from empty
        if (_selectedEntity == null && newSelection != null)
        {
            _selectedEntity = newSelection;

            _selectedEntity.SetSelected(true);

            OnSelectionChanged?.Invoke(this, null, newSelection);
            return;
        }

        // Changing from one entity to another
        if (_selectedEntity != null && newSelection != null && _selectedEntity != newSelection)
        {
            Entity oldSelection = _selectedEntity;
            _selectedEntity = newSelection;

            oldSelection.SetSelected(false);
            newSelection.SetSelected(true);

            OnSelectionChanged?.Invoke(this, oldSelection, newSelection);
            return;
        }

        // Deselecting the current entity
        if (_selectedEntity != null && newSelection == null)
        {
            Entity oldSelection = _selectedEntity;
            _selectedEntity = null;

            oldSelection.SetSelected(false);

            OnSelectionChanged?.Invoke(this, oldSelection, null);

            return;
        }
    }

    public void ClearSelection()
    {
        if (_selectedEntity == null)
            return;

        Entity oldSelectedEntity = _selectedEntity;
        _selectedEntity = null;

        oldSelectedEntity.SetSelected(false);

        OnSelectionChanged?.Invoke(this, oldSelectedEntity, null);
    }


    public Entity GetFirstSelectedEntity(Vector2 selectionPoint)
    {
        foreach (Entity entity in _allEntities)
        {
            float entityDistanceFromSelectionPoint = Vector2.Distance(new Vector2(entity.transform.position.x, entity.transform.position.z), selectionPoint);

            if (entityDistanceFromSelectionPoint <= entity.SelectionRadius)
            {
                return entity;
            }
        }

        return null;
    }

    public List<Entity> GetEntitiesWithinRadius(Vector3 queryPosition, float radius)
    {
        List<Entity> nearbyEntities = new List<Entity>();

        foreach (Entity entity in _allEntities)
        {
            if (Vector3.Distance(queryPosition, entity.transform.position) <= radius)
                nearbyEntities.Add(entity);
        }

        return nearbyEntities;
    }

    
}