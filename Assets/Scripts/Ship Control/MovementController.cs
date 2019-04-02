using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    //-----VARIABLES-----

	private Vector3 currentTargetPosition = Vector3.zero;
	private Vector3 trueLastPositionAdded;

	private List<Vector3> targetPositions = new List<Vector3>();
	public List<Vector3> TargetPositions { get; }

	private Quaternion targetRot;
	private Quaternion endRotation;

	public float maxForwardSpeed;
	private float currentForwardSpeed;
	public float CurrentForwardSpeed { get; }

	public float forwardAcceleration;
	public float distanceFalloff;

	public float maxRotationSpeed;
	private float currentRotationSpeed;
	public float rotationAcceleration;
	private float angleFalloff = 10f;

	public float earlyTurnDistance;

    public bool logSpeedValues;
    public bool idle;

    //-----METHODS-----

    /// <summary>
    /// 
    /// </summary>
	public void Initialise() {

	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="chain"></param>
	public void AddMoveToPosition (Vector3 position, bool chain) {
		if (chain == true) {
			targetPositions [targetPositions.Count - 1] = trueLastPositionAdded;
			trueLastPositionAdded = position;

			targetPositions.Add (position);
		} else {
            targetPositions.Clear();
            trueLastPositionAdded = position;

			targetPositions.Add(position);
		}

	}    

    /// <summary>
    /// 
    /// </summary>
	void FixedUpdate () {
		if (!idle) {
			if (targetPositions.Count > 0) {
				currentTargetPosition = targetPositions [0];
				float distToTarget = Vector3.Distance (transform.position, currentTargetPosition);

				if (distToTarget > 1f) {
					targetRot = Quaternion.LookRotation (currentTargetPosition - transform.position);

					float angleFalloffModifier = Mathf.Clamp (Quaternion.Angle (transform.rotation, targetRot) / angleFalloff, 0f, 1f);
					if (Quaternion.Angle (transform.rotation, targetRot) > 0.1f) {
						currentRotationSpeed = Mathf.Clamp (currentRotationSpeed + (rotationAcceleration * Time.fixedDeltaTime), 0, maxRotationSpeed * angleFalloffModifier);

						transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRot, currentRotationSpeed * Time.fixedDeltaTime);
					}

					float rotSlowDownModififer = Mathf.Clamp (1f - ((currentRotationSpeed) / maxRotationSpeed), 0.2f, 1f);
					float distanceFalloffModifier = Mathf.Clamp (distToTarget / distanceFalloff, 0f, 1f);

					currentForwardSpeed = Mathf.Clamp (currentForwardSpeed + (forwardAcceleration * Time.fixedDeltaTime), 0, (maxForwardSpeed * distanceFalloffModifier) * rotSlowDownModififer);
					transform.Translate (Vector3.forward * currentForwardSpeed * Time.fixedDeltaTime);

					if (distToTarget <= earlyTurnDistance && targetPositions.Count > 1) {
						targetPositions.RemoveAt (0);
					}

					//Debug print statments
					if (logSpeedValues) {
						print (Mathf.Round ((((currentRotationSpeed * angleFalloffModifier) / maxRotationSpeed) * 100)) + "% - Rotational Speed");
						print (Mathf.Round (((currentForwardSpeed / maxForwardSpeed) * 100)) + "% - Forward Speed");
					}
				} else {
					currentForwardSpeed = 0f;
					targetPositions.Clear ();
				}
			} else {
				float angleFalloffModifier = Mathf.Clamp (Quaternion.Angle (transform.rotation, endRotation) / angleFalloff, 0f, 1f);
				if (Quaternion.Angle (transform.rotation, endRotation) > 0.1f) {
					currentRotationSpeed = Mathf.Clamp (currentRotationSpeed + (rotationAcceleration * Time.fixedDeltaTime), 0, maxRotationSpeed * angleFalloffModifier);

					transform.rotation = Quaternion.RotateTowards (transform.rotation, endRotation, currentRotationSpeed * Time.fixedDeltaTime);
				} else {
					currentRotationSpeed = 0f;
				}
			}
		}

	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
	Quaternion LookAt2D (Vector3 worldPosition) {
		Vector3 lookAtPos = worldPosition - transform.position;			
		float rotZ = Mathf.Atan2(lookAtPos.y, lookAtPos.x) * Mathf.Rad2Deg;
		Quaternion newDir = Quaternion.Euler(0f, 0f, rotZ - 90);

		return newDir;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
	private float RandomSign(float value) {
        if (Random.Range(0, 2) == 0) {
            return value;
        } else {
            return -value;
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
    public bool drawGizmos;
    public bool showVisualisations;
    void OnDrawGizmos () {
        if (drawGizmos) {
            //Hide/view path based gizmos
            if (Application.isPlaying == false) {
                if (showVisualisations) {
                    targetPositions = new List<Vector3>();
                    targetPositions.Add(Vector3.zero);
                } else {
                    targetPositions = null;
                }
            } else {
                showVisualisations = false;
            }

            if (showVisualisations) {
                //Player directional gizmos
                Gizmos.DrawRay(transform.position, transform.forward * 100f);

                //Player position gizmos
                Gizmos.DrawCube(transform.position + (Vector3.up * 10f), Vector3.one * 2f);
            }

            //Gizmos for path based variables
            if (targetPositions != null && targetPositions.Count > 0) {
                //Distance falloff gizmos
                Vector3 targetToTransformDir = (transform.position - currentTargetPosition).normalized * distanceFalloff;
                Vector3 falloffTangent = Vector3.Cross(targetToTransformDir, Vector3.up).normalized;

                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, falloffTangent * 7.5f);
                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, -falloffTangent * 7.5f);

                //Early turning distance gizmo
                targetToTransformDir = targetToTransformDir.normalized * earlyTurnDistance;
                Vector3 earlyTurningTangent = Vector3.Cross(targetToTransformDir, Vector3.up).normalized;

                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, earlyTurningTangent * 15f);
                Gizmos.DrawRay(currentTargetPosition + targetToTransformDir, -earlyTurningTangent * 15f);

                //Target directional gizmos
                Gizmos.DrawLine(transform.position, currentTargetPosition);

                //Target position gizmos
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
