using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{

    //-----VARIABLES-----

    private bool isFriendly;
    public bool IsFriendly { set => isFriendly = value; }

    public Transform[] hardpointTransforms;
    private Turret[] turretControllers;

    private Transform targetTransform;
    private bool targetChanged;

    //-----METHODS-----

    private void Initialise()
    {
        turretControllers = new Turret[hardpointTransforms.Length];

        for (int i = 0; i < hardpointTransforms.Length; i++)
        {
            HardpointID hardID = hardpointTransforms[i].GetComponent<HardpointID>();

            GameObject turretPrefab = null;
            GameObject turretClone;
            Turret turretCloneController;

            switch (hardID.hardpointType)
            {
                case HardpointID.HardpointType.laserTurret:
                    switch (hardID.hardpointSize)
                    {
                        case HardpointID.HardpointSize.small:
                            turretPrefab = (GameObject)Resources.Load("Turrets/Laser Turret Small");
                            break;
                        case HardpointID.HardpointSize.large:
                            turretPrefab = (GameObject)Resources.Load("Turrets/Laser Turret Large");
                            break;
                    }
                    break;
                case HardpointID.HardpointType.missileTurret:
                    switch (hardID.hardpointSize)
                    {
                        case HardpointID.HardpointSize.small:
                            turretPrefab = (GameObject)Resources.Load("Turrets/Missile Turret Small");
                            break;
                        case HardpointID.HardpointSize.large:
                            turretPrefab = (GameObject)Resources.Load("Turrets/Missile Turret Large");
                            break;
                    }
                    break;
            }

            turretClone = (GameObject)Instantiate(turretPrefab, hardID.transform.position, hardID.transform.rotation);
            turretClone.transform.parent = hardID.transform;

            turretCloneController = turretClone.GetComponent<Turret>();
            turretControllers[i] = turretCloneController;
        }

    }


    public void SetTarget(Transform newTarget)
    {
        targetTransform = newTarget;

        foreach (Turret turretCont in turretControllers)
        {
            //turretCont.target = newTarget;		
        }

    }


    public void CeaseFire()
    {
        targetTransform = null;

        foreach (Turret turretCont in turretControllers)
        {
            //turretCont.target = null;		
        }

    }




}
