using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime;
using UnityEngine;

public class GameManager : SceneSingleton<GameManager> {

    public enum Faction {
        FRIENDLY,
        ENEMY,
        NEUTRAL,
        NONE
    };

    public static Dictionary<Faction, Color> FactionColors { get; private set; }

    private void Start() {
        FactionColors = new Dictionary<Faction, Color>();

        FactionColors.Add(Faction.FRIENDLY, Color.green);
        FactionColors.Add(Faction.ENEMY, Color.red);
        FactionColors.Add(Faction.NEUTRAL, Color.yellow);
        FactionColors.Add(Faction.NONE, Color.white);
    }

}
