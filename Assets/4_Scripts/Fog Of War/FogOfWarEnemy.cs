using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarEnemy : MonoBehaviour {

	private Material[] baseMaterials;
	private Material[] fadeMaterials;
	private float[] startingAlphas;

	private Transform[] shipTransforms;

	private bool isVisible = true;
	public bool IsVisible {
		get { return isVisible; }
	}

	private float hideDelay = 0.1f;
	private float hideCountdown;

	void Start () {
		hideCountdown = hideDelay;

		shipTransforms = GetComponentsInChildren<Transform> ();

		Hide ();

	}


	void Update () {
		hideCountdown -= Time.deltaTime;
		if (hideCountdown < 0 && isVisible == true) {
			Hide ();
		}

	}

	public void ResetHideCountdown () {
		hideCountdown = hideDelay;
	}


	public void Hide () {
		foreach (Transform trans in shipTransforms) {
			if (trans.gameObject.layer == 10) {
				trans.gameObject.layer = 11;
			}
		}

		isVisible = false;
	}


	public void Reveal () {
		foreach (Transform trans in shipTransforms) {
			if (trans.gameObject.layer == 11) {
				trans.gameObject.layer = 10;
			}
		}

		isVisible = true;
	}


	public void HideAnimated () {	
		SetMaterialsToFade ();
		StartCoroutine (FadeOutAnimation ());

		isVisible = false;

	}

	private void SetMaterialsToFade () {
		MeshRenderer[] objectMeshRens = GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer meshRen in objectMeshRens) {
			Material[] objectMats = meshRen.materials;

			for (int m = 0; m < objectMats.Length; m++) {
				bool matchingMatFound = false;
				int i = 0;
				while (matchingMatFound == false && i < baseMaterials.Length) {
					if (RGBCompare(objectMats[m].color, baseMaterials[i].color)) {
						objectMats [m] = fadeMaterials [i];
						matchingMatFound = true;
					} else {
						i++;
					}

				}

			}

			meshRen.materials = objectMats;

		}

	}

	private IEnumerator FadeOutAnimation () {
		float alphaPercentage = 1f;
		MeshRenderer[] objectMeshRens = GetComponentsInChildren<MeshRenderer> ();

		yield return new WaitForFixedUpdate();
		while (objectMeshRens[0].material.color.a > 0) {
			alphaPercentage -= Time.fixedDeltaTime;
			foreach (MeshRenderer meshRen in objectMeshRens) {
				Material[] objectMats = meshRen.materials;

				for (int m = 0; m < objectMats.Length; m++) {
					bool matchingMatFound = false;
					int i = 0;
					while (matchingMatFound == false && i < baseMaterials.Length) {
						if (RGBCompare (objectMats [m].color, baseMaterials [i].color)) {							
							objectMats [m].color = new Color (objectMats [m].color.r, objectMats [m].color.g, objectMats [m].color.b, startingAlphas[i] * alphaPercentage);
							matchingMatFound = true;
						} else {
							i++;
						}

					}

				}

				meshRen.materials = objectMats;

			}

			yield return new WaitForFixedUpdate ();
		}


	}


	public void RevealAnimated () {		
		StartCoroutine (FadeInAnimation ());

		isVisible = false;

	}

	private void SetMaterialsToOpaque () {
		MeshRenderer[] objectMeshRens = GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer meshRen in objectMeshRens) {
			Material[] objectMats = meshRen.materials;

			for (int m = 0; m < objectMats.Length; m++) {
				bool matchingMatFound = false;
				int i = 0;
				while (matchingMatFound == false && i < baseMaterials.Length) {
					if (RGBCompare(objectMats[m].color, baseMaterials[i].color)) {
						objectMats [m] = baseMaterials [i];
						matchingMatFound = true;
					} else {
						i++;
					}

				}

			}

			meshRen.materials = objectMats;

		}

	}

	private IEnumerator FadeInAnimation () {
		float alphaPercentage = 0f;
		MeshRenderer[] objectMeshRens = GetComponentsInChildren<MeshRenderer> ();

		yield return new WaitForFixedUpdate();
		while (objectMeshRens [0].material.color.a < 1f) {
			alphaPercentage += Time.fixedDeltaTime;
			foreach (MeshRenderer meshRen in objectMeshRens) {
				Material[] objectMats = meshRen.materials;

				for (int m = 0; m < objectMats.Length; m++) {
					bool matchingMatFound = false;
					int i = 0;
					while (matchingMatFound == false && i < baseMaterials.Length) {
						if (RGBCompare (objectMats [m].color, baseMaterials [i].color)) {							
							objectMats [m].color = new Color (objectMats [m].color.r, objectMats [m].color.g, objectMats [m].color.b, startingAlphas[i] * alphaPercentage);
							matchingMatFound = true;
						} else {
							i++;
						}

					}

				}

				meshRen.materials = objectMats;

			}

			yield return new WaitForFixedUpdate ();
		}

		SetMaterialsToOpaque ();
	}


	private bool RGBCompare (Color color1, Color color2) {
		bool rCheck = Mathf.Abs (color1.r - color2.r) < 0.04f;
		bool gCheck = Mathf.Abs (color1.g - color2.g) < 0.04f;
		bool bCheck = Mathf.Abs (color1.b - color2.b) < 0.04f;

		return (rCheck && gCheck && bCheck);
	}

}
