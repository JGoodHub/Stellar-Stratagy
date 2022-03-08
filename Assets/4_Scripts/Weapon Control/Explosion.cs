using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	private void Start () {
		Init ();
	}

	public void Init() {
		foreach (ParticleSystem partSys in GetComponentsInChildren<ParticleSystem>()) {
			partSys.Play ();
		}
	}

}
