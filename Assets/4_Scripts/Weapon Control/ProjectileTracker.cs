using System.Collections.Generic;
using UnityEngine;

public class ProjectileTracker
{

	private static HashSet<LaserBolt> _laserBolts = new HashSet<LaserBolt>();

	public static void RegisterLaserBolt(LaserBolt laserBolt)
	{
		if (_laserBolts.Contains(laserBolt))
			return;

		_laserBolts.Add(laserBolt);
	}

	public static void UnregisterLaserBolt(LaserBolt laserBolt)
	{
		if (_laserBolts.Contains(laserBolt) == false)
			return;

		_laserBolts.Remove(laserBolt);
	}

	public static void FreezeAllProjectiles()
	{
		foreach (LaserBolt bolt in _laserBolts)
		{
			bolt._frozen = true;
		}

	}

	public static void UnfreezeAllProjectiles()
	{
		foreach (LaserBolt bolt in _laserBolts)
		{
			bolt._frozen = false;
		}
	}

}
