using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Inventory : MonoBehaviour
{
    #region Singleton
    // Use "Inventory.instance" to access the Inventory instance and perform Add/Remove function

    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("> 1 Inventory instance found");
            return;
        }
        instance = this;
    }
    #endregion

    public static int stackLimit = 99; //stack limit of items

    public delegate void OnItemChanged(bool remainScrollPosition, bool doDestroyAll);
    public OnItemChanged onItemChangedCallback;

    // Define how many things you want to add to inventory at the beginning.
    [Range(1,20)]
    public int StarterPackageSize;

    // Whether restart the inventory when restarting game
    [SerializeField]
    private bool resetInventoryAfterRestart = false;

    SoundManager soundManager;

    public List<Item> items = new List<Item>();

    void Start()
    {
    	// must initialize this first
        soundManager = SoundManager.instance;
        if(resetInventoryAfterRestart)
    	{
    		PlayerPrefs.DeleteKey("inventoryIndex");
    	}

        string[] indexArray = new string[0];
        // if inventoryIndex is already set
        if(PlayerPrefs.HasKey("inventoryIndex"))
        	indexArray = PlayerPrefs.GetString("inventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
        // else, set inventoryIndex and add starter package of seeds
        else
        {
        	PlayerPrefs.SetString("inventoryIndex","");

        	// add starter crops seeds to player's inventory
        	AddStarterPackage(StarterPackageSize);
        	
        }

        if(indexArray.Length > 0)
        {
        	InitializeInventory(indexArray);
        }
        
    }

    // OnDestroy is called before exiting the game
    void OnDestroy()
    {
    	SaveInventory();
    }

    public void SaveInventory()
    {
    	string index = "";
    	foreach(Item item in items)
    	{
    		index += " " + item.Name();
    		// Debug.Log("name: "+item.Name());
    		PlayerPrefs.SetInt(item.Name(), item.Num());
    	}
    	PlayerPrefs.SetString("inventoryIndex", index);
    	// Debug.Log("index: "+index);
    }

    void AddStarterPackage(int StarterPackageSize)
    {
    	for(int i = 0; i < StarterPackageSize; i++)
    	{
    		Item newItem = new Item();
    		newItem.SetAllFields(Item.GetCropName(i), 5);
    		items.Add(newItem);
    		newItem = new Item();
    		newItem.SetAllFields(Item.GetCropName(i+20), 5);
    		items.Add(newItem);
    	}
    	if (onItemChangedCallback != null)
        {
            // StartCoroutine(onItemChangedCallback.Invoke());
            onItemChangedCallback.Invoke(false, true);
        }
    }

    void InitializeInventory(string[] indexArray)
    {
        foreach(string itemName in indexArray)
        {
        	// Debug.Log("Name + Int: "+ itemName+" "+PlayerPrefs.GetInt(itemName));
        	Item item = new Item();
	        item.SetAllFields(itemName, PlayerPrefs.GetInt(itemName));

        	items.Add(item);
        }

        if (onItemChangedCallback != null)
        {
            // StartCoroutine(onItemChangedCallback.Invoke());
            onItemChangedCallback.Invoke(false, true);
        }
    }

    // num can be negative to perform remove
    public void Add(string name, int num)
    {
    	if(num == 0)
    		return;

        Item item = new Item();
        item.SetAllFields(name, num);

        int count = items.Count;
        if (count == 0)
        {
            items.Add(item);
            if (onItemChangedCallback != null)
	        {
	            onItemChangedCallback.Invoke(true, true);
	        }
            // SaveAddItem(name, num);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {

                if (items[i].Name().Equals(item.Name()))
                {
                    // update only if updated num of item <= stackLimit
                    if (num + items[i].Num() <= stackLimit)
                    {
                        items[i].AddNum(num);
                        if (onItemChangedCallback != null)
				        {
				            onItemChangedCallback.Invoke(true, false);
				        }
                        // SaveUpdateItem(name, num);
                    }
                    else if (items[i].Num() != stackLimit)
                    {
                        items[i].SetNum(stackLimit);
                        if (onItemChangedCallback != null)
				        {
				            onItemChangedCallback.Invoke(true, false);
				        }
                        // SaveUpdateItem(name, num);
                    }
                    else
                    {
                        return;
                    }
                    // Remove item if number is reduced to 0 or below 0
                    if (items[i].Num() <= 0)
                    {
                        items.RemoveAt(i);
                        if (onItemChangedCallback != null)
				        {
				            onItemChangedCallback.Invoke(true, true);
				        }
                        // SaveRemoveItem(name);
                        // Debug.Log("reduced successfully");
                    }

                    break;
                }

                if (i == count - 1)
                {
                    items.Add(item);
                    if (onItemChangedCallback != null)
			        {
			            onItemChangedCallback.Invoke(true, true);
			        }
                    // SaveAddItem(name, num);
                    break;
                }
            }
        }

        // play sound effect
        if (num > 0)
        {
            //soundManager.PlaySound("add");
        }
        else
        {
            soundManager.PlaySound("remove");
            // Debug.Log("Play remove sound");
        }

        // Debug.Log(items.Count);

    }


    // public void Remove(Item item)
    // {
    // 	items.Remove(item);
    // 	if(onItemChangedCallback != null){
    // 		onItemChangedCallback.Invoke();
    // 	}
    // }


}
