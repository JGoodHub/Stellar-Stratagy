using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeedController : MonoBehaviour {

	[Range(0.05f, 1f)] public float speed;
	
	void Update () {
		Time.timeScale = speed;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;		
	}
}
