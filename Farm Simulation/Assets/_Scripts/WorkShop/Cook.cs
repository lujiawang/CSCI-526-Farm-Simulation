using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
	IngredientsInventoryUI ingredientsUIScript;
	RecipesInventoryUI recipesUIScript;

	Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        ingredientsUIScript = this.GetComponent<IngredientsInventoryUI>();
        recipesUIScript = GameObject.Find("Recipes").GetComponent<RecipesInventoryUI>();

        inventory = Inventory.instance;
    }

    public void Cooking()
    {
    	if(inventory == null)
    	{
    		Debug.LogWarning("inventory is not present!");
    		return;
    	}

    	List<string> ingredients = new List<string>();
    	foreach(Item item in ingredientsUIScript.ingredients)
    	{
    		ingredients.Add(item.Name());
    	}
    	// not enough ingredients to start cooking
    	if(ingredients.Count < IngredientsInventoryUI.ingredientLowerLimit)
    	{
    		ShowToast cScript = GameObject.Find("Canvas").GetComponent<ShowToast>();
    		cScript.showToast("You need at least " + IngredientsInventoryUI.ingredientLowerLimit
    		 + " ingredients to start cooking!", 1);
    		return;
    	}

    	Recipe matchRecipe = Recipe.MatchRecipe(ingredients);
    	// Debug.Log(matchRecipe);

    	if(matchRecipe != null) //successfully cooked an item
    	{
    		// store recipe; Update RecipesUI if is a newly found recipe
    		if(Recipe.StoreRecipe(matchRecipe.Name()))
    		{
	    		recipesUIScript.UpdateUI(false);
    		}

    		// Destroy ingredients menu objects
    		ingredientsUIScript.DestroyAll();

    		// change tab if necessary
            InventoryUI cScript = GameObject.Find("Inventory").GetComponent<InventoryUI>();
            cScript.ToggleRespectiveShowButton(Item.GetItemId(matchRecipe.Name()));
    		// Add cooked food to inventory
    		inventory.Add(matchRecipe.Name(), 1);
    	}else //failed in cooking
    	{
    		// Destroy ingredients menu objects
    		ingredientsUIScript.DestroyAll();

    		// show some notifications
    	}
    }
}
