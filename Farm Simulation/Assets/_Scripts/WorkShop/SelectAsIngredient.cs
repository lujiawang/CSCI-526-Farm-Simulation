using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAsIngredient : MonoBehaviour
{
	GameObject motherInventory;

	Inventory inventory;
	IngredientsInventoryUI uiScript;
	bool enableSelect = false;

    void Start()
    {
    	motherInventory = this.transform.parent.parent.parent.parent.gameObject;
    	if(motherInventory.name == "Ingredients" || motherInventory.name == "Inventory")
    		enableSelect = true;
    	if(GameObject.Find("Inventory") != null)
    		inventory = Inventory.instance;
    }

    public void Select()
    {
    	if(GameObject.Find("Ingredients") != null)
    		uiScript = GameObject.Find("Ingredients").GetComponent<IngredientsInventoryUI>();
    	if(!enableSelect || uiScript == null || inventory == null || 
    		!uiScript.GetComponent<CanvasGroup>().blocksRaycasts )
    		return;
    	string name = this.transform.Find("Text").GetComponent<Text>().text;
    	if(motherInventory.name == "Ingredients") //Deselect item from ingredients
    	{
    		uiScript.RemoveItem(name, this.transform.parent.gameObject);
            // change tab if necessary
            InventoryUI cScript = GameObject.Find("Inventory").GetComponent<InventoryUI>();
            cScript.ToggleRespectiveShowButton(Item.GetItemId(name));

    		inventory.Add(name, 1);

    		
    	}else //Select item from inventory as ingredient
    	{
    		if(uiScript.AddItem(name))
    			inventory.Add(name, -1);
    	}
    }
}
