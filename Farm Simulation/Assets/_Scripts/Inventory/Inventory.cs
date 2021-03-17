using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    #region Singleton
    // Use "Inventory.instance" to access the Inventory instance and perform Add/Remove function

    public static Inventory instance;

    public int stackLimit;

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



    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    SoundManager soundManager;

    // subscribe any method to this callback to notify self of changes made in inventory

    public List<Item> items = new List<Item>();

    void Start()
    {
        soundManager = SoundManager.instance;
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

    public static Sprite GetCropSprite(string name)
    {
        Sprite[] icons = Resources.LoadAll<Sprite>("Crop_Spritesheet");
        bool seed = name.Contains("Seed");
    	string tempName = name;
        if (seed)
            tempName = name.Remove(name.IndexOf("Seed"), "Seed".Length);
        else
            tempName = name;//.Remove(name.IndexOf("Fruit"), "Fruit".Length);
    	// Debug.Log(tempName);
        foreach (Sprite i in icons)
        {
        	if(seed)
        	{
        		if(tempName + "_0" == i.name)
            		return i;
        	}
            else
            {
        		if(tempName + "_5" == i.name)
            		return i;
            }
            
        }
        return null;
    }

    // num can be negative to perform remove
    public void Add(string name, int num)
    {
        Item item = new Item();
        item.SetName(name);
        item.SetNum(num);
        Sprite icon = GetCropSprite(name);
        item.SetIcon(icon);
        int count = items.Count;

        if (count == 0)
        {
            items.Add(item);
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
                    }
                    else if (items[i].Num() != stackLimit)
                    {
                        items[i].SetNum(stackLimit);
                    }
                    else
                    {
                        return;
                    }
                    // Remove item if number is reduced to 0 or below 0
                    if (items[i].Num() <= 0)
                    {
                        items.RemoveAt(i);
                        // Debug.Log("reduced successfully");
                    }
                    break;
                }

                if (i == count - 1)
                {
                    items.Add(item);
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
