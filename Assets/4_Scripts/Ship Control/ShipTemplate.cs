using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipTemplate", menuName = "ScriptableObjects/Create New Ship Template", order = 1)]
public class ShipTemplate : ScriptableObject
{
    [Header("Metadata")]
    public string loadoutName;
    public string loadoutId;

    [Header("Resources")]
    public List<ResourcePair> startingResources = new List<ResourcePair>();


    [Header("Effects")]
    public GameObject shipExplosionPrefab;


    public int GetResource(ResourceType type)
    {
        foreach (ResourcePair resourcePair in startingResources)
        {
            if (resourcePair.type == )
        }
    }
}

[Serializable]
public struct ResourcePair
{
    public ResourceType type;
    public int amount;
}

public enum ResourceType
{
    HULL,
    SHIELD,
    FUEL,
    MISSILE,
    CREW
};

public enum WeaponType
{
    LASER,
    MISSILE
};

public enum HardpointType
{
    SMALL,
    LARGE
};