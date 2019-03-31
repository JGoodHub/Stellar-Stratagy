using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //-----SINGLETON SETUP-----

	public static GameManager instance = null;
	
	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	//-----VARIABLES-----
	
	

    //-----METHODS-----
	
    /// <summary>
    /// 
    /// </summary>
	void Start() {
        //Independent initialisations
        CameraManager.instance.Initalise();
        PlayerManager.instance.Initialise();

        //Dependent initialisations

    }

    //-----GIZMOS-----
    //public bool drawGizmos;
    void OnDrawGizmos() {
	
	}
	
}
