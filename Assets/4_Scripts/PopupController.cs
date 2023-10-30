using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime;
using UnityEngine;

namespace GoodHub.Core.Runtime.PopupSystem
{

	public class PopupController : GlobalSingleton<PopupController>
	{

		public GameObject _starMapPopup;

		public void ShowStarMapPopup()
		{
			Instantiate(_starMapPopup, transform);
		}

	}

}
