using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this class can only be used with InventorySLot prefab obj
public class AccentObj : MonoBehaviour
{	
	Coroutine lastCOR;
	string lastTargetName;
	// Vector3 lastOriginalScale;
	Vector3 originalScale = new Vector3(1,1,1);

	[Range(0,1)]
	public float defaultDuration = 0.3f;
	float stayStillDuration = 0.1f;

	public IEnumerator Accent(Transform target)
	{
		if(target == null)
			yield break;
		if(lastTargetName == GetName(target) && lastCOR != null)
		{
			StopCoroutine(lastCOR);
			// target.localScale = lastOriginalScale;
		}

		target.localScale = originalScale;
		
		lastTargetName = GetName(target);
		// lastOriginalScale = target.localScale;
		lastCOR = StartCoroutine(AccentCOR(target, defaultDuration));

		yield return lastCOR;
	}

	public IEnumerator Accent(Transform target, float duration)
	{
		if(target == null)
			yield break;
		if(lastTargetName == GetName(target) && lastCOR != null)
		{
			StopCoroutine(lastCOR);
			// target.localScale = lastOriginalScale;
		}

		target.localScale = originalScale;
		
		lastTargetName = GetName(target);
		// lastOriginalScale = target.localScale;
		lastCOR = StartCoroutine(AccentCOR(target, duration));

		yield return lastCOR;
	}

	public string GetName(Transform target)
	{
		if(target == null)
			return null;
		return target.parent.Find("Text").GetComponent<Text>().text;
	}

	// public bool isEqual(Transform target1, Transform target2)
	// {
	// 	if(target1 == null || target2 == null)
	// 		return false;
	// 	string str1 = target1.parent.Find("Text").GetComponent<Text>().text;
	// 	string str2 = target2.parent.Find("Text").GetComponent<Text>().text;
	// 	return str1 == str2;
	// }

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
