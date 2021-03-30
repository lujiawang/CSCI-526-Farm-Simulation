// This file demonstrates how to use soundmanager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToUseSoundManager : MonoBehaviour
{
    // FIRST THING to do: You MUST attach a new AudioSource component to GameManager->SoundPlayer
    // Uncheck "Play on Awake"
    // And set the AudioClip you want to play in the new AudioSource 
    // SECOND: Update the first PlaySound() funtion of SoundManager.cs to have a new case

	SoundManager soundManager;

	void Start()
	{
		soundManager = SoundManager.instance;
	}
    void Foo()
    {
    	string clipName = "XXX Name"; // the new clip name you added to PlaySound() cases
    	int index = 100; // the new AudioSource' case number that you added to the PlaySound() cases
    	soundManager.PlaySound(clipName);
    	// Or
    	soundManager.PlaySound(index);
    }

    // if you want to call PlaySound() from a place where the SoundPlayer itself may have not finished rendering
    // For example, if you want to call PlaySound() from Start() or Awake() method
    void Foo2()
    {
    	StartCoroutine(yourCOR());
    }

    IEnumerator yourCOR()
    {
    	string clipName = "XXX Name"; // the new clip name you added to PlaySound() cases
    	int index = 100; // the new AudioSource' case number that you added to the PlaySound() cases
    	bool endLoop = false;
        while(!endLoop)
        {
            endLoop = soundManager.PlaySound(index);
            // Or
            endLoop = soundManager.PlaySound(clipName);
            yield return null; 
        }
    }
}
