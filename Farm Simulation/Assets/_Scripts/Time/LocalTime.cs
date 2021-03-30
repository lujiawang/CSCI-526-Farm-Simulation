using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LocalTime : MonoBehaviour
{
    // private static GameObject instance;

    // void Awake()
    // {
    //     DontDestroyOnLoad(this.gameObject);
    //     if (instance == null)
    //          instance = this.gameObject;
    //     else
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
    void OnDestroy()
    {
    	SaveLocalTime();
    }

    void SaveLocalTime()
    {
    	// save local time
    	PlayerPrefs.SetString("exitLocalTime", System.DateTime.Now.ToString());
    	// Debug.Log(System.DateTime.Now.ToString());
    }
}
