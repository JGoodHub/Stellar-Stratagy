using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerManager : SceneSingleton<PlayerManager>
{

	public ShipController playerShip;

	public void Start()
	{
		SelectionController.Instance.OnSelectionChanged += OnSelectionChanged;
		SelectionController.Instance.OnNavPlaneQuery += OnNavPlaneQuery;
	}

	private void OnSelectionChanged(object sender, Entity oldEntity, Entity newEntity)
	{
		if (newEntity == null)
			return;

		if (newEntity.TryGetComponent(out ShipController ship))
			SetShipOrbitTarget(ship);
	}

	private void OnNavPlaneQuery(object sender, Vector2 navPlanePoint)
	{
		if (SelectionController.SelectedEntity == null)
		{
			SetShipHeading(new Vector3(navPlanePoint.x, 0f, navPlanePoint.y));
		}
	}

	public void SetShipHeading(Vector3 position)
	{
		playerShip.OrbitalController.SetOrbitTarget(null);
		playerShip.Helm.SetHeading(position);
		playerShip.Helm.SetDirectionRingVisible(true);
	}

	private void SetShipOrbitTarget(ShipController otherShip)
	{
		playerShip.OrbitalController.SetOrbitTarget(otherShip.OrbitalController);
		playerShip.Helm.SetDirectionRingVisible(false);

	}

}
