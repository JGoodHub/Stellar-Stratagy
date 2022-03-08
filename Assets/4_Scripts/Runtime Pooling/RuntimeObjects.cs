using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeObjects : MonoBehaviour {
	//Laser Projectiles
	private static List<GameObject> laserBoltSmall = new List<GameObject> ();
	public static List<GameObject> LaserBoltSmall => laserBoltSmall;

	private static List<GameObject> laserBoltLarge = new List<GameObject> ();
	public static List<GameObject> LaserBoltLarge => laserBoltLarge;

	//Missile Projectiles
	private static List<GameObject> missileSmall = new List<GameObject> ();
	public static List<GameObject> MissileSmall => missileSmall;

	private static List<GameObject> missileLarge = new List<GameObject> ();
	public static List<GameObject> MissileLarge => missileLarge;

	//Explosion Effects
	private static List<GameObject> surfaceExplosionSmall = new List<GameObject> ();
	public static List<GameObject> SurfaceExplosionSmall => surfaceExplosionSmall;

	private static List<GameObject> surfaceExplosionLarge = new List<GameObject> ();
	public static List<GameObject> SurfaceExplosionLarge => surfaceExplosionLarge;

	//Shield Effects
	private static List<GameObject> shieldHitSmall = new List<GameObject> ();
	public static List<GameObject> ShieldHitSmall => shieldHitSmall;

	private static List<GameObject> shieldHitLarge = new List<GameObject> ();
	public static List<GameObject> ShieldHitLarge => shieldHitLarge;

	public static void AddObject (GameObject usedObject, List<GameObject> targetList, bool moveObject) {
		targetList.Add (usedObject);

		if (moveObject) {
			usedObject.transform.position = new Vector3 (0, 2000, 0);
		}

		foreach (MonoBehaviour script in usedObject.GetComponents<MonoBehaviour>()) {
			script.enabled = false;
		}
	}

	public static GameObject GetObject (List<GameObject> targetList) {
		if (targetList.Count == 0) {
			return null;
		} else {
			GameObject usedObject = targetList [targetList.Count - 1];
			targetList.RemoveAt (targetList.Count - 1);

			foreach (MonoBehaviour script in usedObject.GetComponents<MonoBehaviour>()) {
				script.enabled = true;
			}

			return usedObject;
		}

	}


}
