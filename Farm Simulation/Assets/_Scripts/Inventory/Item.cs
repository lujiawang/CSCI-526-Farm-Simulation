using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item") ]

// public class Item : ScriptableObject
public class Item : IComparable<Item>, IEquatable<Item>
{
	
	// new public string name = "New Item";
	// public Sprite icon = null;
	// public int cost = 0;
	// public int num = 0;



	// public struct Item 
	// {
		
		private string name;
		private int num;

		private int id;
		private Sprite icon;

		private int sellPrice;
		private int buyPrice;

		public static int seedIdUpperLimit = 19;
		public static int harvestIdUpperLimit = 39;
		public static int foodIdUpperLimit = 49;
		public static int allIdUpperLimit = 49;

		public static float sellToBuyFactor = 1.9f;

		// for seeds and harvets
		public static int tierDivideLength = 4;
		public static int basicPrice = 15;
		public static float tierUpPriceFactor = 1.7f;
		public static float seedToFruitFactor = 2.5f / 4; //4 = Expectation of randomharvest amount

		public static float basicGrowTime = 2f;
		public static float tierUpGrowTimeFactor = 1.3f;

		// for foods
		public static float cropToFoodFactor = 2.5f;
		// public static int foodTierDivideLength = 3;
		// public static int foodBasicPrice = 325;
		// public static float foodTierUpPriceFactor = 1.3f;

		public string Name(){
			return this.name;
		}
		public int Num(){
			return this.num;
		}

		public int Id(){
			return this.id;
		}
		public Sprite Icon(){
			return this.icon;
		}

		public int SellPrice(){
			return this.sellPrice;
		}
		public int BuyPrice(){
			return this.buyPrice;
		}

		public void SetName(string name){
			this.name = name;
		}
		public void SetNum(int num){
			this.num = num;
		}
		public void AddNum(int num){
			this.num += num;
		}

		public void SetId(int id){
			this.id = id;
		}
		public void SetIcon(Sprite icon){
			this.icon = icon;
		}

		public void SetSellPrice(int sellPrice){
			this.sellPrice = sellPrice;
		}
		public void SetBuyPrice(int buyPrice){
			this.buyPrice = buyPrice;
		}

		public void SetAllFields(string name, int num)
		{
			this.name = name;
			this.num = num;

			this.id = GetItemId(name);
			this.icon = GetItemSprite(name);

			this.sellPrice = GetItemSellPrice(this.id);
			this.buyPrice = GetItemBuyPrice(this.sellPrice);
		}

		public void SetAllFields(int id, int num)
		{
			if(id < 0 || id > allIdUpperLimit)
			{
				Debug.LogWarning("id out of bounds!");
				return;
			}
			this.id = id;
			this.num = num;

			this.name = GetItemName(id);
			this.icon = GetItemSprite(this.name);

			this.sellPrice = GetItemSellPrice(this.id);
			this.buyPrice = GetItemBuyPrice(this.sellPrice);
		}

		public int CompareTo(Item other)
	    {
	    	float generatedThisId = (this.id > seedIdUpperLimit && this.id <= harvestIdUpperLimit) ? 
	    	(float)(this.id - seedIdUpperLimit - 0.5f) : (float) this.id;
	    	float generatedOtherId = (other.Id() > seedIdUpperLimit && other.Id() <= harvestIdUpperLimit) ? 
	    	(float)(other.Id() - seedIdUpperLimit - 0.5f) : (float) other.Id();
	    	if(generatedThisId - generatedOtherId > 0)
	    		return 1;
	    	else if(generatedThisId - generatedOtherId < 0)
	    		return -1;
	    	else
	    		return 0;
	    }

	    // NOTE: difference in this.num does not affect equality
	    public bool Equals(Item other)
	    {
	    	if(this.id == other.Id()) return true;
	    	return false;
	    }

		public static Sprite GetItemSprite(string name)
	    {
	        Sprite[] icons = Resources.LoadAll<Sprite>("Crop_Spritesheet");
	        bool seed = name.Contains("Seed");
	    	string tempName = name;
	        if (seed)
	            tempName = name.Remove(name.IndexOf("Seed"), "Seed".Length);
	        else
	            tempName = name;
	    	// Debug.Log(tempName);
	        foreach (Sprite i in icons)
	        {
	        	if(seed)
	        	{
	        		if(tempName + "_0" == i.name)
	            		return i;
	        	}
	            else
	            {
	        		if(tempName + "_5" == i.name)
	            		return i;
	            }
	            
	        }
	        // failed in finding sprite from Crop_Spritesheet. Proceed to Meals_Spritesheet
	        icons = Resources.LoadAll<Sprite>("Meals_Spritesheet");
	        foreach(Sprite i in icons)
	        {
	        	if(tempName == i.name)
	        		return i;
	        }
	        Debug.LogWarning(name + " doesn't exist");
	        return null;
	    }

	    public static int GetItemSellPrice(int id)
	    {
	    	if(id < 0)
	    	{
	    		Debug.LogWarning("invalid input! id cannot be negative");
	    		return -1;
	    	}else if(id <= seedIdUpperLimit) //seeds
	    	{
	    		double multiplier = Math.Pow( tierUpPriceFactor, (id / tierDivideLength) );
	    		if(multiplier < 1.0d) multiplier = 1.0d;
	    		return (int) (basicPrice * multiplier);
	    	}else if(id <= harvestIdUpperLimit) //for grown crops(fruits)
	    	{
	    		return (int)(GetItemSellPrice(id - seedIdUpperLimit - 1) * seedToFruitFactor);
	    	}else if(id <= foodIdUpperLimit) //for foods
	    	{
	    		string[] ingredients = Recipe.GetRecipeIngredients(GetItemName(id));
	    		int price = 0;
	    		foreach(string name in ingredients)
	    		{
	    			price += GetItemSellPrice(GetItemId(name));
	    		}
	    		return (int) (price * cropToFoodFactor);

	    		// double multiplier = Math.Pow(foodTierUpPriceFactor, ( (id - harvestIdUpperLimit - 1) / tierDivideLength));
	    		// if(multiplier < 1.0d) multiplier = 1.0d;
	    		// return (int) (foodBasicPrice * multiplier);
    		}else
	    	{
	    		Debug.LogWarning("id: " + id + "is overbounds");
	    		return -1;
	    	}
	    }

	    public static int GetItemBuyPrice(int sellPrice)
	    {
	    	return (int)(sellPrice * sellToBuyFactor);
	    }

	    public static float GetGrowTime(string name)
	    {
	    	int id = GetItemId(name);
	    	if(0 <= id && id <= seedIdUpperLimit)
	    	{
	    		double multiplier = Math.Pow( tierUpGrowTimeFactor, 
	    			(id / tierDivideLength) );
	    		if(multiplier < 1.0d) multiplier = 1.0d;
	    		return (float) (basicGrowTime * multiplier);
	    	}else if(id <= harvestIdUpperLimit)
	    	{
	    		double multiplier = Math.Pow( tierUpGrowTimeFactor, 
	    			((id - seedIdUpperLimit - 1) / tierDivideLength) );
	    		if(multiplier < 1.0d) multiplier = 1.0d;
	    		return (float) (basicGrowTime * multiplier);
    		}else
    		{
    			Debug.LogWarning("invalid input! name: " + name);
	    		return -1;
    		}
	    }

	    public static int RandomHarvest(string name)
	    {
	    	// int id = GetCropId(name);
	    	System.Random rand = new System.Random();
	        return rand.Next(3,6);
	    }

	    public static string GetItemName(int id)
	    {
	    	switch(id)
	    	{
	    		case 0:
	    			return "CornSeed";
	    		case 1:
	    			return "PotatoSeed";
	    		case 2:
	    			return "RiceSeed";
	    		case 3:
	    			return "WheatSeed";
	    		case 4:
	    			return "CassavaSeed";
	    		case 5:
	    			return "CucumberSeed";
	    		case 6:
	    			return "EggplantSeed";
	    		case 7:
	    			return "OrangeSeed";
	    		case 8:
	    			return "LemonSeed";
	    		case 9:
	    			return "GrapesSeed";
	    		case 10:
	    			return "SunflowerSeed";
	    		case 11:
	    			return "TomatoSeed";
	    		case 12:
	    			return "MelonSeed";
	    		case 13:
	    			return "PineappleSeed";
	    		case 14:
	    			return "StrawberrySeed";
	    		case 15:
	    			return "TurnipSeed";
	    		case 16:
	    			return "AvocadoSeed";
	    		case 17:
	    			return "CoffeeSeed";
	    		case 18:
	    			return "RoseSeed";
	    		case 19:
	    			return "TulipSeed";

	    		case 20:
	    			return "Corn";
	    		case 21:
	    			return "Potato";
	    		case 22:
	    			return "Rice";
	    		case 23:
	    			return "Wheat";
	    		case 24:
	    			return "Cassava";
	    		case 25:
	    			return "Cucumber";
	    		case 26:
	    			return "Eggplant";
	    		case 27:
	    			return "Orange";
	    		case 28:
	    			return "Lemon";
	    		case 29:
	    			return "Grapes";
	    		case 30:
	    			return "Sunflower";
	    		case 31:
	    			return "Tomato";
	    		case 32:
	    			return "Melon";
	    		case 33:
	    			return "Pineapple";
	    		case 34:
	    			return "Strawberry";
	    		case 35:
	    			return "Turnip";
	    		case 36:
	    			return "Avocado";
	    		case 37:
	    			return "Coffee";
	    		case 38:
	    			return "Rose";
	    		case 39:
	    			return "Tulip";

	    		case 40:
	    			return "FruitSalads";
	    		case 41:
	    			return "CornSuccotash";
	    		case 42:
	    			return "EggplantSoup";
	    		case 43:
	    			return "CucumBurger";
	    		case 44:
	    			return "TurnipRamen";
	    		case 45:
	    			return "TomatoSandwich";
	    		case 46:
	    			return "VeggieKebab";
	    		case 47:
	    			return "Salmagundi";
	    		case 48:
	    			return "VeggieRisotto";
	    		case 49:
	    			return "Hodgepodge";
	    		default:
	    			Debug.LogWarning("GetItemName() received invalid input: " + id);
	    			return null;
	    	}
	    }

	    public static int GetItemId(string name)
	    {
	    	switch(name)
	    	{
	    		case "CornSeed":
	    			return 0;
	    		case "PotatoSeed":
	    			return 1;
	    		case "RiceSeed":
	    			return 2;
	    		case "WheatSeed":
	    			return 3;
	    		case "CassavaSeed":
	    			return 4;
	    		case "CucumberSeed":
	    			return 5;
	    		case "EggplantSeed":
	    			return 6;
	    		case "OrangeSeed":
	    			return 7;
	    		case "LemonSeed":
	    			return 8;
	    		case "GrapesSeed":
	    			return 9;
	    		case "SunflowerSeed":
	    			return 10;
	    		case "TomatoSeed":
	    			return 11;
	    		case "MelonSeed":
	    			return 12;
	    		case "PineappleSeed":
	    			return 13;
	    		case "StrawberrySeed":
	    			return 14;
	    		case "TurnipSeed":
	    			return 15;
	    		case "AvocadoSeed":
	    			return 16;
	    		case "CoffeeSeed":
	    			return 17;
	    		case "RoseSeed":
	    			return 18;
	    		case "TulipSeed":
	    			return 19;

	    		case "Corn":
	    			return 20;
	    		case "Potato":
	    			return 21;
	    		case "Rice":
	    			return 22;
	    		case "Wheat":
	    			return 23;
	    		case "Cassava":
	    			return 24;
	    		case "Cucumber":
	    			return 25;
	    		case "Eggplant":
	    			return 26;
	    		case "Orange":
	    			return 27;
	    		case "Lemon":
	    			return 28;
	    		case "Grapes":
	    			return 29;
	    		case "Sunflower":
	    			return 30;
	    		case "Tomato":
	    			return 31;
	    		case "Melon":
	    			return 32;
	    		case "Pineapple":
	    			return 33;
	    		case "Strawberry":
	    			return 34;
	    		case "Turnip":
	    			return 35;
	    		case "Avocado":
	    			return 36;
	    		case "Coffee":
	    			return 37;
	    		case "Rose":
	    			return 38;
	    		case "Tulip":
	    			return 39;

	    		case "FruitSalads":
	    			return 40;
	    		case "CornSuccotash":
	    			return 41;
	    		case "EggplantSoup":
	    			return 42;
	    		case "CucumBurger":
	    			return 43;
	    		case "TurnipRamen":
	    			return 44;
	    		case "TomatoSandwich":
	    			return 45;
	    		case "VeggieKebab":
	    			return 46;
	    		case "Salmagundi":
	    			return 47;
	    		case "VeggieRisotto":
	    			return 48;
	    		case "Hodgepodge":
	    			return 49;
	    		default:
	    			Debug.LogWarning("GetItemId() received invalid input: " + name);
	    			return -1;
	    	}
	    }
	// }
	
    
}


