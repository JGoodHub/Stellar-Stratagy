using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
	private Animation _animation;

	[SerializeField] private Button _closeBtn;

	private void Awake()
	{
		_animation = GetComponentInChildren<Animation>();

		_closeBtn.onClick.AddListener(Close);
	}

	protected virtual void Start()
	{
		_animation.Play("PopupAppear");
	}

	public void Close()
	{
		_animation.Play("PopupDisappear");

		Destroy(gameObject, 1f);
	}

}
