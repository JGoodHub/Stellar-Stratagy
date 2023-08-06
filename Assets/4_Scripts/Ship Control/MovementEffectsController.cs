using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEffectsController : SceneSingleton<MovementEffectsController>
{
    [SerializeField] private LineRenderer _moveableAreaLineRenderer;

    public void ShowMoveableArea(List<Vector3> outlinePositions)
    {
        _moveableAreaLineRenderer.enabled = true;

        _moveableAreaLineRenderer.positionCount = outlinePositions.Count;
        _moveableAreaLineRenderer.SetPositions(outlinePositions.ToArray());
    }

    public void ClearMoveableArea()
    {
        _moveableAreaLineRenderer.enabled = false;
    }
}