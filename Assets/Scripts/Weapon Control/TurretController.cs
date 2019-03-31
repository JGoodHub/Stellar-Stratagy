using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

	private Transform target;
	public Transform Target {
		set { target = value; }
	}

	public float damage;

	public Transform yawAxis;
	private Quaternion resetYaw;

	public Transform pitchAxis;
	private Quaternion resetPitch;

	public float aimResetDelay;
	private float delayCountdown;

	public float rateOfFire;
	private float fireCountdown;
	public Transform[] barrelEnd;
	private int barrelIndex;

	public HardpointID.HardpointType hardpointType;
	public HardpointID.HardpointSize hardpointSize;

	private bool isFriendly;
	public bool IsFriendly {
		set { isFriendly = value; }
	}

	public bool showTargeting;

	public void Init () {
		resetYaw = yawAxis.localRotation;
		resetPitch = pitchAxis.localRotation;

		if (!isFriendly) {
			foreach (Transform trans in GetComponentsInChildren<Transform>()) {
				trans.gameObject.layer = 10;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		delayCountdown -= Time.deltaTime;
		fireCountdown -= Time.deltaTime;

		if (target != null) {
			float distanceToPlane = Vector3.Dot (transform.up, target.position - yawAxis.position);
			Vector3 plantPoint = target.position - yawAxis.up * distanceToPlane;

			if (Vector3.Angle (transform.up, (target.position - plantPoint).normalized) < 1f) {			
				yawAxis.LookAt (plantPoint, transform.up);
				pitchAxis.LookAt (target.position, transform.up);

				if (fireCountdown <= 0) {
					Fire ();
				}

				delayCountdown = aimResetDelay;
			}

			if (showTargeting) {
				Debug.DrawLine (yawAxis.position, plantPoint);
				Debug.DrawLine (plantPoint, target.position);

				Debug.DrawRay (pitchAxis.position, pitchAxis.forward * 50f);
			}
		}


		if (delayCountdown <= 0f) {
			yawAxis.localRotation = resetYaw;
			pitchAxis.localRotation = resetPitch;
			fireCountdown = rateOfFire;
		}


	}


	private void Fire () {
		GameObject projectileClone = null;
		DamageProjectile damageProjectileClone = null;

		switch (hardpointType) {
		case HardpointID.HardpointType.laserTurret:
			switch (hardpointSize) {
			case HardpointID.HardpointSize.small:
				projectileClone = GetProjectile (RuntimeObjects.LaserBoltSmall, "Projectiles/Laser Bolt Small");
				projectileClone.transform.parent = GameObject.Find ("Runtime Objects/Projectiles/Laser Small").transform;
				break;
			case HardpointID.HardpointSize.large:
				projectileClone = GetProjectile (RuntimeObjects.LaserBoltLarge, "Projectiles/Laser Bolt Large");
				projectileClone.transform.parent = GameObject.Find ("Runtime Objects/Projectiles/Laser Large").transform;
				break;
			}
			break;
		case HardpointID.HardpointType.missileTurret:
			switch (hardpointSize) {
			case HardpointID.HardpointSize.small:
				projectileClone = GetProjectile (RuntimeObjects.MissileSmall, "Projectiles/Missile Small");
				projectileClone.transform.parent = GameObject.Find ("Runtime Objects/Projectiles/Missile Small").transform;
				break;
			case HardpointID.HardpointSize.large:
				projectileClone = GetProjectile (RuntimeObjects.MissileLarge, "Projectiles/Missile Large");
				projectileClone.transform.parent = GameObject.Find ("Runtime Objects/Projectiles/Missile Large").transform;
				break;
			}

			MissileProjectile missileProjecileClone = projectileClone.GetComponent<MissileProjectile> ();
			missileProjecileClone.Target = target;
			missileProjecileClone.Init ();

			break;
		}

		projectileClone.transform.position = barrelEnd [barrelIndex].position;
		projectileClone.transform.rotation = barrelEnd [barrelIndex].rotation;

		damageProjectileClone = projectileClone.GetComponent<DamageProjectile> ();
		damageProjectileClone.ProjectileSize = hardpointSize;
		damageProjectileClone.ProjectileType = hardpointType;
		damageProjectileClone.Damage = damage;
		damageProjectileClone.IsFriendly = isFriendly;
		damageProjectileClone.MaxLifetime = 10f;
		damageProjectileClone.Init ();

		barrelIndex = (barrelIndex + 1) % barrelEnd.Length;
		fireCountdown = rateOfFire;

	}

	private GameObject GetProjectile (List<GameObject> targetList, string targetPath) {
		GameObject projectileClone;

		projectileClone = RuntimeObjects.GetObject (targetList);
		if (projectileClone == null) {
			projectileClone = (GameObject) Instantiate ((GameObject) Resources.Load (targetPath));
		}

		return projectileClone;
	}

	void OnDrawGizmos () {
		if (showTargeting) {
			Gizmos.DrawSphere (target.position, 0.33f);
		}

	}


}
