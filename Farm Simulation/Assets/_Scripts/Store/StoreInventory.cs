using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StoreInventory : MonoBehaviour
{
    #region Singleton
    // Use "Inventory.instance" to access the Inventory instance and perform Add/Remove function

    public static StoreInventory instance;

    void Awake()
    {
        if (instance != null)
        {
            // Debug.LogWarning("> 1 StoreInventory instance found");
            return;
        }
        instance = this;
    }
    #endregion


    public static int stackLimit = 99;
    public static int randomizeItemNumLimit = 10; // when randomizing, the maximum number of crops that can be generated
    public static int randomizeItemTypesUpperLimit = 40; //how many types of items could be generated, used for randomizing store    
    [Range(1, 40)]
    public int randomizeItemTypes = 30; //5 // when randomizing, how many types of crops that can be generated

    public delegate void OnStoreItemChanged(bool remainScrollPosition, bool doDestroyAll);
    public OnStoreItemChanged onStoreItemChangedCallback;
    // subscribe any method to this callback to notify self of changes made in inventory

    SoundManager soundManager;

    InternetTime internetTime;

    // day, hour, minute, second
    [Range(10,100)]
    public double updateIntervalSecs = 10d;
    private TimeSpan storeUpdateInterval; // how much time before store update

    public List<Item> items = new List<Item>();

    void Start()
    {
    	// must initialize this first
        soundManager = SoundManager.instance;
        internetTime = InternetTime.instance;
        internetTime.onTimeChangedCallback += UpdateStore;

        storeUpdateInterval = TimeSpan.FromSeconds(updateIntervalSecs);

        string[] indexArray = new string[0];
        // if inventoryIndex is already set
        if(PlayerPrefs.HasKey("storeInventoryIndex"))
        	indexArray = PlayerPrefs.GetString("storeInventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
        // else, set inventoryIndex and add starter package of seeds
        else
        {
        	PlayerPrefs.SetString("storeInventoryIndex","");

        	// randomize the store
        	RandomizeStore();
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
    	// PlayerPrefs.DeleteAll();
    }

    void RandomizeStore()
    {
    	System.Random rand = new System.Random();
    	items = new List<Item>(); //clear out items list
    	List<int> ids = new List<int>(); //store all generated ids
    	for (int i = 0; i < randomizeItemTypes; i++)
    	{
    		int id = -1;
    		// loop until generated a unique id number
    		do{
    			id = rand.Next(randomizeItemTypesUpperLimit); //generate integer between 0 - randomizeItemTypesUpperLimit
    		}while(ids.Contains(id));
    		ids.Add(id);

    		int num = rand.Next(1, randomizeItemNumLimit+1);
    		items.Add(new Item(id, num));

    		// rand = new Random();
    	}
    	if (onStoreItemChangedCallback != null)
        {
            // StartCoroutine(onStoreItemChangedCallback.Invoke(false));
            onStoreItemChangedCallback.Invoke(false, true);
        }
        if(StoreToggle.isStoreOpen)
            soundManager.PlaySound(5);
    	
    }

    public void SaveInventory()
    {
    	string index = "";
    	foreach(Item item in items)
    	{
    		index += " " + item.Name();
    		PlayerPrefs.SetInt(item.Name()+"Store", item.Num());
    	}
    	PlayerPrefs.SetString("storeInventoryIndex", index);
    }

    void InitializeInventory(string[] indexArray)
    {
        foreach(string itemName in indexArray)
        {
        	Item item = new Item(itemName, PlayerPrefs.GetInt(itemName + "Store"));
        	items.Add(item);
        }

        if (onStoreItemChangedCallback != null)
        {
            // StartCoroutine(onStoreItemChangedCallback.Invoke());
            onStoreItemChangedCallback.Invoke(false, true);
        }
    }

    // update the store inventory when certiain amoutn of time passes
    void UpdateStore()
    {
        // Debug.Log("updatestore");
    	// if the game continues from last save
    	if(PlayerPrefs.HasKey("prevTimeStoreUpdate"))
    	{
    		DateTime prevTime = DateTime.Parse(PlayerPrefs.GetString("prevTimeStoreUpdate"));
    		// check if time elapsed is greater than update interval
    		TimeSpan diffTime = internetTime.GetTime().Subtract(prevTime);
    		// Debug.Log(diffTime);
    		if(TimeSpan.Compare(diffTime, storeUpdateInterval) >= 0)
    		{
    			PlayerPrefs.SetString("prevTimeStoreUpdate", internetTime.GetTime().ToString());
    			RandomizeStore();
    		}    		
    	}
    	else //if is new game
    	{
    		PlayerPrefs.SetString("prevTimeStoreUpdate", internetTime.GetTime().ToString());
    		RandomizeStore();
    	}
    }

    public void Add(string name, int num, bool playDefaultSound)
    {
        if(num == 0)
            return;

        Item item = new Item(name, num>stackLimit?stackLimit:num);

        int count = items.Count;
        if (count == 0 && num > 0)
        {
            items.Add(item);
            if (onStoreItemChangedCallback != null)
            {
                onStoreItemChangedCallback.Invoke(true, true);
            }
            // Play sounds
            if(playDefaultSound)
                soundManager.PlaySound(4);

            return;
        }
        for (int i = 0; i < count; i++)
        {
            if (items[i].Name().Equals(item.Name()))
            {
                // update only if updated num of item <= stackLimit
                if (num + items[i].Num() <= stackLimit)
                {
                    items[i].AddNum(num);
                    // Remove item if number is reduced to 0 or below 0
                    if (items[i].Num() <= 0)
                    {
                        items.RemoveAt(i);
                        if (onStoreItemChangedCallback != null)
                        {
                            onStoreItemChangedCallback.Invoke(true, true);
                        }
                        // Play sounds
                        if(playDefaultSound)
                            soundManager.PlaySound(3);
                        return;
                    }
                    if (onStoreItemChangedCallback != null)
                    {
                        onStoreItemChangedCallback.Invoke(true, false);
                    }
                    // Play sounds
                    if(playDefaultSound)
                        soundManager.PlaySound(num>0?4:3);
                }
                else if (items[i].Num() < stackLimit) // added total is over stackLimit
                {
                    items[i].SetNum(stackLimit);
                    if (onStoreItemChangedCallback != null)
                    {
                        onStoreItemChangedCallback.Invoke(true, false);
                    }
                    // Play sounds
                    if(playDefaultSound)
                        soundManager.PlaySound(4);
                }
                else // original number of item is already at stackLimit
                {
                    if(playDefaultSound)
                        soundManager.PlaySound(4);
                    return;
                }

                return;
            }

            if (i == count - 1 && num > 0)
            {
                items.Add(item);
                if (onStoreItemChangedCallback != null)
                {
                    onStoreItemChangedCallback.Invoke(true, true);
                }
                if(playDefaultSound)
                    soundManager.PlaySound(4);
                break;
            }
        }

    }

    public void Add(string name, int num)
    {
        if(num == 0)
            return;

        Item item = new Item(name, num>stackLimit?stackLimit:num);

        int count = items.Count;
        if (count == 0 && num > 0)
        {
            items.Add(item);
            if (onStoreItemChangedCallback != null)
            {
                onStoreItemChangedCallback.Invoke(true, true);
            }
            // Play sounds
            soundManager.PlaySound(4);

            return;
        }
        for (int i = 0; i < count; i++)
        {
            if (items[i].Name().Equals(item.Name()))
            {
                // update only if updated num of item <= stackLimit
                if (num + items[i].Num() <= stackLimit)
                {
                    items[i].AddNum(num);
                    // Remove item if number is reduced to 0 or below 0
                    if (items[i].Num() <= 0)
                    {
                        items.RemoveAt(i);
                        if (onStoreItemChangedCallback != null)
                        {
                            onStoreItemChangedCallback.Invoke(true, true);
                        }
                        // Play sounds
                        soundManager.PlaySound(3);
                        return;
                    }
                    if (onStoreItemChangedCallback != null)
                    {
                        onStoreItemChangedCallback.Invoke(true, false);
                    }
                    // Play sounds
                    soundManager.PlaySound(num>0?4:3);
                }
                else if (items[i].Num() < stackLimit) // added total is over stackLimit
                {
                    items[i].SetNum(stackLimit);
                    if (onStoreItemChangedCallback != null)
                    {
                        onStoreItemChangedCallback.Invoke(true, false);
                    }
                    // Play sounds
                    soundManager.PlaySound(4);
                }
                else // original number of item is already at stackLimit
                {
                    soundManager.PlaySound(4);
                    return;
                }

                return;
            }

            if (i == count - 1 && num > 0)
            {
                items.Add(item);
                if (onStoreItemChangedCallback != null)
                {
                    onStoreItemChangedCallback.Invoke(true, true);
                }
                soundManager.PlaySound(4);
                break;
            }
        }

    }


}
