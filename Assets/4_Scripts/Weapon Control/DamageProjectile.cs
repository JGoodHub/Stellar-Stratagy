// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class DamageProjectile : MonoBehaviour {
// 	
// 	private float damage;
// 	public float Damage {
// 		set => damage = value;
// 	}
//
// 	private bool dead;
// 	private float currentLifetime;
// 	private float maxLifetime;
// 	public float MaxLifetime {
// 		set => maxLifetime = value;
// 	}
//
// 	private bool isFriendly;
// 	public bool IsFriendly {
// 		set => isFriendly = value;
// 	}
//
// 	private HardpointID.HardpointSize projectileSize;
// 	public HardpointID.HardpointSize ProjectileSize {
// 		set => projectileSize = value;
// 	}
//
// 	private HardpointID.HardpointType projectileType;
// 	public HardpointID.HardpointType ProjectileType {
// 		set => projectileType = value;
// 	} 
//
// 	public bool showDebugInfo;
//
// 	public void Init () {
// 		currentLifetime = maxLifetime;
// 		dead = false;
// 	}
//
// 	private void Update () {
// 		currentLifetime -= Time.deltaTime;
//
// 		if (currentLifetime <= 0f) {
// 			StoreProjectile ();
// 		}
//
// 	}
//
// 	private void OnTriggerEnter (Collider other) {
// 		if (other.gameObject.layer == 10 && (other.tag == "Enemy Shield" || other.tag == "Enemy Hull") && !dead) {
// 			RaycastHit hit;
// 			int layerMask = 1 << 10;
// 			Ray ray = new Ray (transform.position - transform.forward, transform.forward);
// 			Physics.Raycast (ray, out hit, 4f, layerMask);
//
// 			GameObject effectClone = null;
// 			if (other.tag == "Enemy Shield") {				
// 				switch (projectileSize) {
// 				case HardpointID.HardpointSize.small:
// 					effectClone = GetEffect (RuntimeObjects.ShieldHitSmall, "Shield Effects/Shield Hit Small");
// 					StartCoroutine(StoreObjectAfter (effectClone, RuntimeObjects.ShieldHitSmall, false, 0.8f));
// 					break;
// 				case HardpointID.HardpointSize.large:
// 					effectClone = GetEffect (RuntimeObjects.ShieldHitLarge, "Shield Effects/Shield Hit Large");
// 					StartCoroutine(StoreObjectAfter (effectClone, RuntimeObjects.ShieldHitLarge, false, 0.8f));
// 					break;
// 				}
//
// 				//effectClone.GetComponent<ShieldHit> ().Init ();
// 				effectClone.transform.parent = GameObject.Find ("Runtime Objects/Shield Hits").transform;
// 			} else if (other.tag == "Enemy Hull") {	
// 				
// 				switch (projectileSize) {
// 				case HardpointID.HardpointSize.small:
// 					effectClone = GetEffect (RuntimeObjects.SurfaceExplosionSmall, "Explosions/Surface Explosion Small");
// 					StartCoroutine(StoreObjectAfter (effectClone, RuntimeObjects.SurfaceExplosionSmall, false, 2f));
// 					break;
// 				case HardpointID.HardpointSize.large:
// 					effectClone = GetEffect (RuntimeObjects.SurfaceExplosionSmall, "Explosions/Surface Explosion Large");
// 					StartCoroutine(StoreObjectAfter (effectClone, RuntimeObjects.SurfaceExplosionLarge, false, 2f));
// 					break;
// 				}
//
// 				effectClone.GetComponent<Explosion> ().Init ();
// 				effectClone.transform.parent = GameObject.Find ("Runtime Objects/Explosions").transform;
// 			}
//
//
// 			effectClone.transform.position = hit.point;
// 			effectClone.transform.rotation = Quaternion.LookRotation (hit.normal);
//
// 			StoreProjectile ();
//
// 			bool rootFound = false;
// 			Transform rootTransform = hit.collider.transform;
// 			while (rootFound == false) {
// 				if (rootTransform.tag == "Enemy Root") {
// 					rootFound = true;
// 				} else {
// 					rootTransform = rootTransform.parent;
// 				}
// 			}
//
// 			//rootTransform.GetComponent<ShipData> ().TakeDamage (damage);
//
//
// 			if (showDebugInfo) {
// 				Debug.DrawRay (ray.origin, ray.direction * 4f, Color.blue, 1f);
// 				Debug.DrawRay (hit.point, hit.normal, Color.green, 1f);
// 			}
//
// 		}
// 	}
//
// 	private void StoreProjectile () {
// 		dead = true;
// 		switch (projectileType) {
// 		case HardpointID.HardpointType.laserTurret:
// 			switch (projectileSize) {
// 			case HardpointID.HardpointSize.small:
// 				RuntimeObjects.AddObject (gameObject, RuntimeObjects.LaserBoltSmall, true);
// 				break;
// 			case HardpointID.HardpointSize.large:
// 				RuntimeObjects.AddObject (gameObject, RuntimeObjects.LaserBoltLarge, true);
// 				break;
// 			}
// 			break;
// 		case HardpointID.HardpointType.missileTurret:
// 			//GetComponent<MissileProjectile> ().SmokeTrail.Stop();
//
// 			switch (projectileSize) {
// 			case HardpointID.HardpointSize.small:					
// 				RuntimeObjects.AddObject (gameObject, RuntimeObjects.MissileSmall, true);
// 				break;
// 			case HardpointID.HardpointSize.large:
// 				RuntimeObjects.AddObject (gameObject, RuntimeObjects.MissileLarge, true);
// 				break;
// 			}
// 			break;
// 		}
//
// 	}
//
//
// 	private GameObject GetEffect (List<GameObject> targetList, string targetPath) {
// 		GameObject effectClone;
//
// 		effectClone = RuntimeObjects.GetObject (targetList);
// 		if (effectClone == null) {
// 			effectClone = (GameObject) Instantiate ((GameObject) Resources.Load (targetPath));
// 		}
//
// 		return effectClone;
// 	}
//
// 	private IEnumerator StoreObjectAfter (GameObject objectToStore, List<GameObject> destination, bool move, float delay) {
// 		yield return new WaitForSeconds (delay);
// 		RuntimeObjects.AddObject (objectToStore, destination, move);
// 	}
//
// 	private void OnDrawGizmos () {
// 		if (showDebugInfo) {
// 			Gizmos.DrawSphere (transform.position - (transform.forward * 0.8f), 0.05f);
// 		}
//
// 	}
//
//
//
// }
