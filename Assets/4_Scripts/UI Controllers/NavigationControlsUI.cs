using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationControlsUI : MonoBehaviour {

    [Header("Thrust")]

    public Button increaseSpeedButton;
    public Button decreaseSpeedButton;

    public Image[] enginePowerBars;

    [Header("Fuel")]

    public Image fuelBarFill;
    public Text fuelEmptyText;

    public Color criticalColour;
    public Color lowColour;
    public Color highColour;

    public float criticalThreshold = 0.1f;
    public float lowThreshold = 0.4f;

    [Header("Mini-map")]

    public Button zoomInButton;
    public Button zooOutButton;


    // Start is called before the first frame update
    private void Start() {
        increaseSpeedButton.onClick.AddListener(IncreaseShipSpeed);
        increaseSpeedButton.onClick.AddListener(UpdateEngineBars);

        decreaseSpeedButton.onClick.AddListener(DecreaseShipSpeed);
        decreaseSpeedButton.onClick.AddListener(UpdateEngineBars);
    }

    private void Update() {
        float fuelBarFillAmount = PlayerManager.Instance.playerShip.ShipStats.GetResourceOfType(ShipStats.ResourceType.FUEL, true);

        if (fuelBarFillAmount <= 0) {
            fuelEmptyText.enabled = true;
        } else {
            fuelEmptyText.enabled = false;

            fuelBarFill.fillAmount = fuelBarFillAmount;

            if (fuelBarFillAmount <= criticalThreshold)
                fuelBarFill.color = criticalColour;
            else if (fuelBarFillAmount > criticalThreshold && fuelBarFillAmount <= lowThreshold)
                fuelBarFill.color = lowColour;
            else if (fuelBarFillAmount > lowThreshold)
                fuelBarFill.color = highColour;
        }

        UpdateEngineBars();
    }

    public void IncreaseShipSpeed() {
        PlayerManager.Instance.playerShip.FlightController.IncreaseSpeed();
    }

    public void DecreaseShipSpeed() {
        PlayerManager.Instance.playerShip.FlightController.DecreaseSpeed();
    }

    public void UpdateEngineBars() {
        for (int i = 0; i < enginePowerBars.Length; i++) {
            enginePowerBars[i].enabled = false;
        }

        for (int i = 0; i < PlayerManager.Instance.playerShip.FlightController.speedSetting; i++) {
            enginePowerBars[i].enabled = true;
        }
    }


}
