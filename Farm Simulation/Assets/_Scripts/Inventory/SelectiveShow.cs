using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			cScript.ReverseShowParam(buttonName);
		}else //storeInventory
		{
			GameObject storeInventoryObj = this.transform.parent.gameObject;
			StoreInventoryUI cScript = storeInventoryObj.GetComponent<StoreInventoryUI>();
			cScript.ReverseShowParam(buttonName);
		}
	}
}
