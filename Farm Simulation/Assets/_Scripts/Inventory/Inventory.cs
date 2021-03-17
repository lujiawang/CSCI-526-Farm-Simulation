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
		if(instance != null)
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
		Add("Corn", 1);
		Add("Cucumber", 5);
		Add("Cucumber", 5);
		Add("Avocado", 5);
		// Add("Cassava", 5);
		// Add("Coffee", 5);
		// Add("Eggplant", 5);
		// Add("Grapes", 5);
		// Add("Lemon", 5);
		Add("Melon", 5);
	}

	public static Sprite GetSeedSprite(string name)
	{
		Sprite[] icons = Resources.LoadAll<Sprite>("Crop_Spritesheet");
		foreach(Sprite i in icons)
		{
			if(i.name == name + "_0")
				return i;
		}
		return null;
	}

	// num can be negative to perform remove
	public void Add(string name, int num)
	{
		Item item = new Item(); 
		item.SetName(name); 
		item.SetNum(num);
		Sprite icon = GetSeedSprite(name);
		item.SetIcon(icon);
		int count = items.Count;

		if(count == 0)
		{
			items.Add(item);
		}
		else
		{
			for(int i = 0; i < count; i++)
			{

				if(items[i].Name() == name)
				{
					// update only if updated num of item <= stackLimit
					if(num + items[i].Num() <= stackLimit){
						items[i].AddNum(num);
					}else if(items[i].Num() != stackLimit){
						items[i].SetNum(stackLimit);
					}else{
						return;
					}
					// Remove item if number is reduced to 0 or below 0
					if(items[i].Num() <= 0)
					{
						items.RemoveAt(i);
						// Debug.Log("reduced successfully");
					}
					break;
				}

				if(i == count-1)
				{
					items.Add(item);
					break;
				}
			}
		}

		// play sound effect
		if(num > 0)
		{
			soundManager.PlaySound("AddItemSound");
		}else
		{
			soundManager.PlaySound("RemoveItemSound");
			// Debug.Log("Play remove sound");
		}

		// Debug.Log(items.Count);

		
		if(onItemChangedCallback != null){
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
