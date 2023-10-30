using System;
using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime;
using UnityEngine;

public class TurnEffectsController : SceneSingleton<TurnEffectsController>
{
    [Header("Movement")]
    [SerializeField] private LineRenderer _moveableAreaRenderer;
    [SerializeField] private LineRenderer _pathLineRenderer;

    [Header("Targeting")]
    [SerializeField] private Transform _reticlesContainer;

    private void Start()
    {
        _moveableAreaRenderer.enabled = false;
        _pathLineRenderer.enabled = false;
    }

    public void ShowMoveableArea(List<Vector3> outlinePositions)
    {
        _moveableAreaRenderer.enabled = true;

        _moveableAreaRenderer.positionCount = outlinePositions.Count;
        _moveableAreaRenderer.SetPositions(outlinePositions.ToArray());
    }

    public void ShowFlightTrajectory(List<Vector3> pathPositions)
    {
        _pathLineRenderer.enabled = true;

        _pathLineRenderer.positionCount = pathPositions.Count;
        _pathLineRenderer.SetPositions(pathPositions.ToArray());
    }

    public void ClearMoveableArea()
    {
        _moveableAreaRenderer.enabled = false;
    }

    public void ClearPathLine()
    {
        _pathLineRenderer.enabled = false;
    }
}