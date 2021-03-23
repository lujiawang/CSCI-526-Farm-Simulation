using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
	Inventory inventory;
	StoreInventory storeInventory;

	void Start()
    {
    	inventory = Inventory.instance;
    	storeInventory = StoreInventory.instance;
    }

    public void Buy()
    {
    	// Debug.Log("Buy");
    	string name = this.transform.Find("Text").gameObject.GetComponent<Text>().text;
    	storeInventory.Add(name, -1);
    	inventory.Add(name, 1);
    }
}
