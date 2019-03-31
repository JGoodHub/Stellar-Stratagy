using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

	private List<CollisionAvoidance> colAvoidList = new List<CollisionAvoidance>();
	public List<CollisionAvoidance> ColAvoidList {
		get{ return colAvoidList; }
	}

	public CollisionAvoidance IsPositionOccupied (Vector3 position, float radius) {
		int i = 0;
		while (i < colAvoidList.Count) {
			if (colAvoidList [i].FinalTargetPosition != Vector3.zero) {
				float distance = Vector3.Distance (position, colAvoidList[i].FinalTargetPosition);
				if (distance < radius * 2f && distance > 0.05f) {
					return colAvoidList [i];
				} else {
					i++;
				}
			} else {
				i++;
			}
		}

		return null;
		
	}


}
