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

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    SoundManager soundManager;

    // subscribe any method to this callback to notify self of changes made in inventory

    public List<Item> items = new List<Item>();

    void Start()
    {
    	// must initialize this first
        soundManager = SoundManager.instance;

        string[] indexArray = new string[0];
        // if inventoryIndex is already set
        if(PlayerPrefs.HasKey("inventoryIndex"))
        	indexArray = PlayerPrefs.GetString("inventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
        // else, set inventoryIndex and add starter package of seeds
        else
        {
        	PlayerPrefs.SetString("inventoryIndex","");

        	// add starter crops seeds to player's inventory
        	Add("CornSeed", 1);
	        Add("CucumberSeed", 5);
	        Add("CucumberSeed", 5);
	        Add("AvocadoSeed", 5);
	        // Add("CassavaSeed", 5);
	        // Add("CoffeeSeed", 5);
	        // Add("EggplantSeed", 5);
	        // Add("GrapesSeed", 5);
	        // Add("LemonSeed", 5);
	        Add("MelonSeed", 5);
        }

        if(indexArray.Length > 0)
        {
        	InitializeInventory(indexArray);
        }
        
    }

    // OnDestroy is called before exiting the game
    void OnDestroy()
    {
    	// PlayerPrefs.DeleteAll();
    	SaveInventory();
    }

    public void SaveInventory()
    {
    	string index = "";
    	foreach(Item item in items)
    	{
    		index += " " + item.Name();
    		PlayerPrefs.SetInt(item.Name(), item.Num());
    	}
    	PlayerPrefs.SetString("inventoryIndex", index);
    }

    // private void SaveAddItem(string name, int num)
    // {
    // 	string index = PlayerPrefs.GetString("inventoryIndex");
    // 	index += " " + name;
    // 	PlayerPrefs.SetString("inventoryIndex", index);
    // 	PlayerPrefs.SetInt(name, num);
    // }

    // private void SaveUpdateItem(string name, int num)
    // {
    // 	PlayerPrefs.SetInt(name, num + PlayerPrefs.GetInt(name));
    // }

    // private void SaveRemoveItem(string name)
    // {
    // 	string index = "";
    // 	string[] indexArray = PlayerPrefs.GetString("inventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
    // 	foreach(string i in indexArray)
    // 	{
    // 		if(i != name)
    // 			index += " " + i;
    // 	}
    // 	PlayerPrefs.SetString("inventoryIndex", index);
    // 	PlayerPrefs.DeleteKey(name);
    // }

    public void InitializeInventory(string[] indexArray)
    {
        foreach(string itemName in indexArray)
        {
        	Item item = new Item();
	        item.SetAllFields(itemName, PlayerPrefs.GetInt(itemName));

        	items.Add(item);
        }

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
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
                        // SaveUpdateItem(name, num);
                    }
                    else if (items[i].Num() != stackLimit)
                    {
                        items[i].SetNum(stackLimit);
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
                        // SaveRemoveItem(name);
                        // Debug.Log("reduced successfully");
                    }

                    break;
                }

                if (i == count - 1)
                {
                    items.Add(item);
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


        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

    }


    // public void Remove(Item item)
    // {
    // 	items.Remove(item);
    // 	if(onItemChangedCallback != null){
    // 		onItemChangedCallback.Invoke();
    // 	}
    // }


}
