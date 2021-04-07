using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class RecipesInventoryUI : MonoBehaviour
{
	public Transform itemsParent;

	public GameObject slotPrefab;
	public GameObject recipeBarPrefab;

	[SerializeField]
	private bool resetRecipes = false;

    // Start is called before the first frame update
    void Start()
    {
    	if(resetRecipes)
    		PlayerPrefs.DeleteKey("recipesInventoryIndex");
    	PlayerPrefs.SetString("recipesInventoryIndex", "FruitSalads CornSuccotash EggplantSoup CucumBurger TurnipRamen TomatoSandwich VeggieKebab Salmagundi VeggieRisotto Hodgepodge");
    	// PlayerPrefs.SetString("recipesInventoryIndex", "FruitSalads CornSuccotash EggplantSoup CucumBurger");
    	// PlayerPrefs.SetString("recipesInventoryIndex", "FruitSalads CornSuccotash");
    	UpdateUI(false);
    }

    // PARAM remainScrollPosition indicates whether to maintain previous scrollView position
    void UpdateUI(bool remainScrollPosition)
    {
    	if(!PlayerPrefs.HasKey("recipesInventoryIndex"))
    		return;
    	string[] indexArray = PlayerPrefs.GetString("recipesInventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
    	List<Recipe> recipes = new List<Recipe>();
    	foreach(string name in indexArray)
    	{
    		Recipe recipe = new Recipe();
    		recipe.SetAllFields(name);
    		recipes.Add(recipe);
    	}
    	recipes.Sort();

    	foreach(Transform child in itemsParent)
    	{
    		Destroy(child.gameObject);
    	}

		foreach(Recipe recipe in recipes)
		{
			Item item = new Item();
			item.SetAllFields(recipe.Name(), 0);
			InstantiateSlot(item, itemsParent, false);

			// instantiate the corresponding recipe bar
			InstantiateBar(recipe);
		}

        // change scroll height after updating UI
        StartCoroutine(ScrollHeightRoutine(remainScrollPosition));
    }

    void InstantiateBar(Recipe recipe)
    {
		GameObject bar = Instantiate(recipeBarPrefab, itemsParent);
		foreach(string name in recipe.Ingredients())
		{
			Item item = new Item();
			item.SetAllFields(name, 0);
			InstantiateSlot(item, bar.transform, true);
		}
    }

    void InstantiateSlot(Item item, Transform parent, bool belongsToBar)
    {
    	GameObject slot = Instantiate(slotPrefab, parent);
    	Transform ItemButton = slot.transform.Find("ItemButton");
    	// change Name
        ItemButton.Find("Text").GetComponent<Text>().text = item.Name();
        // change/disable Number
        if(item.Num() > 0)
	    	ItemButton.Find("Number").GetComponent<Text>().text = "" + item.Num();
	    else
	    	ItemButton.Find("Number").GetComponent<Text>().text = "";
        // set "Value"
        ItemButton.Find("Price").GetComponent<Text>().text = "" + item.SellPrice();
        // change Sprite
        ItemButton.Find("Item").GetComponent<Image>().sprite = item.Icon();
        // disable DragAndDrop
        ItemButton.Find("Item").GetComponent<DragAndDrop>().enabled = false;
        // change "Item" to the new name
        ItemButton.Find("Item").name = item.Name();

        if(!belongsToBar) // is recipe item
        {
        	ItemButton.Find("EqualSign").GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    IEnumerator ScrollHeightRoutine(bool remainScrollPosition)
    {
        yield return null;
        ScrollHeight cScript = GetComponent<ScrollHeight>();
        cScript.UpdateHeight(itemsParent, remainScrollPosition);
    }
}
