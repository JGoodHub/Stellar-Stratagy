using System.Collections;
using UnityEngine;

public class LaserWeaponConfig : WeaponConfig
{
	[Header("Laser Weapon Config")]
	public GameObject LaserPrefab;
	public int ShotsPerBurst = 3;
	public int BurstsPerAction = 3;
	public float FiringTimeStart = 0.166f;
	public float FiringTimeEnd = 0.833f;
	public float LaserSpeed = 60;
	public float ArcSpread = 5f;

	public override void Fire(Entity sourceEntity, Entity targetEntity, Vector3 targetPosition)
	{
		sourceEntity.StartCoroutine(FireLasersAtTargetCoroutine(sourceEntity, targetEntity, targetPosition));
	}

	private IEnumerator FireLasersAtTargetCoroutine(Entity sourceEntity, Entity targetEntity, Vector3 targetPosition)
	{
		if (FiringTimeStart > 0)
			yield return new WaitForSeconds(FiringTimeStart * TurnController.TURN_DURATION);

		float firingDuration = (FiringTimeEnd - FiringTimeStart) * TurnController.TURN_DURATION;
		float burstDuration = firingDuration / (BurstsPerAction * 2 - 1);
		float fireInterval = burstDuration / ShotsPerBurst;
		
		WaitForSeconds burstDurationYield = new WaitForSeconds(burstDuration);
		WaitForSeconds fireIntervalYield = new WaitForSeconds(fireInterval);

		for (int burstCount = 0; burstCount < BurstsPerAction; burstCount++)
		{
			for (int shotCount = 0; shotCount < ShotsPerBurst; shotCount++)
			{
				GameObject laserObject = Instantiate(LaserPrefab, sourceEntity.transform.position, Quaternion.identity);
				CombatProjectileLaserBolt combatProjectileLaserBolt = laserObject.GetComponent<CombatProjectileLaserBolt>();

				Vector3 origin = sourceEntity.transform.position;
				Vector3 direction = (targetEntity == null ? targetPosition : targetEntity.transform.position) - origin;

				float arcTheta = Random.Range(-ArcSpread, ArcSpread);
				direction = Quaternion.Euler(0f, arcTheta, 0f) * direction;

				combatProjectileLaserBolt.Initialise(origin, direction, LaserSpeed, sourceEntity.id);

				yield return fireIntervalYield;
			}

			if (burstCount < BurstsPerAction - 1)
				yield return burstDurationYield;
		}
	}

}
