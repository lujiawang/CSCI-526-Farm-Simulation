using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreToggle : MonoBehaviour
{

    public CanvasGroup BackButton;

    public CanvasGroup StoreInventory;

    GameObject InventoryObj;


    void Start()
    {
    	// CloseStore();
        InventoryObj = GameObject.FindGameObjectsWithTag("Inventory")[0];
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
        MenuAppear cScript = InventoryObj.GetComponent<MenuAppear>();
        if(!MenuAppear.isMenu)
            cScript.MenuHideAndShow();
    }

    public void CloseStore()
    {
    	BackButton.alpha = 0f;
    	BackButton.blocksRaycasts = false;
    	StoreInventory.alpha = 0f;
    	StoreInventory.blocksRaycasts = false;
        // close inventory menu
        MenuAppear cScript = InventoryObj.GetComponent<MenuAppear>();
        if(MenuAppear.isMenu)
            cScript.MenuHideAndShow();
    }


}
