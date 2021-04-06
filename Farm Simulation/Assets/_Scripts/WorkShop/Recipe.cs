using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Recipe : IComparable<Recipe>
{
    private string name;
    private string[] ingredients;

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
}
