using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomObj : MonoBehaviour
{	
	Coroutine lastCOR;
	Transform lastTarget;

	[Range(0,1)]
	public float defaultDuration = 0.3f;

	public IEnumerator Zoom(Transform target, bool zoomIn)
	{
		if(target == null)
			yield break;
		if(target == lastTarget && lastCOR != null) StopCoroutine(lastCOR);
		lastCOR = StartCoroutine(zoomInOrOut(target, zoomIn, defaultDuration));
		lastTarget = target;

		yield return lastCOR;
	}

	public IEnumerator Zoom(Transform target, bool zoomIn, float duration)
	{
		if(target == null)
			yield break;
		if(target == lastTarget && lastCOR != null) StopCoroutine(lastCOR);
		lastCOR = StartCoroutine(zoomInOrOut(target, zoomIn, duration));
		lastTarget = target;

		yield return lastCOR;
	}

	IEnumerator zoomInOrOut(Transform target, bool zoomIn, float duration)
	{
		if(target == null)
			yield break;

		if(!zoomIn) // disable interactability of the obj if obj will be destroyed
			target.GetComponent<CanvasGroup>().blocksRaycasts = false;
		float a, b;
	    if (zoomIn)
	    {
	        a = 0f;
	        b = 1f;
	    }
	    else
	    {
	        a = 1f;
	        b = 0f;
	    }

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
	    // destroy target when zooming out
	    if(!zoomIn)
	    	Destroy(target.gameObject);
	}

}
