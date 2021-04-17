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

    ZoomObj zoomScript;
    AccentObj accentScript;

	Inventory inventory;

    bool showSeeds = true;
    bool showHarvests = true;
    bool showOthers = true;
    bool showDelete = false;

    // Start is called before the first frame update
    void Start()
    {
        zoomScript = GetComponentInParent<Canvas>().rootCanvas.GetComponent<ZoomObj>();
        accentScript = this.GetComponent<AccentObj>();
    	inventory = Inventory.instance;
        inventory.onItemChangedCallback = null;
    	inventory.onItemChangedCallback += UpdateUI;

        UpdateUI(false);
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
        UpdateUI(false);
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
        if(AreAllShowParamsOn() || !MenuAppear.isMenu)
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

    public bool SetDeleteParam()
    {
        showDelete = !showDelete;
        UpdateUI(true);
        return showDelete;
    }

    public bool GetDeleteParam()
    {
        return showDelete;
    }

    void ShowHideDeleteBtn(GameObject slot)
    {
        Button deleteButton = slot.transform.Find("ItemButton").Find("DeleteButton").GetComponent<Button>();
        deleteButton.interactable = showDelete;
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

    // PARAM remainScrollPosition indicates whether to maintain previous scrollView position
    void UpdateUI(bool remainScrollPosition)
    {
        Coroutine lastDestoryCOR = null;
        inventory.items.Sort();
        // inventory.items should always be sorted
        List<Item> showItems = new List<Item>();
        foreach(Item item in inventory.items)
        {
            if(ShouldShow(item.Id()))
                showItems.Add(item);
        }

        // Delete items that are not in showItems list
        foreach(Transform slot in itemsParent)
        {
            string name = slot.GetChild(0).Find("Text").GetComponent<Text>().text;
            bool doDestroy = true;
            foreach(Item item in showItems)
            {
                if(name == item.Name())
                {
                    doDestroy = false;
                    break;
                }
            }
            if(doDestroy)
                lastDestoryCOR = StartCoroutine(zoomScript.Zoom(slot, false));
        }

        int firstAddedItemIndex = -1;
        // Add/Update items
        for(int i = 0; i < showItems.Count; i++)
        {
            bool foundMatch = false;
            foreach(Transform slot in itemsParent)
            {
                string name = slot.GetChild(0).Find("Text").GetComponent<Text>().text;
                if(name == showItems[i].Name()) // update matches, without rerendering them
                {
                    int prevNum = Convert.ToInt16(slot.GetChild(0).Find("Number").GetComponent<Text>().text);
                    // mark the firstAddedItem
                    if(prevNum < showItems[i].Num() && firstAddedItemIndex == -1)
                        firstAddedItemIndex = i;
                    // Accent the number change
                    if(prevNum != showItems[i].Num())
                        StartCoroutine(accentScript.Accent(slot.GetChild(0).Find("Number")));
                    slot.GetChild(0).Find("Number").GetComponent<Text>().text = "" + showItems[i].Num();
                    // slot.SetSiblingIndex(i);
                    ShowHideDeleteBtn(slot.gameObject);
                    foundMatch = true;
                    break;
                }
            }
            if(!foundMatch) //instantiate a new slot
            {
                if(firstAddedItemIndex == -1)
                    firstAddedItemIndex = i;
                GameObject newSlot = InstantiateSlot(showItems[i]);
                newSlot.transform.SetSiblingIndex(i);
            }
        }

        // change scroll height after updating UI
        if(firstAddedItemIndex == -1 || MenuAppear.isMenu)
            StartCoroutine(ScrollHeightRoutine(remainScrollPosition, lastDestoryCOR));
        else
            StartCoroutine(ScrollHeightRoutine(firstAddedItemIndex, lastDestoryCOR));
        
    }

    GameObject InstantiateSlot(Item item)
    {
        GameObject slot = Instantiate(slotPrefab, itemsParent);
        Transform ItemButton = slot.transform.Find("ItemButton");
        // change Name
        ItemButton.Find("Text").GetComponent<Text>().text = item.Name();
        // change Number
        ItemButton.Find("Number").GetComponent<Text>().text = "" + item.Num();
        // set "Value"
        ItemButton.Find("Price").GetComponent<Text>().text = "" + item.SellPrice();
        // change Sprite
        ItemButton.Find("Item").GetComponent<Image>().sprite = item.Icon();
        // change "Item" to the new name
        ItemButton.Find("Item").name = item.Name();
        // determine whether Show or hide the delete button
        ShowHideDeleteBtn(slot);

        // animate: zoom in
        StartCoroutine(zoomScript.Zoom(slot.transform, true));

        return slot;
    }

    IEnumerator ScrollHeightRoutine(bool remainScrollPosition, Coroutine waitTillAfter)
    {
        if(waitTillAfter != null)
            yield return waitTillAfter;
        yield return null;
        ScrollHeight cScript = GetComponent<ScrollHeight>();
        cScript.UpdateHeight(itemsParent, remainScrollPosition);
    }

    IEnumerator ScrollHeightRoutine(int firstAddedItemIndex, Coroutine waitTillAfter)
    {
        if(waitTillAfter != null)
            yield return waitTillAfter;
        yield return null;
        ScrollHeight cScript = GetComponent<ScrollHeight>();
        cScript.UpdateHeight(itemsParent, firstAddedItemIndex);
    }
}
