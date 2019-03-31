using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour {

	public float speed;
	private Transform target;
	public Transform Target {
		set { target = value; }
	}

	[SerializeField] private ParticleSystem smokeTrail;
	public ParticleSystem SmokeTrail {
		get { return smokeTrail; }
	}

	private bool missed = false;

	void Start () {
		Init ();
	}

	public void Init () {
		smokeTrail.Play ();
		transform.LookAt (target.position);
	}

	void FixedUpdate () {
		if (missed == false && target != null && Vector3.Distance(transform.position, target.position) < 4f) {
			missed = true;
		}

		if (!missed && target != null) {
			transform.LookAt (target.position);
		}


		transform.Translate (Vector3.forward * speed * Time.deltaTime);

	}


}
