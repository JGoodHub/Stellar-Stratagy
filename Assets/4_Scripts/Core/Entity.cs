using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    [Header("Entity")]

    public new string name;

    [SerializeField] private GameObject selectionRing;
    [SerializeField] private float selectionRadius;
    public float SelectionRadiusSqrd => selectionRadius * selectionRadius; //TO DO: Cache result

    [Header("Factions")]

    public GameManager.Faction owner;

    private void OnEnable() {
        SelectionController.RegisterEntity(this);
    }

    private void OnDisable() {
        SelectionController.UnregisterEntity(this);
    }

    protected virtual void Start() {
        SetSelected(false);
    }

    public void SetSelected(bool state) {
        selectionRing.SetActive(state);
    }


    private void OnDrawGizmos() {
        switch (owner) {
            case GameManager.Faction.PLAYER:
                Gizmos.color = Color.green;
                break;
            case GameManager.Faction.ENEMY:
                Gizmos.color = Color.red;
                break;
            case GameManager.Faction.INDEPENDENT:
                Gizmos.color = Color.yellow;
                break;
            case GameManager.Faction.NONE:
                Gizmos.color = Color.white;
                break;
        }

        GizmoExtensions.DrawWireCircle(transform.position, selectionRadius);
    }
}



