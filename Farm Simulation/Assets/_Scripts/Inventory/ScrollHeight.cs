using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHeight : MonoBehaviour
{
	float originalWidth; // original width of itemsParent
	float originalHeight; // original height of itemsParent
	float originalAnchorX; // original anchorPosition.x of itemsParent
	float originalAnchorY; // original anchorPosition.y of itemsParent
	int columnCount = 4;

	float heightOfOneCell = 180f;

	void Start()
	{
		GameObject itemsParentObj = this.transform.GetChild(0).GetChild(0).gameObject;
		if(itemsParentObj.name != "ItemsParent")
			Debug.LogWarning("Fatal error: fecthed wrong object.");
		RectTransform itemsParentRectTransform = itemsParentObj.GetComponent<RectTransform>();
		originalWidth = itemsParentRectTransform.sizeDelta.x;
		originalHeight = itemsParentRectTransform.sizeDelta.y;
		// Debug.Log(originalHeight);
		// Debug.Log(originalWidth);
		originalAnchorY = itemsParentRectTransform.anchoredPosition.y;
		originalAnchorX = itemsParentRectTransform.anchoredPosition.x;
		// Debug.Log(originalAnchorY);
		StartCoroutine(UpdateHeightRoutine(itemsParentObj.transform));
	}

	public void UpdateHeight(Transform itemsParent)
	{
		StartCoroutine(UpdateHeightRoutine(itemsParent));
	}

	public IEnumerator UpdateHeightRoutine(Transform itemsParent)
	{
		int rows = (itemsParent.childCount + columnCount - 1) / columnCount;
		// Debug.Log(rows);
		RectTransform rectTransform = itemsParent.GetComponent<RectTransform>();
		if(rows > 4)
		{
			float diffHeight = (rows-4) * heightOfOneCell;
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight + diffHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY - diffHeight/2);
		}
		else
		{
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY);
		}
		yield return null;
		// Debug.Log(rectTransform.anchoredPosition.y);
		// Debug.Log(rectTransform.offsetMax);
		// Debug.Log(rectTransform.sizeDelta.y);
		// Debug.Log(rectTransform.sizeDelta.y);
	}

}
