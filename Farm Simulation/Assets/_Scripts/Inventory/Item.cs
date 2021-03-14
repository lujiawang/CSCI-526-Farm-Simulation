using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item") ]

// public class Item : ScriptableObject
public class Item
{
	
	// new public string name = "New Item";
	// public Sprite icon = null;
	// public int cost = 0;
	// public int num = 0;



	// public struct Item 
	// {
		// public int id;
		private string name;
		private int num;
		private Sprite icon;

		public string Name(){
			return this.name;
		}

		public Sprite Icon(){
			return this.icon;
		}

		public int Num(){
			return this.num;
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

		public void SetIcon(Sprite icon){
			this.icon = icon;
		}

		// public void SetIcon(string iconName){
		// 	this.icon = null;
		// }

		// public Item(int id, string name, int num){
		// 	this.id = id;
		// 	this.name = name;
		// 	this.num = num;
		// }
	// }
	
    
}
