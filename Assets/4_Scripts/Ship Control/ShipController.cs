using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : Entity {

    public ManualFlightController FlightController => GetComponent<ManualFlightController>();
    public ShipStats ShipStats => GetComponent<ShipStats>();
    public FogOfWarMask FogMask => GetComponent<FogOfWarMask>();

    //protected override void Start() {
    //    base.Start();
    //}

}
