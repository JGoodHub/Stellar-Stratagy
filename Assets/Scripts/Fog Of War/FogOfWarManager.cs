using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour {

    //-----SINGLETON SETUP-----

    public static FogOfWarManager instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    //-----VARIABLES-----

    public GameObject shipMaskPrefab;
    private Dictionary<Transform, Transform> friendlyShipMasks = new Dictionary<Transform, Transform>();

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    public void Initialise() {

    }

    /// <summary>
    /// 
    /// </summary>
    void Update() {
        foreach (Transform shipTransform in friendlyShipMasks.Keys) {
            friendlyShipMasks[shipTransform].position = new Vector3(shipTransform.position.x, 999, shipTransform.position.z);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shipTransform"></param>
    /// <param name="maskRadius"></param>
    public void AddNewShipMask (Transform shipTransform, float maskRadius) {
        GameObject shipMaskObject = Instantiate(shipMaskPrefab, new Vector3(0, 1000, 0), Quaternion.identity);
        shipMaskObject.transform.localScale = Vector3.one * (maskRadius / 4f);

        friendlyShipMasks.Add(shipTransform, shipMaskObject.transform);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shipTransform"></param>
    public void RemoveShipMask (Transform shipTransform) {
        if (friendlyShipMasks.ContainsKey(shipTransform)) {
            Destroy(friendlyShipMasks[shipTransform]);
            friendlyShipMasks.Remove(shipTransform);
        } else {
            Debug.LogError("Trying to destory mask with an invalid ship transform");
        }
    }
	
	//-----GIZMOS-----
	//public bool drawGizmos;
	void OnDrawGizmos() {
	
	}
	
}
