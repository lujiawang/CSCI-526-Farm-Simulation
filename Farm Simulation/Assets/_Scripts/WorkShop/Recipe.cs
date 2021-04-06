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
    	this.ingredients = GetIngredients(name);
    }

    public int CompareTo(Recipe recipe)
    {
    	int thisId = Item.GetCropId(this.name);
    	int otherId = Item.GetCropId(recipe.Name());

    	if(thisId - otherId > 0)
    		return 1;
    	else if(thisId - otherId < 0)
    		return -1;
    	else
    		return 0;
    }

    public static string[] GetIngredients(string name)
    {
    	switch(name)
    	{
    		case "dddd":
    			return new string[]{"Corn", "Tomato"};
    			break;
    		default:
    			Debug.LogWarning("Recipe->SetIngredients() error!");
    			return null;
    	}
    }
}
