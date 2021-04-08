using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cook : MonoBehaviour
{
    public Transform congratPanel;
    Coroutine showCongratsCOR;
    public Text failureToast;
    string failureMessage = "An unpleasant smell overwhelmed you as you approached the pot. You had to throw all this garbage out...";
    
	IngredientsInventoryUI ingredientsUIScript;
	RecipesInventoryUI recipesUIScript;

    ZoomObj zoomScript;
    FadeObj fadeScript;

	Inventory inventory;
    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        zoomScript = GetComponentInParent<Canvas>().rootCanvas.GetComponent<ZoomObj>();
        fadeScript = GetComponentInParent<Canvas>().rootCanvas.GetComponent<FadeObj>();

        ingredientsUIScript = this.GetComponent<IngredientsInventoryUI>();
        recipesUIScript = GameObject.Find("Recipes").GetComponent<RecipesInventoryUI>();

        inventory = Inventory.instance;
        soundManager = SoundManager.instance;
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

        // play sound as cooing starts
        soundManager.PlaySound(14);

    	Recipe matchRecipe = Recipe.MatchRecipe(ingredients);
    	// Debug.Log(matchRecipe);

    	if(matchRecipe != null) //successfully cooked an item
    	{
    		// store recipe; Update RecipesUI and show congratulations if is a newly found recipe
            bool isNewRecipe = Recipe.StoreRecipe(matchRecipe.Name());
    		if(isNewRecipe)
    		{
	    		recipesUIScript.UpdateUI(false);
                showCongratsCOR = StartCoroutine(ShowCongrats(matchRecipe, showCongratsCOR));
                // play sound
                soundManager.PlaySound(11);
    		}

    		// Destroy ingredients menu objects
    		ingredientsUIScript.DestroyAll();

    		// change tab if necessary
            InventoryUI cScript = GameObject.Find("Inventory").GetComponent<InventoryUI>();
            cScript.ToggleRespectiveShowButton(Item.GetItemId(matchRecipe.Name()));
    		// Add cooked food to inventory
    		inventory.Add(matchRecipe.Name(), 1);

            // play sound
            if(!isNewRecipe)
                soundManager.PlaySound(12);
    	}else //failed in cooking
    	{
    		// Destroy ingredients menu objects
    		ingredientsUIScript.DestroyAll();

    		// show notifications
            ShowToast cScript = GameObject.Find("Canvas").GetComponent<ShowToast>();
            cScript.showCustomizedToast(failureToast, failureMessage, 2);

            // play sound
            soundManager.PlaySound(13);
    	}
    }

    IEnumerator ShowCongrats(Recipe recipe, Coroutine waitTillAfter)
    {
        if(waitTillAfter != null)
            yield return waitTillAfter;
        yield return ShowCongrats(recipe);
    }

    IEnumerator ShowCongrats(Recipe recipe)
    {
        string name = recipe.Name();
        Sprite sprite = Item.GetItemSprite(name);
        Transform itemButton = congratPanel.Find("InventorySlot").Find("ItemButton");
        itemButton.Find("Text").GetComponent<Text>().text = name;
        itemButton.Find("Item").GetComponent<Image>().sprite = sprite;

        // Animations
        congratPanel.GetComponent<CanvasGroup>().alpha = 1f;
        yield return StartCoroutine(zoomScript.Zoom(congratPanel, true, 0.4f));
        float counter = 0f;
        float duration = 2f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(fadeScript.Fade(congratPanel, false, 0.4f));
    }
}
