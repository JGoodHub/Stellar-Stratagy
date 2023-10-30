using System;
using System.Diagnostics.Tracing;
using DG.Tweening;
using GoodHub.Core.Runtime;
using UnityEngine;

public class TurnController : SceneSingleton<TurnController>
{
    [SerializeField] private float _turnRealtimeDuration = 12f;

    public float TurnRealtimeDuration => _turnRealtimeDuration;

    public static event Action OnRealtimeStarted;
    
    public static event Action OnRealtimeEnded;

    public static event Action OnPlayersTurnStarted;
    
    public static event Action OnPlayersTurnEnded;

    public static event Action OnEnemyTurnStarted;

    public static event Action OnEnemyTurnEnded;

    private void Start()
    {
        OnPlayersTurnStarted?.Invoke();
    }

    public void PlayerActionsSubmitted()
    {
        OnPlayersTurnEnded?.Invoke();

        OnEnemyTurnStarted?.Invoke();
    }

    public void EnemyActionSubmitted()
    {
        OnEnemyTurnEnded?.Invoke();

        StartRealtime();
    }

    private void StartRealtime()
    {
        OnRealtimeStarted?.Invoke();

        DOVirtual.DelayedCall(_turnRealtimeDuration, () =>
        {
            OnRealtimeEnded?.Invoke();
            OnPlayersTurnStarted?.Invoke();
        }, false);
    }
}