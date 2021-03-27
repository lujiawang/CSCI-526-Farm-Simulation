// this file demonstrates how to subscribe to InternetTime.cs

// SUMMARY: InternetTime fetches time from internet every few seconds, defined by FetchInterval. 
// FetchInterval can be defined in GameManager.
// You can update certain stuff based on the change of internet time by subscribing your function to
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
        if(PlayerPrefs.HasKey("exitInternetTime"))
        {
            DateTime lastRecordedTimeBeforeExitGame = DateTime.Parse(PlayerPrefs.GetString("exitInternetTime"));
        }
    }

    // "UpdateXXX" will be called each time InternetTime updates the current internet time
    void UpdateXXX()
    {
        DateTime updatedTime = internetTime.GetTime(); 
        // stuff that needs to be updated when time changes
    }

    // if you want to update internet time immediately
    IEnumerator UpdateTimeImmediately() // note that the function type IEnumerator cannot change
    {
        yield return StartCoroutine(internetTime.FetchTime());
        // This is after internetTime is updated. Do stuff here. 
    }
}
