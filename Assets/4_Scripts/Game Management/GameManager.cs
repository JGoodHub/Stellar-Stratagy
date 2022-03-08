using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public enum Faction {
        PLAYER,
        ENEMY,
        INDEPENDENT,
        NONE
    };

    public static Dictionary<Faction, Color> FactionColors { get; private set; }

    private void Start() {
        FactionColors = new Dictionary<Faction, Color>();

        FactionColors.Add(Faction.PLAYER, Color.green);
        FactionColors.Add(Faction.ENEMY, Color.red);
        FactionColors.Add(Faction.INDEPENDENT, Color.yellow);
        FactionColors.Add(Faction.NONE, Color.white);
    }

}
