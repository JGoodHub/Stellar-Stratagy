using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardpointID : MonoBehaviour {

	public HardpointType hardpointType;
	public enum HardpointType {
		laserTurret, missileTurret
	}

	public HardpointSize hardpointSize;
	public enum HardpointSize {
		small, large
	}


}
