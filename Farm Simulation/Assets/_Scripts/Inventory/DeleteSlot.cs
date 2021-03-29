﻿// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSlot : MonoBehaviour
{
	Inventory inventory;
	GameObject itemButton;

	void Start()
	{
		inventory = Inventory.instance;
    	itemButton = this.transform.parent.gameObject;
	}
	
    public void DeleteThisSlot()
    {
    	string itemName = itemButton.transform.Find("Text").GetComponent<Text>().text;
    	inventory.Add(itemName, -Inventory.stackLimit);
    }
}