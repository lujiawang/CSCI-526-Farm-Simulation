using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class StoreToggle : MonoBehaviour
{

    public CanvasGroup BackButton;

    public CanvasGroup StoreInventory;

    GameObject inventoryObj;

    public static bool isStoreOpen;

    // GameObject storeInventoryObj;


    void Start()
    {
        inventoryObj = GameObject.FindGameObjectsWithTag("Inventory")[0];
        // storeInventoryObj = StoreInventory.gameObject;

        // SwitchSlotsInteractability(inventoryObj, true);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
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
    }*/

    public void OpenStore()
    {
        if(isStoreOpen)
            return;
        BackButton.alpha = 1f;
    	BackButton.blocksRaycasts = true;
    	StoreInventory.alpha = 1f;
    	StoreInventory.blocksRaycasts = true;
        // open inventory menu
        MenuAppear cScript = inventoryObj.GetComponent<MenuAppear>();
        if(!MenuAppear.isMenu)
            cScript.MenuHideAndShow();
        // disable all InventorySlots' Items so they can't be dragged
        StartCoroutine(SwitchSlotsInteractability(inventoryObj, false));
        // SwitchSlotsInteractability(storeInventoryObj, false);
        isStoreOpen = true;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        TouchToMove.disablePlayerMovement = true;
    }

    public void CloseStore()
    {
        if(!isStoreOpen)
            return;
    	BackButton.alpha = 0f;
    	BackButton.blocksRaycasts = false;
    	StoreInventory.alpha = 0f;
    	StoreInventory.blocksRaycasts = false;
        // close inventory menu
        MenuAppear cScript = inventoryObj.GetComponent<MenuAppear>();
        // if(MenuAppear.isMenu)
        //     cScript.MenuHideAndShow();
        // enable all InventorySlots' Items so they can be dragged
        StartCoroutine(SwitchSlotsInteractability(inventoryObj, true));
        isStoreOpen = false;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        TouchToMove.disablePlayerMovement = false;
    }

    public IEnumerator SwitchSlotsInteractability(GameObject inventory, bool onOrOff)
    {
        // Debug.Log(onOrOff);
        yield return null;
        Transform itemsParentObj = inventory.transform.GetChild(0).GetChild(0);
        // Debug.Log(itemsParentObj.childCount);
        foreach(Transform slot in itemsParentObj)
        {
            // Debug.Log("switched!");
            GameObject itemObj = slot.GetChild(0).GetChild(0).gameObject;
            if(itemObj.name.Contains("Seed"))
            {
                CanvasGroup itemCanvasGroup = itemObj.GetComponent<CanvasGroup>();
                itemCanvasGroup.interactable = onOrOff;
                itemCanvasGroup.blocksRaycasts = onOrOff;
                // Debug.Log(itemCanvasGroup.blocksRaycasts);
            }
        }
    }


}
