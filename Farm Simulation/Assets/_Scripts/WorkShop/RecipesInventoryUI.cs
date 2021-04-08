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

    ZoomObj zoomScript;

    // Start is called before the first frame update
    void Start()
    {
        zoomScript = GetComponentInParent<Canvas>().rootCanvas.GetComponent<ZoomObj>();
    	if(resetRecipes)
    		PlayerPrefs.DeleteKey("recipesInventoryIndex");
    	// PlayerPrefs.SetString("recipesInventoryIndex", "FruitSalads CornSuccotash EggplantSoup CucumBurger TurnipRamen TomatoSandwich VeggieKebab Salmagundi VeggieRisotto Hodgepodge");
    	// PlayerPrefs.SetString("recipesInventoryIndex", "FruitSalads CornSuccotash EggplantSoup CucumBurger");
    	// PlayerPrefs.SetString("recipesInventoryIndex", "FruitSalads CornSuccotash");
    	UpdateUI(false);
    }

    // PARAM remainScrollPosition indicates whether to maintain previous scrollView position
    public void UpdateUI(bool remainScrollPosition)
    {
    	if(!PlayerPrefs.HasKey("recipesInventoryIndex") || PlayerPrefs.GetString("recipesInventoryIndex").Length <= 0)
    		return;
    	string[] indexArray = PlayerPrefs.GetString("recipesInventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
    	List<Recipe> recipes = new List<Recipe>();
    	foreach(string name in indexArray)
    	{
    		Recipe recipe = new Recipe(name);
    		recipes.Add(recipe);
    	}
    	recipes.Sort();

        Coroutine lastDestoryCOR = null;

        // Delete recipes that are not in recipes list
        for(int i = 0; i < itemsParent.childCount; i += 2)
        {
            string name = itemsParent.GetChild(i).GetChild(0).Find("Text").GetComponent<Text>().text;
            bool doDestroy = true;
            foreach(Recipe recipe in recipes)
            {
                if(name == recipe.Name())
                {
                    doDestroy = false;
                    break;
                }
            }
            if(doDestroy)
            {
                StartCoroutine(zoomScript.Zoom(itemsParent.GetChild(i), false));
                lastDestoryCOR = StartCoroutine(zoomScript.Zoom(itemsParent.GetChild(i+1), false));
            }
        }

        int firstAddedRecipeIndex = -1;
        // Add/Update recipes
        for(int i = 0; i < recipes.Count; i++)
        {
            bool foundMatch = false;
            for(int j = 0; j < itemsParent.childCount; j += 2)
            {
                string name = itemsParent.GetChild(j).GetChild(0).Find("Text").GetComponent<Text>().text;
                if(name == recipes[i].Name()) // skip the child
                {
                    foundMatch = true;
                    break;
                }
            }
            if(!foundMatch) //instantiate a new recipe and its bar
            {
                GameObject newSlot = InstantiateSlot(new Item(recipes[i].Name(), 0), itemsParent, false);
                newSlot.transform.SetSiblingIndex(i * 2);
                GameObject newBar = InstantiateBar(recipes[i]);
                newBar.transform.SetSiblingIndex(i * 2 + 1);

                if(firstAddedRecipeIndex == -1)
                    firstAddedRecipeIndex = i * 2;
            }
        }

        // change scroll height after updating UI
        if(firstAddedRecipeIndex == -1)
            StartCoroutine(ScrollHeightRoutine(remainScrollPosition, lastDestoryCOR));
        else
            StartCoroutine(ScrollHeightRoutine(firstAddedRecipeIndex, lastDestoryCOR));
    }

    GameObject InstantiateBar(Recipe recipe)
    {
		GameObject bar = Instantiate(recipeBarPrefab, itemsParent);
		foreach(string name in recipe.Ingredients())
		{
			Item item = new Item(name, 0);
			InstantiateSlot(item, bar.transform, true);
		}

        // animate: zoom in
        StartCoroutine(zoomScript.Zoom(bar.transform, true));

        return bar;
    }

    GameObject InstantiateSlot(Item item, Transform parent, bool belongsToBar)
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

    IEnumerator ScrollHeightRoutine(int jumpToItemNumber, Coroutine waitTillAfter)
    {
        if(waitTillAfter != null)
            yield return waitTillAfter;
        yield return null;
        ScrollHeight cScript = GetComponent<ScrollHeight>();
        cScript.UpdateHeight(itemsParent, jumpToItemNumber);
    }
}
