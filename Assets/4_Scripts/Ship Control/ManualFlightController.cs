using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualFlightController : MonoBehaviour
{
    //-----VARIABLES-----

    public ShipController ShipController => GetComponent<ShipController>();

    [Header("Thrust")] 
    
    public const int SPEED_DIVISIONS = 10;

    public int speedSetting = 0;
    private int speedSettingCap = SPEED_DIVISIONS;

    private float currentSpeed;
    private float targetSpeed;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

    [Header("Rotation")] public AnimationCurve rotationSpeedCurve;
    public GameObject directionRingObject;

    [SerializeField] private float peakRotationSpeed;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    [Header("Fuel Rate")] public float maxFuelRate = 1f;

    [Header("Debug")] public bool logSpeedValues;
    public bool showVisualisations;


    //-----METHODS-----

    private void Update()
    {
        //Turning
        if (Quaternion.Angle(transform.rotation, targetRotation) >= 0.01f)
        {
            Quaternion rotation = transform.rotation;
            float angleNormalised = Quaternion.Angle(rotation, targetRotation) / Quaternion.Angle(startRotation, targetRotation);
            float angleModified = rotationSpeedCurve.Evaluate(angleNormalised) * peakRotationSpeed;

            rotation = Quaternion.RotateTowards(rotation, targetRotation, angleModified * Time.deltaTime);
            transform.rotation = rotation;
        }
        else
        {
            transform.rotation = targetRotation;
        }

        directionRingObject.transform.rotation = Quaternion.Lerp(directionRingObject.transform.rotation, Quaternion.LookRotation(targetRotation * Vector3.forward, Vector3.up), 0.2f);

        //Speed
        if (currentSpeed < targetSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else if (targetSpeed == 0 && currentSpeed < 0.01f)
        {
            currentSpeed = 0;
        }

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);

        //Fuel
        float currentFuelRate = maxFuelRate * (currentSpeed / maxSpeed);
        ShipController.ShipStats.ApplyChangeToResources(ShipStats.ResourceType.FUEL, -currentFuelRate * Time.deltaTime);
    }

    public void IncreaseSpeed()
    {
        speedSetting++;
        speedSetting = Mathf.Clamp(speedSetting, 0, speedSettingCap);

        UpdateTargetSpeed();
    }

    public void DecreaseSpeed()
    {
        speedSetting--;
        speedSetting = Mathf.Clamp(speedSetting, 0, speedSettingCap);

        UpdateTargetSpeed();
    }

    public void SetSpeedCap(int speedSettingCap)
    {
        this.speedSettingCap = Mathf.Clamp(speedSettingCap, 0, SPEED_DIVISIONS);
        speedSetting = Mathf.Clamp(speedSetting, 0, speedSettingCap);

        UpdateTargetSpeed();
    }

    private void UpdateTargetSpeed()
    {
        targetSpeed = (speedSetting / (float) SPEED_DIVISIONS) * maxSpeed;
    }

    public void SetHeading(float newHeading)
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(newHeading, Vector3.up) * Vector3.forward);
    }

    public void SetHeading(Vector3 target)
    {
        startRotation = transform.rotation;

        Vector3 direction = target - transform.position;
        direction.y = 0;
        targetRotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    //-----GIZMOS-----

    private void OnDrawGizmos()
    {
        if (showVisualisations)
        {
            //Player directional gizmos
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * 100f);

            //Player position gizmos
            Gizmos.DrawCube(transform.position + (Vector3.up * 10f), Vector3.one * 2f);
        }
    }
}