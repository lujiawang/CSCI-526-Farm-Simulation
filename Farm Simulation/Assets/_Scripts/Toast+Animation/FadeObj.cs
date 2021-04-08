using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeObj : MonoBehaviour
{	
	Coroutine lastCOR;
	Transform lastTarget;

	[Range(0,1)]
	public float defaultDuration = 0.3f;

	public IEnumerator Fade(Transform target, bool fadeIn)
	{
		if(target == null)
			yield break;
		if(target == lastTarget && lastCOR != null)
		{
			StopCoroutine(lastCOR);
		}
		
		lastTarget = target;
		lastCOR = StartCoroutine(FadeCOR(target, fadeIn, defaultDuration));

		yield return lastCOR;
	}

	public IEnumerator Fade(Transform target, bool fadeIn, float duration)
	{
		if(target == null)
			yield break;
		if(target == lastTarget && lastCOR != null)
		{
			StopCoroutine(lastCOR);
		}
		
		lastTarget = target;
		lastCOR = StartCoroutine(FadeCOR(target, fadeIn, duration));

		yield return lastCOR;
	}

	IEnumerator FadeCOR(Transform target, bool fadeIn, float duration)
	{
		if(target == null)
			yield break;

		if(!fadeIn) // disable interactability of the obj if obj will be destroyed
			target.GetComponent<CanvasGroup>().blocksRaycasts = false;
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

	    float counter = 0f;
	    while (counter < duration)
	    {
	    	if(target == null)
				yield break;
	        counter += Time.deltaTime;
	        float alpha = Mathf.Lerp(a, b, counter / duration);

	        target.GetComponent<CanvasGroup>().alpha = alpha;
	        yield return null;
	    }
	}

}
