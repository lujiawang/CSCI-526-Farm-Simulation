// This file demonstrates how to show toast messages


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToShowToastMessage : MonoBehaviour
{
	GameObject canvas;

    void Start()
    {
        canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];
    }

    void Foo()
    {
       	ShowToast cScript = canvas.GetComponent<ShowToast>();
        cScript.showToast("The toast message you want to display", 1); //the int value is the duration of the message
    }
}
