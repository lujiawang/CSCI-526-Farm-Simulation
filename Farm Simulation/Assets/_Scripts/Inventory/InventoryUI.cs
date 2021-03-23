using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour
{
	public Transform itemsParent;

	public GameObject slotPrefab;

	Inventory inventory;

	InventorySlot[] slots;
    // Start is called before the first frame update
    void Start()
    {
    	inventory = Inventory.instance;
    	inventory.onItemChangedCallback += UpdateUI;

    	slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        
    }

    // Update is called once per frame
    void Update()
    {
    	// slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        
    }

    void UpdateUI()
    {
    	// Debug.Log("Updating Inventory UI");
    	slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    	int itemsCount = inventory.items.Count;
    	int slotsCount = slots.Length;

    	// Debug.Log(itemsCount);


    	for(int i = 0; i < itemsCount; i++)
    	{
    		int j = 0;
    		for(j = 0; j < slotsCount; j++)
    		{
    			Transform ItemButton = slots[j].transform.Find("ItemButton");
    			GameObject Name = ItemButton.Find("Text").gameObject;
    			string name = Name.GetComponent<Text>().text;
    			// if already has this item in inventory
    			if(name == inventory.items[i].Name())
	    		{
	    			// Convert.ToInt16(ItemButton.Find("Number").gameObject.GetComponent<Text>().text)
	    			int newNum = inventory.items[i].Num();
                    // update displayed number
	    			ItemButton.Find("Number").gameObject.GetComponent<Text>().text = "" + newNum;
                    // update image
                    // if(ItemButton.Find(name).gameObject.GetComponent<Image>().sprite != inventory.items[i].Icon()){
                    //     ItemButton.Find(name).gameObject.GetComponent<Image>().sprite = inventory.items[i].Icon();
                    // }

	    			j = -1;
	    			break;
	    			
	    			// newSlot.AddItem(inventory.items[i]);
	    		}
    		}

    		// if did not find a match in slots with items, create a new one
    		if(j != -1)
    		{
    			GameObject newSlot = Instantiate(slotPrefab, itemsParent) as GameObject;
    			if(newSlot == null){
    				Debug.LogWarning("NULL");
    			}
    			Transform ItemButton = newSlot.transform.Find("ItemButton");
    			// change Name
    			ItemButton.Find("Text").gameObject.GetComponent<Text>().text = inventory.items[i].Name();
    			// change Number
    			ItemButton.Find("Number").gameObject.GetComponent<Text>().text = "" + inventory.items[i].Num();
                // set "Value"
                ItemButton.Find("Price").gameObject.GetComponent<Text>().text = "" + inventory.items[i].SellPrice();
    			// change Sprite
    			ItemButton.Find("Item").gameObject.GetComponent<Image>().sprite = inventory.items[i].Icon();
    			// change "Item" to the new name
    			ItemButton.Find("Item").gameObject.name = inventory.items[i].Name();
    		}
    		
    	}

    	// clear out all the unmatched slots
    	slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    	for(int i = 0; i < slotsCount; i++)
    	{
    		Transform ItemButton = slots[i].transform.Find("ItemButton");
			GameObject Name = ItemButton.Find("Text").gameObject;
			string name = Name.GetComponent<Text>().text;

			int j = 0;
    		for(j = 0; j < itemsCount; j++)
    		{
    			if(name == inventory.items[j].Name())
    			{
    				j = -1;
    				break;
    			}
    		}
    		// if did not find a match in items with slots, destroy the slot
    		if(j != -1) 
    		{
    			Destroy(slots[i].gameObject);
    			// Debug.Log(slots[i]);
    		}
    	}

    	// for(int i = 0; i < slots.Length; i++)
    	// {
    	// 	if(i < inventory.items.Count)
    	// 	{
    	// 		slots[i].AddItem(inventory.items[i]);
    	// 	}else
    	// 	{
    	// 		slots[i].ClearSlot();
    	// 	}
    	// }


    }
}
