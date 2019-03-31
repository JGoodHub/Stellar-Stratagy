using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeObjectsManager : MonoBehaviour {

    //-----SINGLETON SETUP-----

    public static RuntimeObjectsManager instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    //-----ENUMS-----

    //public enum CollectionName { EXPLOSIONS };

    //-----STRUCTS-----

    [SerializeField]
    public struct RuntimeCollection {
        public string CollectionName;
        public Transform CollectionTransform;
    }

    //-----VARIABLES-----

    public RuntimeCollection[] runtimeCollections;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    public void Initialise() {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="collectionName"></param>
    public void AddToCollection (GameObject obj, string collectionName) {
        foreach(RuntimeCollection collection in runtimeCollections) {
            if (collectionName == collection.CollectionName) {
                obj.transform.SetParent(collection.CollectionTransform);
                break;
            }
        }
    }

    //-----GIZMOS-----
    //public bool drawGizmos;
    void OnDrawGizmos() {

    }

}
