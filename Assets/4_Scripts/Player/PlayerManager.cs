using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerManager : SceneSingleton<PlayerManager>
{

	[SerializeField] private ShipController playerShip;

	public static ShipController PlayerShip => Instance.playerShip;

	public void Start()
	{
		SelectionController.Instance.OnSelectionChanged += OnSelectionChanged;
		SelectionController.Instance.OnNavPlaneQuery += OnNavPlaneQuery;
	}

	private void OnSelectionChanged(object sender, Entity oldEntity, Entity newEntity)
	{
		if (newEntity == null)
		{
			SetOrbitTarget(null);
			SetFollowTarget(null);
			return;
		}

		if (newEntity.TryGetComponent(out ShipController ship))
		{
			if (ship.isStatic)
				SetOrbitTarget(ship);
			else
				SetFollowTarget(ship);
		}
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
		playerShip.OrbitalController.SetTarget(null);
		playerShip.Helm.SetHeadingPosition(position);
		playerShip.Helm.SetDirectionRingVisible(true);
	}

	private void SetOrbitTarget(ShipController otherShip)
	{
		playerShip.OrbitalController.SetTarget(otherShip);
		playerShip.Helm.SetDirectionRingVisible(false);

	}

	private void SetFollowTarget(ShipController otherShip)
	{
		playerShip.FollowFlightController.SetTarget(otherShip);
		playerShip.Helm.SetDirectionRingVisible(false);

	}

}
