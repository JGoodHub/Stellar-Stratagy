using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsUI : Singleton<PlayerStatsUI>
{

    [Header("Player Stats UI")]

    public Image hullFillImage;
    public TextMeshProUGUI hullFillText;

    [Space]

    public Image shieldFillImage;
    public TextMeshProUGUI shieldFillText;



}
