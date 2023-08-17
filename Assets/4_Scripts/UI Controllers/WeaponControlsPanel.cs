using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControlsPanel : MonoBehaviour
{
    [SerializeField] private Button _expandButton;
    [SerializeField] private Button _collapseButton;
    [SerializeField] private Vector2 _collapsedSize;
    [SerializeField] private Vector2 _expandedSize;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private RectTransform _expandablePanel;
    [SerializeField] private GameObject _collapsedContent;
    [SerializeField] private GameObject _expandedContent;

    private bool _isExpanded;

    private CanvasGroup _collapseCanvasGroup;
    private CanvasGroup _expandCanvasGroup;

    private void Start()
    {
        if (_expandButton != null)
        {
            _expandButton.onClick.AddListener(() =>
            {
                if (_isExpanded)
                    Collapse();
                else
                    Expand();
            });
        }
    }

    private void Expand()
    {
        TweenerCore<Vector2, Vector2, VectorOptions> tweener = _expandablePanel.DOSizeDelta(_expandedSize, _transitionDuration);
        tweener.SetEase(Ease.InOutQuart);
        tweener.
    }

    private void Collapse()
    {
        _expandablePanel.DOSizeDelta(_collapsedSize, _transitionDuration).SetEase(Ease.InOutQuart);
    }
}