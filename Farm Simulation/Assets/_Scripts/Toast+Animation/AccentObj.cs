using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccentObj : MonoBehaviour
{	
	Coroutine lastCOR;
	Transform lastTarget;
	Vector3 lastOriginalScale;

	[Range(0,1)]
	public float defaultDuration = 0.3f;
	float stayStillDuration = 0.1f;

	public IEnumerator Accent(Transform target)
	{
		if(target == null)
			yield break;
		if(target == lastTarget && lastCOR != null)
		{
			StopCoroutine(lastCOR);
			target.localScale = lastOriginalScale;
		}
		
		lastTarget = target;
		lastOriginalScale = target.localScale;
		lastCOR = StartCoroutine(AccentCOR(target, defaultDuration));

		yield return lastCOR;
	}

	public IEnumerator Accent(Transform target, float duration)
	{
		if(target == null)
			yield break;
		if(target == lastTarget && lastCOR != null)
		{
			StopCoroutine(lastCOR);
			target.localScale = lastOriginalScale;
		}
		
		lastTarget = target;
		lastOriginalScale = target.localScale;
		lastCOR = StartCoroutine(AccentCOR(target, duration));

		yield return lastCOR;
	}

	IEnumerator AccentCOR(Transform target, float duration)
	{
		if(target == null)
			yield break;

		float a = 1f, b = 2f;

	    Vector3 originalScale = target.localScale;
	    float counter = 0f;

	    while (counter < duration)
	    {
	    	if(target == null)
				yield break;
	        counter += Time.deltaTime;
	        float scale = Mathf.Lerp(a, b, counter / duration);

	        target.localScale = scale * new Vector3(originalScale.x, originalScale.y, originalScale.z);
	        yield return null;
	    }

	    counter = 0f;
	    while (counter < stayStillDuration)
	    {
	    	if(target == null)
				yield break;
	        counter += Time.deltaTime;
	        yield return null;
	    }
	    if(target == null)
			yield break;
	    target.localScale = originalScale;
	}

}
