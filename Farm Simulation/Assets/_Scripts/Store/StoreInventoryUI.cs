using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class StoreInventoryUI : MonoBehaviour
{
	public Transform itemsParent;

	public GameObject slotPrefab;

	StoreInventory storeInventory;

    bool showSeeds = true;
    bool showHarvests = true;
    bool showOthers = true;

    // Start is called before the first frame update
    void Start()
    {
    	storeInventory = StoreInventory.instance;
    	storeInventory.onStoreItemChangedCallback += UpdateUI;
    }

    public void SetShowParam(string showParam)
    {
        int param = 0;
        switch(showParam)
        {
            case "Seeds":
                showSeeds = !showSeeds;
                param = 1;
                break;
            case "Harvests":
                showHarvests = !showHarvests;
                param = 2;
                break;
            case "Others":
                showOthers = !showOthers;
                param = 3;
                break;
            default:
                Debug.LogWarning("InventoryUI:ReverseShowParam() argument error");
                return;
        }
        // not all true scenario, pick the one selected to be true, otehrs false
        if(showSeeds || showHarvests || showOthers)
        {
            switch(param)
            {
                case 1:
                    showSeeds = true;
                    showHarvests = false;
                    showOthers = false;
                    break;
                case 2:
                    showSeeds = false;
                    showHarvests = true;
                    showOthers = false;
                    break;
                case 3:
                    showSeeds = false;
                    showHarvests = false;
                    showOthers = true;
                    break;
                default:
                    return;
            }
        }else // all false, turns to all true
        {
            showSeeds = true;
            showHarvests = true;
            showOthers = true;
        }
        UpdateUI(false, true);
    }

    public bool GetShowParam(string showParam)
    {
        switch(showParam)
        {
            case "Seeds":
                return showSeeds;
            case "Harvests":
                return showHarvests;
            case "Others":
                return showOthers;
            default:
                Debug.LogWarning("InventoryUI:GetShowParam() argument error");
                return false;
        }
    }

    public bool AreAllShowParamsOn()
    {
        return showSeeds && showHarvests && showOthers;
    }

    void ShowHide(GameObject slot, int id)
    {
        if(id <= Item.seedIdUpperLimit) //slot is a seed
        {
            slot.SetActive(showSeeds);
        }else if(id <= Item.harvestIdUpperLimit) //slot is a harvest
        {
            slot.SetActive(showHarvests);
        }else //other
        {
            slot.SetActive(showOthers);
        }
    }

    void UpdateUI(bool remainScrollPosition, bool doDestroyAll)
    {
    	// Debug.Log("Updating Inventory UI");
    	InventorySlot[] slots = itemsParent.GetComponentsInChildren<InventorySlot>(true);
    	int itemsCount = storeInventory.items.Count;
    	int slotsCount = slots.Length;

    	// Debug.Log(itemsCount);
        if(!doDestroyAll)
        {
            for(int i = 0; i < slotsCount; i++)
            {
                int newNum = storeInventory.items[i].Num();
                // update displayed number
                slots[i].transform.Find("ItemButton").Find("Number").gameObject.GetComponent<Text>().text = "" + newNum;
            }
            // change scroll height after updating UI
            // StartCoroutine(ScrollHeightRoutine(remainScrollPosition));
            return;
        }

        storeInventory.items.Sort();
        // yield return null;
        for(int i = 0; i < slotsCount; i++)
        {
            Destroy(slots[i].gameObject);
        }

        for(int i = 0; i < itemsCount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, itemsParent) as GameObject;
            if(newSlot == null){
                Debug.LogWarning("NULL");
            }
            Transform ItemButton = newSlot.transform.Find("ItemButton");
            // change Name
            ItemButton.Find("Text").gameObject.GetComponent<Text>().text = storeInventory.items[i].Name();
            // change Number
            ItemButton.Find("Number").gameObject.GetComponent<Text>().text = "" + storeInventory.items[i].Num();
            // set "Value"
            ItemButton.Find("Price").gameObject.GetComponent<Text>().text = "" + storeInventory.items[i].BuyPrice();
            // change Sprite
            ItemButton.Find("Item").gameObject.GetComponent<Image>().sprite = storeInventory.items[i].Icon();
            // change "Item" to the new name
            ItemButton.Find("Item").gameObject.name = storeInventory.items[i].Name();
            // disable DragAndDrop script
            ItemButton.GetChild(0).gameObject.GetComponent<DragAndDrop>().enabled = false;

            // determine whether Show or hide the slot based on showParams
            ShowHide(newSlot, storeInventory.items[i].Id());
        }

        StartCoroutine(ScrollHeightRoutine(remainScrollPosition));

    }

    IEnumerator ScrollHeightRoutine(bool remainScrollPosition)
    {
        yield return null;
        ScrollHeight cScript = GetComponent<ScrollHeight>();
        cScript.UpdateHeight(itemsParent, remainScrollPosition);
    }
}
