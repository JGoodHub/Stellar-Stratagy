using System;
using UnityEngine;

public class GlobalSingleton<T> : SceneSingleton<T> where T : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
