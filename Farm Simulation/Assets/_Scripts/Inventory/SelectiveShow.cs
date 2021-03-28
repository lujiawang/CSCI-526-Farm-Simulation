using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.EventSystems;

public class SelectiveShow : MonoBehaviour
{
	string buttonName;
	bool isInventory;

	void Start()
	{
		buttonName = this.transform.GetChild(0).gameObject.name;
		isInventory = this.transform.parent.gameObject.name == "Inventory";
	}


	public void ShowHide()
	{
		if(isInventory)
		{
			GameObject inventoryObj = this.transform.parent.gameObject;
			InventoryUI cScript = inventoryObj.GetComponent<InventoryUI>();
			cScript.SetShowParam(buttonName);

			foreach(Transform child in cScript.transform)
			{
				if(child.gameObject.name.Contains("SelectiveShowButton"))
				{
					string childButtonName = child.GetChild(0).gameObject.name;
					Button button = child.GetComponent<Button>();
					bool buttonOn = cScript.GetShowParam(childButtonName) && !cScript.AreAllShowParamsOn();
					Image buttonImg = child.GetComponent<Image>();
					buttonImg.color = buttonOn ? button.colors.highlightedColor : button.colors.normalColor;
				}
			}
		}else //storeInventory
		{
			GameObject storeInventoryObj = this.transform.parent.gameObject;
			StoreInventoryUI cScript = storeInventoryObj.GetComponent<StoreInventoryUI>();
			cScript.SetShowParam(buttonName);

			foreach(Transform child in cScript.transform)
			{
				if(child.gameObject.name.Contains("SelectiveShowButton"))
				{
					string childButtonName = child.GetChild(0).gameObject.name;
					Button button = child.GetComponent<Button>();
					bool buttonOn = cScript.GetShowParam(childButtonName) && !cScript.AreAllShowParamsOn();
					Image buttonImg = child.GetComponent<Image>();
					buttonImg.color = buttonOn ? button.colors.highlightedColor : button.colors.normalColor;
				}
			}
		}

	}

}
