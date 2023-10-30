using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TurnControls : MonoBehaviour
{
    [SerializeField] private Image _turnProgressFill;
    [SerializeField] private Button _endTurnButton;

    private void Awake()
    {
        _endTurnButton.onClick.AddListener(EndTurnClicked);

        TurnController.OnPlayersTurnStarted += () =>
        {
            _turnProgressFill.fillAmount = 0;
            _endTurnButton.interactable = true;
        };

        TurnController.OnRealtimeStarted += () =>
        {
            _turnProgressFill.fillAmount = 0;
            _turnProgressFill.DOFillAmount(1f, TurnController.Singleton.TurnRealtimeDuration);
        };
    }

    private void EndTurnClicked()
    {
        TurnController.Singleton.PlayerActionsSubmitted();

        _endTurnButton.interactable = false;
    }
}