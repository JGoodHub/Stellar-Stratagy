using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public new string name;
    public int id;
    public Type type;
    [Space]
    [SerializeField] private GameObject selectionRing;
    [SerializeField] private float selectionRadius;

    public float SelectionRadius => selectionRadius;

    [Header("Factions")]
    public GameManager.Faction alignment;


    protected virtual void Start()
    {
        SelectionController.Instance.RegisterEntity(this);
        SetSelected(false);

        id = Random.Range(0, 1000000);
    }

    private void OnDestroy()
    {
        if (SelectionController.Instance != null)
            SelectionController.Instance.UnregisterEntity(this);
    }

    public void SetSelected(bool state)
    {
        if (selectionRing != null)
            selectionRing.SetActive(state);
    }

    public enum Type
    {
        SHIP,
        STATION,
        ASTEROID,
        WARPGATE
    }

    private void OnDrawGizmos()
    {
        GizmoExtensions.DrawWireCircle(transform.position, selectionRadius, Color.green);
    }
}