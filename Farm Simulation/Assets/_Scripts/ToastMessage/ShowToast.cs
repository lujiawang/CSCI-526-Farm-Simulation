using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowToast : MonoBehaviour
{
    void Start()
	{
	    // showToast("Hello", 2);
	    // showToast("dfbaj", 3);
	    // showToast("ohpogprejig", 2);
	}

	public static int showNumber = 1;
	public Text txt1;
	public Text txt2;

	public void showToast(string text, int duration)
	{
		StartCoroutine(showToastCOR(text, duration));	    
	}

	public IEnumerator showToastCOR(string text, int duration)
	{
		Text txt;
		switch(showNumber)
		{
			case 1:
				txt = txt1;
				break;
			case 2:
				txt = txt2;
				break;
			default:
				txt = txt1;
				break;
		}
		showNumber++;
		if(showNumber > 2)
			showNumber = 1;
		txt.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		
	    Color orginalColor = txt.color;

	    txt.text = text;
	    txt.enabled = true;

	    //Fade in
	    yield return fadeInAndOut(txt, true, 0.5f);

	    //Wait for the duration
	    float counter = 0;
	    while (counter < duration)
	    {
	        counter += Time.deltaTime;
	        yield return null;
	    }

	    //Fade out
	    yield return fadeInAndOut(txt, false, 0.5f);

	    txt.enabled = false;
	    txt.color = orginalColor;
	}

	public IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
	{
	    //Set Values depending on if fadeIn or fadeOut
	    float a, b;
	    if (fadeIn)
	    {
	        a = 0f;
	        b = 1f;
	    }
	    else
	    {
	        a = 1f;
	        b = 0f;
	    }

	    Color currentColor = targetText.color;
	    float counter = 0f;

	    while (counter < duration)
	    {
	        counter += Time.deltaTime;
	        float alpha = Mathf.Lerp(a, b, counter / duration);

	        targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
	        yield return null;
	    }
	}
}
