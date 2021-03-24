using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoreToggle : MonoBehaviour
{

    public CanvasGroup BackButton;

    public CanvasGroup StoreInventory;

    GameObject inventoryObj;

    // GameObject storeInventoryObj;


    void Start()
    {
        inventoryObj = GameObject.FindGameObjectsWithTag("Inventory")[0];
        // storeInventoryObj = StoreInventory.gameObject;

        // SwitchSlotsInteractability(inventoryObj, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenStore();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CloseStore();
        }
    }

    public void OpenStore()
    {
        BackButton.alpha = 1f;
    	BackButton.blocksRaycasts = true;
    	StoreInventory.alpha = 1f;
    	StoreInventory.blocksRaycasts = true;
        // open inventory menu
        MenuAppear cScript = inventoryObj.GetComponent<MenuAppear>();
        if(!MenuAppear.isMenu)
            cScript.MenuHideAndShow();
        // disable all InventorySlots' Items so they can't be dragged
        SwitchSlotsInteractability(inventoryObj, false);
        // SwitchSlotsInteractability(storeInventoryObj, false);
    }

    public void CloseStore()
    {
    	BackButton.alpha = 0f;
    	BackButton.blocksRaycasts = false;
    	StoreInventory.alpha = 0f;
    	StoreInventory.blocksRaycasts = false;
        // close inventory menu
        MenuAppear cScript = inventoryObj.GetComponent<MenuAppear>();
        if(MenuAppear.isMenu)
            cScript.MenuHideAndShow();
        // enable all InventorySlots' Items so they can be dragged
        SwitchSlotsInteractability(inventoryObj, true);
    }

    public void SwitchSlotsInteractability(GameObject inventory, bool onOrOff)
    {
        Transform itemsParentObj = inventory.transform.GetChild(0).GetChild(0);
        foreach(Transform slot in itemsParentObj)
        {
            GameObject itemObj = slot.GetChild(0).GetChild(0).gameObject;
            if(itemObj.name.Contains("Seed"))
            {
                CanvasGroup itemCanvasGroup = itemObj.GetComponent<CanvasGroup>();
                itemCanvasGroup.interactable = onOrOff;
                itemCanvasGroup.blocksRaycasts = onOrOff;
            }
        }
    }


}
