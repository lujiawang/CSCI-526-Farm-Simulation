using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartAll : MonoBehaviour
{
    public void ReStart()
    {
    	PlayerPrefs.DeleteAll();
    	Inventory inventory = Inventory.instance;
    	inventory.ReStart();
    	StoreInventory storeInventory = StoreInventory.instance;
    	storeInventory.ReStart();
    }
}
