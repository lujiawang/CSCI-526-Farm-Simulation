using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConsumeFood : MonoBehaviour
{
	GameObject motherInventory;
	FoodEffect effectScript; //on Canvas

	Inventory inventory;
    SoundManager soundManager;
	bool enableConsume = false;

    void Start()
    {
    	motherInventory = this.transform.parent.parent.parent.parent.gameObject;
    	if(motherInventory.name == "Inventory")
    	{
    		enableConsume = true;
    		effectScript = motherInventory.transform.parent.GetComponent<FoodEffect>();
    		inventory = Inventory.instance;
            soundManager = SoundManager.instance;
    	}
    }

    public void Consume()
    {
    	if(!enableConsume || inventory == null || StoreToggle.isStoreOpen
    		|| SceneManager.GetActiveScene().name != "Farming_01_main")
    		return;
    	string name = this.transform.Find("Text").GetComponent<Text>().text;
    	int id = Item.GetItemId(name);
    	if(id > Item.harvestIdUpperLimit && id <= Item.foodIdUpperLimit) //is food
    	{
    		id -= Item.harvestIdUpperLimit;
    		switch(id)
    		{
    			case 1: //FruitSalads
    				effectScript.FastForwardGrow(name);
    				inventory.Add(name, -1);
                    soundManager.PlaySound(15);
    				break;
    			case 2: //Cornsuccotash
    				effectScript.HigherHarvest(name);
    				inventory.Add(name, -1);
                    soundManager.PlaySound(15);
    				break;
    			default:
    				break;
    		}
    	}
    }

}
