using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargeter : MonoBehaviour {

	public GameObject[] unitsSizeZero; 
	public GameObject[] unitsSizeOne;
	public GameObject[] unitsSizeThree;

	public List<GameObject>[] selectedUnits;

	public int maxUnitsPerRow;
	private int currentUnitsPerRow;

	private List<Vector3>[] moveToLocations;
	private Vector3 pivotPoint;
	private Vector3 pivotForward;

	// Use this for initialization
	void Start () {
		pivotPoint = new Vector3 (9999, 9999, 9999);

		selectedUnits = new List<GameObject>[4];
		selectedUnits [0] = unitsSizeZero.ToList ();
		selectedUnits [1] = unitsSizeOne.ToList ();
		selectedUnits [2] = new List<GameObject>();
		selectedUnits [3] = unitsSizeThree.ToList ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			RaycastHit hit;
			int layerMask = 1 << 8;
			Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, layerMask);

			pivotPoint = hit.point;

			moveToLocations = new List<Vector3>[selectedUnits.Length];
			for (int i = 0; i < moveToLocations.Length; i++) {
				moveToLocations[i] = new List<Vector3>();
			}
		} else if (Input.GetMouseButton(1)) {
			currentUnitsPerRow = maxUnitsPerRow + 99;
			for (int i = 0; i < moveToLocations.Length; i++) {
				moveToLocations [i].Clear ();
			}

			RaycastHit hit;
			int layerMask = 1 << 8;
			Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, layerMask);

			pivotForward = ((hit.point + new Vector3(0.001f, 0.001f, 0.001f)) - pivotPoint).normalized;
			Vector3 pivotLeft = new Vector3 (-pivotForward.z, pivotForward.y, pivotForward.x);

			Debug.DrawLine (pivotPoint, hit.point);

			Vector3 startingLocation;
			float columnLimit = 100000f;
			Vector3 evenOffset = Vector3.zero;
			Vector3 rowOffset = pivotForward * GetSpacingModForSize(0);


			int lastFilledRow = 0;
			for (int s = 0; s < selectedUnits.Length; s++) {
				int unitsLeftToPlace = selectedUnits [s].Count;

				if (unitsLeftToPlace > 0) {
					if (s > 0) {
						rowOffset += -pivotForward * ((GetSpacingModForSize (lastFilledRow) / 2) + (GetSpacingModForSize (s) / 2));
					} else {
						rowOffset += -pivotForward * GetSpacingModForSize (s);
					}

					while (GetSpacingModForSize (s) * (currentUnitsPerRow / 2) > columnLimit && currentUnitsPerRow > 1) {
						currentUnitsPerRow--;
					}

					startingLocation = pivotLeft * (GetSpacingModForSize(s) * (currentUnitsPerRow / 2)) + pivotPoint;
					evenOffset = -pivotLeft * (GetSpacingModForSize(s) / 2);

					//Fill full rows
					while (unitsLeftToPlace >= currentUnitsPerRow) {
						for (int i = 0; i < currentUnitsPerRow; i++) {
							Vector3 columnOffset = -pivotLeft * (GetSpacingModForSize(s) * i);
							if (currentUnitsPerRow % 2 == 0) {
								moveToLocations[s].Add(startingLocation + columnOffset + rowOffset + evenOffset);
							} else {
								moveToLocations[s].Add(startingLocation + columnOffset + rowOffset);
							}
						}

						if (unitsLeftToPlace > currentUnitsPerRow) {
							rowOffset += -pivotForward * GetSpacingModForSize(s);
						}

						unitsLeftToPlace -= currentUnitsPerRow;
					}

					//Fill overflow row
					if (unitsLeftToPlace < currentUnitsPerRow && unitsLeftToPlace > 0) {
						startingLocation = pivotLeft * (GetSpacingModForSize(s) * (unitsLeftToPlace / 2)) + pivotPoint;

						if (unitsLeftToPlace % 2 == 0) {
							for (int i = 0; i < unitsLeftToPlace; i++) {
								Vector3 columnOffset = -pivotLeft * (GetSpacingModForSize(s) * i);
								moveToLocations [s].Add (startingLocation + columnOffset + rowOffset + evenOffset);
							}
						} else {
							for (int i = 0; i < unitsLeftToPlace; i++) {
								Vector3 columnOffset = -pivotLeft * (GetSpacingModForSize(s) * i);
								moveToLocations [s].Add (startingLocation + columnOffset + rowOffset);
							}
						}

						unitsLeftToPlace = 0;
					}

					lastFilledRow = s;
				}
			}
			Debug.DrawRay (pivotPoint, rowOffset);
		} else if (Input.GetMouseButtonUp(1)) {
			Quaternion formationDirection = Quaternion.LookRotation (pivotForward);
			for (int s = 0; s < moveToLocations.Length; s++) {
				for (int i = 0; i < moveToLocations [s].Count; i++) {
					MovementController unitControl = selectedUnits [s] [i].GetComponent<MovementController> ();
					unitControl.AddMoveToPosition (moveToLocations [s] [i], false);
					unitControl.SetEndRotation (formationDirection);
				}
			}

			pivotPoint = new Vector3 (9999, 9999, 9999);
		}
	}

	private float GetSpacingModForSize (int size) {
		return (26f * size) + 31f;
	}
		
	void OnDrawGizmos () {
		if (Application.isPlaying) {
			Gizmos.color = Color.red;
			if (pivotPoint.x != 9999) {
				for (int s = 0; s < selectedUnits.Length; s++) {
					foreach (Vector3 pos in moveToLocations[s]) {
						if (pos != Vector3.zero + new Vector3(0.001f, 0.001f, 0001f)) {
							Gizmos.color = Color.red;
							Gizmos.DrawWireSphere (pos, (GetSpacingModForSize(s) - 5f) / 2);
							Gizmos.color = Color.green;
							Gizmos.DrawWireSphere (pos, (GetSpacingModForSize(s) / 2));
						}

					}

				}

			}

		}


	}

}


/*

*/
