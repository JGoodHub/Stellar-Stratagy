using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPopup : Popup
{

	public Button _earthButton;
	public Button _marsButton;

	protected override void Start()
	{
		base.Start();

		_marsButton.onClick.AddListener(MarsSelected);
	}

	private void MarsSelected()
	{
		// NavigationWaypoint earth = NavigationController.Instance.GetWaypointByName("EarthStation");
		// NavigationWaypoint mars = NavigationController.Instance.GetWaypointByName("MarsStation");
		//
		// NavigationPath path = NavigationController.Instance.GetPathBetweenWaypoints(earth, mars);
		// PlayerCombatController.PlayerShip.Helm.FollowNavigationPath(path);

		Close();
	}

}
