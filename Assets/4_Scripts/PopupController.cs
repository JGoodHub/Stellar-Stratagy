using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : Singleton<PopupController>
{

	public GameObject _starMapPopup;

	public void ShowStarMapPopup()
	{
		Instantiate(_starMapPopup, transform);
	}

}
