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
    	UpdateUI(false);
    }

    // PARAM remainScrollPosition indicates whether to maintain previous scrollView position
    void UpdateUI(bool remainScrollPosition)
    {
    	if(!PlayerPrefs.HasKey("recipesInventoryIndex"))
    		return;
    	string[] indexArray = new string[0];
    	indexArray = PlayerPrefs.GetString("recipesInventoryIndex").Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
    	List<Recipe> recipes = new List<Recipe>();
    	foreach(string name in indexArray)
    	{
    		Recipe recipe = new Recipe();
    		recipe.SetAllFields(name);
    		recipes.Add(recipe);
    	}

    	foreach(Transform child in itemsParent)
    	{
    		Destroy(child.gameObject);
    	}

		recipes.Sort();

		foreach(Recipe recipe in recipes)
		{
			GameObject newSlot = Instantiate(slotPrefab, itemsParent) as GameObject;
			Transform ItemButton = newSlot.transform.Find("ItemButton");
			// instantiate the corresponding recipe bar
			InstantiateBar(recipe);
		}

        // for(int i = 0; i < itemsCount; i++)
        // {
        //     GameObject newSlot = Instantiate(slotPrefab, itemsParent) as GameObject;
        //     if(newSlot == null){
        //         Debug.LogWarning("NULL");
        //     }
        //     Transform ItemButton = newSlot.transform.Find("ItemButton");
        //     // change Name
        //     ItemButton.Find("Text").gameObject.GetComponent<Text>().text = inventory.items[i].Name();
        //     // change Number
        //     ItemButton.Find("Number").gameObject.GetComponent<Text>().text = "" + inventory.items[i].Num();
        //     // set "Value"
        //     ItemButton.Find("Price").gameObject.GetComponent<Text>().text = "" + inventory.items[i].SellPrice();
        //     // change Sprite
        //     ItemButton.Find("Item").gameObject.GetComponent<Image>().sprite = inventory.items[i].Icon();
        //     // change "Item" to the new name
        //     ItemButton.Find("Item").gameObject.name = inventory.items[i].Name();

        //     // determine whether Show or hide the slot based on showParams
        //     ShowHide(newSlot, inventory.items[i].Id());
        // }

        // change scroll height after updating UI
        // StartCoroutine(ScrollHeightRoutine(remainScrollPosition));
    }

    void InstantiateBar(Recipe recipe)
    {
		GameObject bar = Instantiate(recipeBarPrefab, itemsParent) as GameObject;
    }

    // IEnumerator ScrollHeightRoutine(bool remainScrollPosition)
    // {
    //     yield return null;
    //     ScrollHeight cScript = GetComponent<ScrollHeight>();
    //     cScript.UpdateHeight(itemsParent, remainScrollPosition);
    // }
}
