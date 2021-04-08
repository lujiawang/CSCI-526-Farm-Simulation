using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollHeight : MonoBehaviour
{
	float originalWidth; // original width of itemsParent
	float originalHeight; // original height of itemsParent
	float originalAnchorX; // original anchorPosition.x of itemsParent
	float originalAnchorY; // original anchorPosition.y of itemsParent
	int columnCount; // columns of the inventory
	float heightOfRow; // how much height to add if one more row is added

	float lastBaselineY; //the baseline anchorY that places the scrollRect at the very top

	float additionalHeight; // compensate for bottom objects that are partially hidden
	float normalRowCount; //maximum rows the UI can show (including partially hidden rows)

	void Start()
	{
		GameObject itemsParentObj = this.transform.GetChild(0).GetChild(0).gameObject;
		if(itemsParentObj.name != "ItemsParent")
			Debug.LogWarning("Fatal error: fetched wrong object.");
		RectTransform itemsParentRectTransform = itemsParentObj.GetComponent<RectTransform>();
		originalWidth = itemsParentRectTransform.sizeDelta.x;
		originalHeight = itemsParentRectTransform.sizeDelta.y;
		// Debug.Log(originalHeight);
		// Debug.Log(originalWidth);
		originalAnchorY = itemsParentRectTransform.anchoredPosition.y;
		originalAnchorX = itemsParentRectTransform.anchoredPosition.x;
		// Debug.Log(originalAnchorY);
		GridLayoutGroup itemsParentGrid = itemsParentObj.GetComponent<GridLayoutGroup>();
		columnCount = itemsParentGrid.constraintCount;
		// Debug.Log(columnCount);
		heightOfRow = itemsParentGrid.cellSize.y + itemsParentGrid.spacing.y;
		// Debug.Log(heightOfRow);
		lastBaselineY = originalAnchorY;
		if(this.name == "Inventory")
		{
			additionalHeight = 0f;
			normalRowCount = 4;
		}
		else if(this.name == "StoreInventory")// StoreInventory
		{
			additionalHeight = 80f;
			normalRowCount = 5;
		}else //Recipes
		{
			additionalHeight = 40f;
			normalRowCount = 4;
		}
		
		// UpdateHeightRoutine(itemsParentObj.transform, false);
	}

	public float currentAnchorY()
	{
		return this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition.y;
		// Debug.Log("current anchor Y= " + currAnchorY);
		// return currAnchorY;
	}

	public void UpdateHeight(Transform itemsParent, bool remainScrollPosition)
	{
		StartCoroutine(UpdateHeightRoutine(itemsParent, remainScrollPosition));
	}

	public void UpdateHeight(Transform itemsParent, int jumpToItemNumber)
	{
		StartCoroutine(UpdateHeightRoutine(itemsParent, jumpToItemNumber));
	}

	public IEnumerator UpdateHeightRoutine(Transform itemsParent, bool remainScrollPosition)
	{
		int rows = (itemsParent.childCount + columnCount - 1) / columnCount;
		// Debug.Log(rows);
		RectTransform rectTransform = itemsParent.GetComponent<RectTransform>();
		// float prevAnchorY = rectTransform.anchoredPosition.y;
		float diffAnchorY = currentAnchorY() - lastBaselineY;
		if(rows > normalRowCount)
		{
			float addHeight = (rows - normalRowCount) * heightOfRow + additionalHeight;
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight + addHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY - addHeight/2);
		}else if(rows == normalRowCount)
		{
			float addHeight = additionalHeight;
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight + addHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY - addHeight/2);
		}else
		{
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY);
		}

		lastBaselineY = currentAnchorY();

		// remain at scroll position per request
		if(remainScrollPosition)
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, diffAnchorY + currentAnchorY());
		// if(remainScrollPosition && rows >= normalRowCount)
		// 	rectTransform.anchoredPosition = new Vector2(originalAnchorX, prevAnchorY);

		yield return null;
	}

	public IEnumerator UpdateHeightRoutine(Transform itemsParent, int jumpToItemNumber)
	{
		int rows = (itemsParent.childCount + columnCount - 1) / columnCount;
		RectTransform rectTransform = itemsParent.GetComponent<RectTransform>();
		int jumpToRow = (jumpToItemNumber + columnCount) / columnCount; //start from row 1
		float diffAnchorY = (jumpToRow - 1) * heightOfRow;
		if(rows > normalRowCount)
		{
			float addHeight = (rows - normalRowCount) * heightOfRow + additionalHeight;
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight + addHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY - addHeight/2);
		}else if(rows == normalRowCount)
		{
			float addHeight = additionalHeight;
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight + addHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY - addHeight/2);
		}else
		{
			rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight);
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, originalAnchorY);
		}

		lastBaselineY = currentAnchorY();

		// jump to position per request
		rectTransform.anchoredPosition = new Vector2(originalAnchorX, diffAnchorY + currentAnchorY());

		yield return null;
	}

}
