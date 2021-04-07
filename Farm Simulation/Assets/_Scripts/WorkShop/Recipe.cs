using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Recipe : IComparable<Recipe>
{
    private string name;
    private string[] ingredients;

    public static List<Recipe> allRecipes = new List<Recipe>()
    {
        new Recipe("FruitSalads"),
        new Recipe("CornSuccotash"),
        new Recipe("EggplantSoup"),
        new Recipe("CucumBurger"),
        new Recipe("TurnipRamen"),
        new Recipe("TomatoSandwich"),
        new Recipe("VeggieKebab"),
        new Recipe("Salmagundi"),
        new Recipe("VeggieRisotto"),
        new Recipe("Hodgepodge")
    };

    public Recipe(string name)
    {
        SetAllFields(name);
    }

    public string Name()
    {
    	return this.name;
    }

    public string[] Ingredients()
    {
    	return this.ingredients;
    }

    public void SetAllFields(string name)
    {
    	this.name = name;
    	this.ingredients = GetRecipeIngredients(name);
    }

    public int CompareTo(Recipe recipe)
    {
    	int thisId = Item.GetItemId(this.name);
    	int otherId = Item.GetItemId(recipe.Name());

    	if(thisId - otherId > 0)
    		return 1;
    	else if(thisId - otherId < 0)
    		return -1;
    	else
    		return 0;
    }

    public static string[] GetRecipeIngredients(string name)
    {
    	switch(name)
    	{
    		case "FruitSalads":
    			return new string[]{"Grapes", "Avocado", "Strawberry"};
            case "CornSuccotash":
                return new string[]{"Corn", "Potato"};
            case "EggplantSoup":
                return new string[]{"Eggplant", "Melon", "Cassava"};
            case "CucumBurger":
                return new string[]{"Cucumber", "Orange", "Wheat"};
            case "TurnipRamen":
                return new string[]{"Turnip", "Wheat"};
            case "TomatoSandwich":
                return new string[]{"Tomato", "Avocado", "Wheat"};
            case "VeggieKebab":
                return new string[]{"Potato", "Tomato", "Turnip"};
            case "Salmagundi":
                return new string[]{"Lemon", "Corn"};
            case "VeggieRisotto":
                return new string[]{"Cucumber", "Rice"};
            case "Hodgepodge":
                return new string[]{"Pineapple", "Potato", "Cucumber"};
    		default:
    			Debug.LogWarning("Recipe->SetIngredients() error! " + name + "not exists");
    			return null;
    	}
    }

    public static Recipe MatchRecipe(List<string> ingredients)
    {
        foreach(Recipe recipe in allRecipes)
        {
            if(recipe.Ingredients().SequenceEqual(ingredients))
                return new Recipe(recipe.Name());
        }
        return null;
    }

    // returns true if is a new recipe
    public static bool StoreRecipe(string recipeName)
    {
        if(!PlayerPrefs.HasKey("recipesInventoryIndex"))
        {
            PlayerPrefs.SetString("recipesInventoryIndex", recipeName);
            return true;
        }
        string storedRecipeIndex = PlayerPrefs.GetString("recipesInventoryIndex");
        string[] indexArray = storedRecipeIndex.Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
        // check if Recipe already exists
        foreach(string storedRecipe in indexArray)
        {
            if(storedRecipe == recipeName)
                return false;
        }
        PlayerPrefs.SetString("recipesInventoryIndex", storedRecipeIndex + " " + recipeName);
        return true;
    }
}
