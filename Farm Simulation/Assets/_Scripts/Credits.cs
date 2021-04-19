using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public void ShowCredits()
    {
    	FadeObj fadeScript = GameObject.Find("SceneCanvas").GetComponent<FadeObj>();
    	Transform panel = this.transform;
    	StartCoroutine(fadeScript.Fade(panel, true, 0.4f));

    	// ZoomObj zoomScript = GameObject.Find("SceneCanvas").GetComponent<ZoomObj>();
    	// StartCoroutine(zoomScript.Zoom(panel, true, 0.4f));
    	// this.GetComponent<CanvasGroup>().alpha = 1f;
    	this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void HideCredits()
    {
    	FadeObj fadeScript = GameObject.Find("SceneCanvas").GetComponent<FadeObj>();
    	Transform panel = this.transform;
    	StartCoroutine(fadeScript.Fade(panel, false, 0.4f));
    	// this.GetComponent<CanvasGroup>().alpha = 0f;
    	this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
