using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    //-----VARIABLES-----

    public MovementController MovementController { get; private set; }
    public ShipData ShipData { get; private set; }
    public FogOfWarMask FogMask { get; private set; }

    public GameManager.Faction faction;

    [SerializeField] private GameObject selectionRing;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        MovementController = GetComponent<MovementController>();
        ShipData = GetComponent<ShipData>();
        FogMask = GetComponent<FogOfWarMask>();

        SetSelectionState(false);
    }


    public void SetSelectionState(bool state)
    {
        selectionRing.SetActive(state);
    }
}
