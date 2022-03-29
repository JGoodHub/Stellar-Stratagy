using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : Entity {

    public FlightController Helm => GetComponent<FlightController>();
    public StatsController Stats => GetComponent<StatsController>();
    public FogOfWarMask FogMask => GetComponent<FogOfWarMask>();
    public LaserWeaponsController LaserWeaponsController => GetComponent<LaserWeaponsController>();

    public OrbitalController OrbitalController => GetComponent<OrbitalController>();

}
