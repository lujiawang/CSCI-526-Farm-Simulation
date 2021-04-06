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
		float currAnchorY =  this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition.y;
		// Debug.Log("current anchor Y= " + currAnchorY);
		return currAnchorY;
	}

	public void UpdateHeight(Transform itemsParent, bool remainScrollPosition)
	{
		StartCoroutine(UpdateHeightRoutine(itemsParent, remainScrollPosition));
	}

	public IEnumerator UpdateHeightRoutine(Transform itemsParent, bool remainScrollPosition)
	{
		int rows = 0;
		if(this.name == "Inventory" || this.name == "StoreInventory")
		{
			// don't get inactive children
			InventorySlot[] slots = itemsParent.GetComponentsInChildren<InventorySlot>(false);
	        // Debug.Log("slotsCount: "+slots.Length);
	        rows = (slots.Length + columnCount - 1) / columnCount;
		}else // Recipes
		{
			rows = itemsParent.childCount / 2;
			// Debug.Log(rows);
		}
		// Debug.Log(rows);
		RectTransform rectTransform = itemsParent.GetComponent<RectTransform>();
		float prevAnchorY = rectTransform.anchoredPosition.y;
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

		// remain at scroll position per request
		if(remainScrollPosition && rows >= normalRowCount)
			rectTransform.anchoredPosition = new Vector2(originalAnchorX, prevAnchorY);

		yield return null;
		// Debug.Log("current anchor Y= "+rectTransform.anchoredPosition.y);
		// Debug.Log(rectTransform.offsetMax);
		// Debug.Log(rectTransform.sizeDelta.y);
		// Debug.Log(rectTransform.sizeDelta.y);
	}

}
