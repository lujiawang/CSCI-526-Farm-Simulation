// this file demonstrates how to subscribe to InternetTime.cs

// SUMMARY: InternetTime fetches time from internet every 10 seconds. You can update
// certain stuff based on the change of internet time by subscribing your function to
// InternetTime.cs


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HowToUseInternetTime : MonoBehaviour
{
	InternetTime internetTime;
	
    void Start()
    {
        internetTime = InternetTime.instance;
        // "UpdateXXX" is the function that is subscribed to internetTime.onTimeChangedCallback
        internetTime.onTimeChangedCallback += UpdateXXX; 
    }

    // "UpdateXXX" will be called each time InternetTime updates the current internet time
    void UpdateXXX()
    {
        DateTime updatedTime = internetTime.GetTime(); 
        // stuff that needs to be updated when time changes
    }
}
