using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedStatsUI : MonoBehaviour
{

    [Header("Selected Stats UI")]

    public TextMeshProUGUI nameText;
    public Image shipImage;

    [Space]

    public Image healthFillImage;
    public TextMeshProUGUI healthFillText;

    [Space]

    public Color alliedColour;
    public Color neutralColour;
    public Color enemyColour;
}
