using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuySellItem : MonoBehaviour
{
	Inventory inventory;
	StoreInventory storeInventory;
    Transform canvas;
    CanvasGroup storeInventoryCanvasGroup;

	void Start()
    {
    	inventory = Inventory.instance;
    	storeInventory = StoreInventory.instance;
        canvas = this.transform.parent.parent.parent.parent.parent;
        storeInventoryCanvasGroup = canvas.Find("StoreInventory").gameObject.GetComponent<CanvasGroup>();
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
        string priceStr = this.transform.Find("Price").gameObject.GetComponent<Text>().text;
        int price = Convert.ToInt16(priceStr);
        // if currecy is enough to buy item
        if(PlayerStats.Currency >= price)
        {
            string name = this.transform.Find("Text").gameObject.GetComponent<Text>().text;
            storeInventory.Add(name, -1);
            inventory.Add(name, 1);
            PlayerStats.ChangeCurrency(-price);

            // change tab if necessary
            InventoryUI cScript = canvas.Find("Inventory").GetComponent<InventoryUI>();
            cScript.ToggleRespectiveShowButton(Item.GetCropId(name));
        }else
        {
            ShowToast cScript = canvas.gameObject.GetComponent<ShowToast>();
            cScript.showToast("You don't have enough coins!", 1);
        }
        
    }

    public void Sell()
    {
        string priceStr = this.transform.Find("Price").gameObject.GetComponent<Text>().text;
        int price = Convert.ToInt16(priceStr);
        // can sell only if store is open
        if(storeInventoryCanvasGroup.blocksRaycasts)
        {
            string name = this.transform.Find("Text").gameObject.GetComponent<Text>().text;
            storeInventory.Add(name, 1);
            inventory.Add(name, -1);
            PlayerStats.ChangeCurrency(price);

            // change tab if necessary
            StoreInventoryUI cScript = canvas.Find("StoreInventory").GetComponent<StoreInventoryUI>();
            cScript.ToggleRespectiveShowButton(Item.GetCropId(name));
        }else //store is closed, show toast message
        {
            // ShowToast cScript = canvas.gameObject.GetComponent<ShowToast>();
            // cScript.showToast("You have to go to the store to sell your items!", 1);
        }
    }
}