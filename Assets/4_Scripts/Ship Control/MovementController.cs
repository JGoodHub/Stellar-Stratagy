using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    
    ///<summary>Controls the point in the turn (in degrees) when the ships decelerates its angular velocity</summary>
    private const float DECELERATION_ANGLE_CUTOFF = 10f;

    //-----VARIABLES-----

    private Vector3 currentTargetPosition = Vector3.zero;
    private Vector3 trueLastPositionAdded;

    private List<Vector3> targetPositions = new List<Vector3>();

    private Quaternion targetRot;
    private Quaternion endRotation;

    [Header("Forward Speed & Modifiers")]
    [SerializeField] private float maxForwardSpeed;
    [SerializeField] private float forwardAcceleration;
    private float currentForwardSpeed;
    [Space]
    [SerializeField] private float distanceFalloff;
    [SerializeField] private float earlyTurnDistance;

    [Header("Rotation Speed")]
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float rotationAcceleration;
    private float currentRotationSpeed;
    
    [Header("Debug")]
    public bool logSpeedValues;
    public bool showVisualisations;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="chain"></param>
	public void AddMoveToPosition(Vector3 position, bool chain) {
        if (chain == true) {
            targetPositions[targetPositions.Count - 1] = trueLastPositionAdded;
            trueLastPositionAdded = position;

            targetPositions.Add(position);
        } else {
            targetPositions.Clear();
            trueLastPositionAdded = position;

            targetPositions.Add(position);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate() {
        if (targetPositions.Count > 0) {
            currentTargetPosition = targetPositions[0];
            float distToTarget = Vector3.Distance(transform.position, currentTargetPosition);

            if (distToTarget > 1f) {
                targetRot = Quaternion.LookRotation(currentTargetPosition - transform.position);

                //When the angle to the next target is not directly ahead begin to accelerate the turning speed up to the maximum
                //Reduce the maximum as we begin to point towards the target to smoothly slow the turn to a stop
                float decelerationModifier = Mathf.Clamp(Quaternion.Angle(transform.rotation, targetRot) / DECELERATION_ANGLE_CUTOFF, 0f, 1f);
                if (Quaternion.Angle(transform.rotation, targetRot) > 0.1f) {
                    currentRotationSpeed = Mathf.Clamp(currentRotationSpeed + (rotationAcceleration * Time.fixedDeltaTime), 0, maxRotationSpeed * decelerationModifier);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, currentRotationSpeed * Time.fixedDeltaTime);
                }

                //Slow down the forward speed proportional to the turning speed
                float rotSlowDownModififer = Mathf.Clamp(1f - (currentRotationSpeed / maxRotationSpeed), 0.1f, 1f);

                //Slow down to forward speed as we approach the target vector
                float distanceFalloffModifier = Mathf.Clamp01(distToTarget / distanceFalloff);

                currentForwardSpeed = Mathf.Clamp(currentForwardSpeed + (forwardAcceleration * Time.fixedDeltaTime), 0, maxForwardSpeed * distanceFalloffModifier * rotSlowDownModififer);
                transform.Translate(Vector3.forward * currentForwardSpeed * Time.fixedDeltaTime);

                //If there's more than one vector start the turn early to stay on the path
                if (distToTarget <= earlyTurnDistance && targetPositions.Count > 1) {
                    targetPositions.RemoveAt(0);
                }

                //Debug print statements
                if (logSpeedValues) {
                    print(Mathf.Round(Mathf.Round((currentRotationSpeed / maxRotationSpeed) * 100)) + "% - Rotational Speed");
                    print(Mathf.Round(Mathf.Round((currentForwardSpeed / maxForwardSpeed) * 100)) + "% - Forward Speed");
                }
            } else {
                currentForwardSpeed = 0f;
                targetPositions.Clear();
            }
        } else {
            //When the angle to the next target is not directly ahead begin to accelerate the turning speed up to the maximum
            //Reduce the maximum as we begin to point towards the target to smoothly slow the turn to a stop
            float decelerationModifier = Mathf.Clamp(Quaternion.Angle(transform.rotation, endRotation) / DECELERATION_ANGLE_CUTOFF, 0f, 1f);
            if (Quaternion.Angle(transform.rotation, endRotation) > 0.1f) {
                currentRotationSpeed = Mathf.Clamp(currentRotationSpeed + (rotationAcceleration * Time.fixedDeltaTime), 0, maxRotationSpeed * decelerationModifier);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, currentRotationSpeed * Time.fixedDeltaTime);
            } else {
                currentRotationSpeed = 0f;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endRot"></param>
	public void SetEndRotation(Quaternion endRot) {
        endRotation = endRot;
    }

    //-----GIZMOS-----

    private void OnDrawGizmos() {

        if (showVisualisations) {
            //Player directional gizmos
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * 100f);

            //Player position gizmos
            Gizmos.DrawCube(transform.position + (Vector3.up * 10f), Vector3.one * 2f);

            //Gizmos for path based variables
            if (targetPositions != null && targetPositions.Count > 0) {
                //Distance falloff gizmos
                Gizmos.color = Color.cyan;
                Vector3 targetToTransformDir = (transform.position - currentTargetPosition).normalized * distanceFalloff;
                Vector3 falloffTangent = Vector3.Cross(targetToTransformDir, Vector3.up).normalized;

                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, falloffTangent * 7.5f);
                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, -falloffTangent * 7.5f);

                //Early turning distance gizmo
                Gizmos.color = Color.green;
                targetToTransformDir = targetToTransformDir.normalized * earlyTurnDistance;
                Vector3 earlyTurningTangent = Vector3.Cross(targetToTransformDir, Vector3.up).normalized;

                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, earlyTurningTangent * 15f);
                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, -earlyTurningTangent * 15f);

                //Target directional and position gizmos
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, currentTargetPosition);
                Gizmos.DrawCube(currentTargetPosition + (Vector3.up * 10f), Vector3.one * 2f);

                //Path visualisation gizmos
                Gizmos.DrawSphere(targetPositions[0], 1f);
                for (int i = 1; i < targetPositions.Count; i++) {
                    Gizmos.DrawSphere(targetPositions[i], 1f);
                    Gizmos.DrawLine(targetPositions[i - 1], targetPositions[i]);
                }

            }

        }

    }

}
