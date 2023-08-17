using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

	private static HashSet<IFreezeable> _projectiles = new HashSet<IFreezeable>();

	private void Start()
	{
		TurnController.OnRealtimeStarted += UnfreezeAllProjectiles;
		TurnController.OnRealtimeEnded += FreezeAllProjectiles;
	}

	public static void RegisterProjectile(IFreezeable projectile)
	{
		if (_projectiles.Contains(projectile))
			return;

		_projectiles.Add(projectile);
	}

	public static void UnregisterProjectile(IFreezeable projectile)
	{
		if (_projectiles.Contains(projectile) == false)
			return;

		_projectiles.Remove(projectile);
	}

	public static void FreezeAllProjectiles()
	{
		foreach (IFreezeable projectile in _projectiles)
		{
			projectile.Freeze();
		}
	}

	public static void UnfreezeAllProjectiles()
	{
		foreach (IFreezeable projectile in _projectiles)
		{
			projectile.Unfreeze();
		}
	}

}
