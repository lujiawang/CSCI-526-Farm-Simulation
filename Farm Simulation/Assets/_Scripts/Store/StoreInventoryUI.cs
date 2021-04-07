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
            // case "All":
            //     param = 4;
            //     break;
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
                // case 4:
                //     showSeeds = true;
                //     showHarvests = true;
                //     showOthers = true;
                //     break;
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
                // Debug.LogWarning("InventoryUI:GetShowParam() argument error");
                return false;
        }
    }

    public void ToggleRespectiveShowButton(int id)
    {
        if(AreAllShowParamsOn())
            return;
        else if(id >= 0 && id <= Item.seedIdUpperLimit && !showSeeds)
        {
            foreach(Transform child in this.transform)
            {
                if(child.childCount > 0 && child.GetChild(0).name == "Seeds")
                {
                    SelectiveShow cScript = child.GetComponent<SelectiveShow>();
                    cScript.ShowHide();
                    return;
                }
            }
        }
        else if(id > Item.seedIdUpperLimit && id <= Item.harvestIdUpperLimit && !showHarvests)
        {
            foreach(Transform child in this.transform)
            {
                if(child.childCount > 0 && child.GetChild(0).name == "Harvests")
                {
                    SelectiveShow cScript = child.GetComponent<SelectiveShow>();
                    cScript.ShowHide();
                    return;
                }
            }
        }
        else if(id > Item.harvestIdUpperLimit && id <= Item.allIdUpperLimit && !showOthers)
        {
            foreach(Transform child in this.transform)
            {
                if(child.childCount > 0 && child.GetChild(0).name == "Others")
                {
                    SelectiveShow cScript = child.GetComponent<SelectiveShow>();
                    cScript.ShowHide();
                    return;
                }
            }
        }
    }

    public bool AreAllShowParamsOn()
    {
        return showSeeds && showHarvests && showOthers;
    }

    bool ShouldShow(int id)
    {
        if(id <= Item.seedIdUpperLimit) //slot is a seed
        {
            return showSeeds;
        }else if(id <= Item.harvestIdUpperLimit) //slot is a harvest
        {
            return showHarvests;
        }else //other
        {
            return showOthers;
        }
    }

    void UpdateUI(bool remainScrollPosition, bool doDestroyAll)
    {
    	// Debug.Log("Updating Inventory UI");
    	InventorySlot[] slots = itemsParent.GetComponentsInChildren<InventorySlot>(false);

    	// Debug.Log(itemsCount);
        if(!doDestroyAll)
        {
            int i = 0;
            foreach(Item item in storeInventory.items)
            {
                if(!ShouldShow(item.Id()))
                    continue;
                // update displayed number
                slots[i].transform.Find("ItemButton").Find("Number").GetComponent<Text>().text = "" + item.Num();
                i++;
            }
            return;
        }

        storeInventory.items.Sort();

        foreach(Transform slot in itemsParent)
        {
            Destroy(slot.gameObject);
        }

        foreach(Item item in storeInventory.items)
        {
            if(!ShouldShow(item.Id()))
                continue;
            GameObject newSlot = Instantiate(slotPrefab, itemsParent);
            Transform ItemButton = newSlot.transform.Find("ItemButton");
            // change Name
            ItemButton.Find("Text").GetComponent<Text>().text = item.Name();
            // change Number
            ItemButton.Find("Number").GetComponent<Text>().text = "" + item.Num();
            // set "Value"
            ItemButton.Find("Price").GetComponent<Text>().text = "" + item.BuyPrice();
            // disable DragAndDrop script
            ItemButton.Find("Item").GetComponent<DragAndDrop>().enabled = false;
            // change Sprite
            ItemButton.Find("Item").GetComponent<Image>().sprite = item.Icon();
            // change "Item" to the new name
            ItemButton.Find("Item").name = item.Name();
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
