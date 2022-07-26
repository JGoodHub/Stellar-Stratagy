using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectedStatsUI : MonoBehaviour {

    [Header("Selected Stats UI")]

    public TextMeshProUGUI nameText;
    public Image shipImage;

    [Space]

    public Image healthFillImage;
    public TextMeshProUGUI healthFillText;

    private void Start() {
        SelectionController.Instance.OnSelectionChanged += OnSelectionChangeHandler;
    }

    private void OnSelectionChangeHandler(object sender, Entity prevEntity, Entity newEntity) {
        // if (newEntity != null) {
        //
        //     healthFillImage.color = GameManager.FactionColors[newEntity.alignment];
        //
        //     //healthFillImage.fillAmount = newEntity.GetComponent<StatsController>().GetResource(StatType.HULL).Normalised;
        //
        //     nameText.text = newEntity.name;
        //
        // }
    }
}
