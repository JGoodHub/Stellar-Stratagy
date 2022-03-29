using System;
using DG.Tweening;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

	[SerializeField] private LineRenderer beamRenderer;

	private Transform source;
	private Transform target;

	public void Initialise(Transform sourceTransform)
	{
		source = sourceTransform;

		SetBeamVisible(false);
	}

	public void SetBeamVisible(bool isVisible)
	{
		beamRenderer.enabled = isVisible;
	}

	public void SetBeamStart(Transform start)
	{
		source = start;
	}

	public void SetBeamEnd(Transform end)
	{
		target = end;
	}

	private void Update()
	{
		if (source != null)
			beamRenderer.SetPosition(0, source.position);

		if (target != null)
			beamRenderer.SetPosition(1, target.position);
	}

	public void HideAfterSeconds(float seconds)
	{
		DOVirtual.DelayedCall(seconds, () => SetBeamVisible(false), false);
	}
}
