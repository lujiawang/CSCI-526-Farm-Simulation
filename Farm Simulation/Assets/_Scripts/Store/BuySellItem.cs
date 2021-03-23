using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySellItem : MonoBehaviour
{
	Inventory inventory;
	StoreInventory storeInventory;

	void Start()
    {
    	inventory = Inventory.instance;
    	storeInventory = StoreInventory.instance;
    }

    public void BuySell()
    {
    	// Debug.Log("Buy");
        if(this.transform.parent.parent.parent.parent.gameObject.name == "Inventory")
        {
            Sell();
        }else
            Buy();
    	
    }

    public void Buy()
    {
        // if(money >= )
        string name = this.transform.Find("Text").gameObject.GetComponent<Text>().text;
        storeInventory.Add(name, -1);
        inventory.Add(name, 1);
    }

    public void Sell()
    {
        string name = this.transform.Find("Text").gameObject.GetComponent<Text>().text;
        storeInventory.Add(name, 1);
        inventory.Add(name, -1);
    }
}
