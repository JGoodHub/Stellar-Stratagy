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
    [SerializeField] private Button _toggleButton;
    [SerializeField] private Vector2 _collapsedSize;
    [SerializeField] private Vector2 _expandedSize;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private RectTransform _expandablePanel;
    [SerializeField] private GameObject _collapsedContent;
    [SerializeField] private GameObject _expandedContent;
    [SerializeField] private CanvasGroup _collapseCanvasGroup;
    [SerializeField] private CanvasGroup _expandCanvasGroup;
    private bool _isExpanded;
    private bool _isTweening;


    private void Start()
    {
        if (_expandButton != null)
            _expandButton.onClick.AddListener(Expand);

        if (_collapseButton != null)
            _collapseButton.onClick.AddListener(Collapse);

        if (_toggleButton != null)
            _toggleButton.onClick.AddListener(Toggle);
    }

    private void Toggle()
    {
        if (_isExpanded)
            Collapse();
        else
            Expand();
    }

    private void Expand()
    {
        if (_isTweening || _isExpanded)
            return;

        _isTweening = true;

        _expandablePanel
            .DOSizeDelta(_expandedSize, _transitionDuration)
            .SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                _isExpanded = true;
                _isTweening = false;
                
                
            });

        SetToExpandedState();
    }

    private void Collapse()
    {
        if (_isTweening || _isExpanded == false)
            return;

        _isTweening = true;

        _expandablePanel
            .DOSizeDelta(_collapsedSize, _transitionDuration)
            .SetEase(Ease.InOutQuart)
            .OnComplete(() =>
            {
                _isExpanded = false;
                _isTweening = false;
            });

        SetToCollapsedState();
    }

    private void SetToExpandedState(bool snap = false)
    {
        if (snap)
            _expandablePanel.sizeDelta = _expandedSize;

        if (_expandButton)
            _expandButton.gameObject.SetActive(false);

        if (_collapseButton)
            _collapseButton.gameObject.SetActive(true);

        _collapsedContent.SetActive(false);
        _expandedContent.SetActive(true);
    }


    private void SetToCollapsedState(bool snap = false)
    {
        if (snap)
            _expandablePanel.sizeDelta = _collapsedSize;

        if (_expandButton)
            _expandButton.gameObject.SetActive(true);

        if (_collapseButton)
            _collapseButton.gameObject.SetActive(false);

        _collapsedContent.SetActive(true);
        _expandedContent.SetActive(false);
    }

    [ContextMenu("Set To Expanded State")]
    private void SetToExpandedStateMenuItem() => SetToExpandedState(true);

    [ContextMenu("Set To Collapsed State")]
    private void SetToCollapsedStateMenuItem() => SetToCollapsedState(true);
}