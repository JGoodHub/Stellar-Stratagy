using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : Entity
{
    public MovementController MovementController { get; private set; }
    public ShipData ShipData { get; private set; }
    public FogOfWarMask FogMask { get; private set; }

    protected override void Start()
    {
        base.Start();

        MovementController = GetComponent<MovementController>();
        ShipData = GetComponent<ShipData>();
        FogMask = GetComponent<FogOfWarMask>();
    }

}
