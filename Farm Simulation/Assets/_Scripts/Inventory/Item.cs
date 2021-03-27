using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item") ]

// public class Item : ScriptableObject
public class Item : IComparable<Item>
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

		public static int tierDivideLength = 4;
		
		public static int basicPrice = 15;
		public static float tierUpPriceFactor = 1.7f;
		public static float seedToFruitFactor = 2.5f / 4; //4 = Expectation of randomharvest amount
		public static float sellToBuyFactor = 1.9f;
		
		public static int seedIdUpperLimit = 19;
		public static int harvestIdUpperLimit = 39;

		public static float basicGrowTime = 2f;
		public static float tierUpGrowTimeFactor = 1.3f;

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

			this.id = GetCropId(name);
			this.icon = GetCropSprite(name);

			this.sellPrice = GetCropSellPrice(this.id);
			this.buyPrice = GetCropBuyPrice(this.sellPrice);
		}

		public static Sprite GetCropSprite(string name)
	    {
	        Sprite[] icons = Resources.LoadAll<Sprite>("Crop_Spritesheet");
	        bool seed = name.Contains("Seed");
	    	string tempName = name;
	        if (seed)
	            tempName = name.Remove(name.IndexOf("Seed"), "Seed".Length);
	        else
	            tempName = name;//.Remove(name.IndexOf("Fruit"), "Fruit".Length);
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
	        return null;
	    }

	    public static string GetCropName(int id)
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
	    		default:
	    			Debug.LogWarning("GetCropName() received invalid input!");
	    			return null;
	    	}
	    }

	    public static int GetCropId(string name)
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
	    		default:
	    			Debug.LogWarning("GetCropId() received invalid input!");
	    			return -1;
	    	}
	    }

	    public static int GetCropSellPrice(int id)
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
	    		return (int)(GetCropSellPrice(id - seedIdUpperLimit - 1) * seedToFruitFactor);
	    	}else
	    	{
	    		Debug.LogWarning("id >= 40 are not defined yet");
	    		return -1;
	    	}
	    	
	    }

	    public static int GetCropBuyPrice(int sellPrice)
	    {
	    	return (int)(sellPrice * sellToBuyFactor);
	    }

	    public static float GetGrowTime(string name)
	    {
	    	int id = GetCropId(name);
	    	if(id <= seedIdUpperLimit)
	    	{
	    		Debug.LogWarning("invalid input! id cannot be negative");
	    		return -1;
	    	}else if(id > harvestIdUpperLimit)
	    	{
	    		Debug.LogWarning("invalid input! id overbounds");
	    		return -1;
	    	}else //falls between harvests id
	    	{
	    		double multiplier = Math.Pow( tierUpGrowTimeFactor, 
	    			((id - seedIdUpperLimit - 1) / tierDivideLength) );
	    		if(multiplier < 1.0d) multiplier = 1.0d;
	    		return (float) (basicGrowTime * multiplier);
	    	}
	    }

	    public static int RandomHarvest(string name)
	    {
	    	// int id = GetCropId(name);
	    	System.Random rand = new System.Random();
	        return rand.Next(3,6);
	    }

	    public int CompareTo(Item item)
	    {
	    	float generatedThisId = (this.id > seedIdUpperLimit && this.id <= harvestIdUpperLimit) ? 
	    	(float)(this.id - seedIdUpperLimit - 0.5f) : (float) this.id;
	    	float generatedItemId = (item.Id() > seedIdUpperLimit && this.id <= harvestIdUpperLimit) ? 
	    	(float)(item.Id() - seedIdUpperLimit - 0.5f) : (float) item.Id();
	    	if(generatedThisId - generatedItemId > 0)
	    		return 1;
	    	else if(generatedThisId - generatedItemId < 0)
	    		return -1;
	    	else
	    		return 0;
	    	// return this.id - item.Id();	    	
	    }
	// }
	
    
}


