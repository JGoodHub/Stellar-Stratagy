using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Custom Configs/New Weapon Config", order = 0)]
public class WeaponConfig : ScriptableObject
{
	[Header("Weapon Config")]
	public string Id;

	public GameObject TurretPrefab;
	public bool RotateToFaceTarget;

	public GameObject WeaponTileItem;
	public GameObject TargeterReticle;
}
