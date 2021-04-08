using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowToast : MonoBehaviour
{
	public static int showNumber = 1;
	public Text txt1;
	public Text txt2;

	Coroutine a;
	Coroutine b;
	Coroutine customized;

	Text lastCustomizedText;

	public void showToast(string text, int duration)
	{
		Text txt = null;
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
		
		if(showNumber == 1)
		{
			if(a != null) StopCoroutine(a);
			a = StartCoroutine(showToastCOR(txt, text, duration));
		}
		else if(showNumber == 2)
		{
			if(b != null) StopCoroutine(b);
			b = StartCoroutine(showToastCOR(txt, text, duration));
		}
		showNumber++;
		if(showNumber > 2)
			showNumber = 1;
	}

	public void showCustomizedToast(Text textComponent, string text, int duration)
	{
		if(textComponent == null)
			return;
		if(textComponent == lastCustomizedText && customized != null) StopCoroutine(customized);
		customized = StartCoroutine(showToastCOR(textComponent, text, duration));
		lastCustomizedText = textComponent;
	}

	public IEnumerator showToastCOR(Text txt, string text, int duration)
	{
		if(txt == null)
			yield break;
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

	    if(txt == null)
			yield break;
	    txt.enabled = false;
	    txt.color = orginalColor;

	    if(txt == txt1)
	    	showNumber = 1;
	}

	public IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
	{
		if(targetText == null)
			yield break;
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
	    	if(targetText == null)
				yield break;
	        counter += Time.deltaTime;
	        float alpha = Mathf.Lerp(a, b, counter / duration);

	        targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
	        yield return null;
	    }
	}
}
