using UnityEngine;

public class ProjectileWeaponConfig : WeaponConfig
{
	public int TurnCooldown;
	public GameObject ProjectilePrefab;
	public GameObject ImpactExplosion;

	public float ArcSpread;
	public int ProjectileCount;
	public float FireInterval;
	public float StartDelay;
	public float FiringDuration;
}
