using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Custom Configs/New Weapon Config", order = 0)]
public class WeaponConfig : ScriptableObject
{
	[Header("Base Weapon Config")]
	public string Id;

	public GameObject TurretPrefab;
	public bool RotateToFaceTarget;

	public GameObject WeaponCardPrefab;
	public GameObject TargetReticlePrefab;

	public virtual void Fire(Entity sourceEntity, Entity targetEntity, Vector3 targetPosition)
	{ }

	public WeaponConfig CreateModifiableInstance()
	{
		return (WeaponConfig) MemberwiseClone();
	}

}
