using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class IngredientsInventoryUI : MonoBehaviour
{
	public Transform itemsParent;

	public GameObject slotPrefab;

	public List<Item> ingredients = new List<Item>();
	public static int ingredientUpperLimit = 3;
	public static int ingredientLowerLimit = 2;

	ShowToast showToastScript;

    // Start is called before the first frame update
    void Start()
    {
    	if(GameObject.Find("Canvas") != null)
    		showToastScript = GameObject.Find("Canvas").GetComponent<ShowToast>();
    	if(PlayerPrefs.HasKey("ingredientsInventoryIndex"))
    	{
    		string[] indexArray = PlayerPrefs.GetString("ingredientsInventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
    		foreach(string name in indexArray)
    		{
    			AddItem(name);
    		}
    	}
    }

    void OnDestroy()
    {
    	SaveIngredients();
    }

    public void SaveIngredients()
    {
    	string ingredientsStr = "";
    	foreach(Item ingredient in ingredients)
    	{
    		ingredientsStr += ingredient.Name() + " ";
    	}
    	PlayerPrefs.SetString("ingredientsInventoryIndex", ingredientsStr);
    }

    public bool AddItem(string name)
    {
    	if(ingredients.Count >= ingredientUpperLimit)
    	{
    		showToastScript.showToast("Cannot add more than " + ingredientUpperLimit + " ingredients!", 1);
    		return false;
    	}
    	else if(Item.GetItemId(name) <= Item.seedIdUpperLimit || Item.GetItemId(name) > Item.harvestIdUpperLimit){
    		showToastScript.showToast("Only harvested crops can be added as ingredients!", 1);
    		return false;
    	}
    	foreach(Item ingredient in ingredients)
    	{
    		if(ingredient.Name() == name)
    		{
    			showToastScript.showToast("You have already added this ingredient!", 1);
	    		return false;
    		}
    	}
    	Item newItem = new Item(name, 1);
    	ingredients.Add(newItem);

    	InstantiateSlot(newItem);
    	return true;
    }

    public void RemoveItem(string name, GameObject slot)
    {
    	for(int i = 0; i < ingredients.Count; i++)
    	{
    		if(name == ingredients[i].Name())
    		{
    			ingredients.RemoveAt(i);
    			// Debug.Log("Removed. The rest ingredients: " + ingredients.Count);
    			Destroy(slot);	
    			return;
    		}
    	}
    }

    public void DestroyAll()
    {
    	foreach(Transform slot in itemsParent)
    	{
    		Destroy(slot.gameObject);
    	}
    	ingredients = new List<Item>();
    }

    void InstantiateSlot(Item item)
    {
    	GameObject slot = Instantiate(slotPrefab, itemsParent);
    	Transform ItemButton = slot.transform.Find("ItemButton");
    	// change Name
        ItemButton.Find("Text").GetComponent<Text>().text = item.Name();
        // change Number
    	ItemButton.Find("Number").GetComponent<Text>().text = "" + item.Num();
        // set "Value"
        ItemButton.Find("Price").GetComponent<Text>().text = "" + item.SellPrice();
        // disable DragAndDrop
        ItemButton.Find("Item").GetComponent<DragAndDrop>().enabled = false;
        // change Sprite
        ItemButton.Find("Item").GetComponent<Image>().sprite = item.Icon();
        // change "Item" to the new name
        ItemButton.Find("Item").gameObject.name = item.Name();
    }

}
