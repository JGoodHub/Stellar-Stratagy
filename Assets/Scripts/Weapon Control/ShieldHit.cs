using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHit : MonoBehaviour {

	private Animator animControl;

	public void Init () {
		animControl = GetComponent<Animator> ();
		animControl.SetTrigger ("Reset");
	}



}
