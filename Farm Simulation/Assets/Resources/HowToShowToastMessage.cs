// This file demonstrates how to show toast messages


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToShowToastMessage : MonoBehaviour
{
	GameObject canvas;
    Text text;

    void Start()
    {
        canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];
    }

    void Foo()
    {
       	ShowToast cScript = canvas.GetComponent<ShowToast>();
        cScript.showToast("The toast message you want to display", 1); //the int value is the duration of the message
    
        // if you have a customized Text comopnent you want to display
        cScript.showCustomizedToast(text, "The toast message you want to display", 1);
    }
}
