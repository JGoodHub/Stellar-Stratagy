using System;
using System.Diagnostics.Tracing;
using DG.Tweening;
using UnityEngine;

public class TurnController : SceneSingleton<TurnController>
{

	public const float TURN_DURATION = 6f;

	public static event Action OnRealtimeStarted;
	public static event Action OnRealtimeEnded;

	public static event Action OnPlayersTurnStarted;
	public static event Action OnPlayersTurnEnded;

	private void Start()
	{
		OnPlayersTurnStarted?.Invoke();
	}

	public void PlayerActionsSubmitted()
	{
		OnPlayersTurnEnded?.Invoke();
		
		OnRealtimeStarted?.Invoke();
		
		DOVirtual.DelayedCall(6f, () =>
		{
			OnRealtimeEnded?.Invoke();
			OnPlayersTurnStarted?.Invoke();
		}, false);
	}

}
