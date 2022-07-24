using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EngineTrails : MonoBehaviour
{

	private TrailRenderer _trail;

	private Vector3[] _cachedPositions = new Vector3[0];

	private bool _holdTrail;

	private float _defaultTrailTime;
	
	private void Start()
	{
		_trail = GetComponent<TrailRenderer>();
		_defaultTrailTime = _trail.time;

		TurnController.OnRealtimeStarted += ReleaseTrail;
		TurnController.OnRealtimeEnded += HoldTrail;
	}

	private void Update()
	{
		if (_holdTrail)
		{
			_trail.time += Time.deltaTime;
		}	
	}

	private void HoldTrail()
	{
		_holdTrail = true;
		
		_cachedPositions = new Vector3[_trail.positionCount];
		_trail.GetPositions(_cachedPositions);
	}

	private void ReleaseTrail()
	{
		_holdTrail = false;

		_trail.time = _defaultTrailTime;
		_trail.SetPositions(_cachedPositions);
	}


}
