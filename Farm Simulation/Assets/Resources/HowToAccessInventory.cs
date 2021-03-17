// this document demonstrates how to add/remove items in inventory
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToAccessInventory : MonoBehaviour{

	Inventory inventory;

	void Start(){
		inventory = Inventory.instance;
	}
	
	public void Foo(){
		// add one corn seed to inventory
		inventory.Add("CornSeed", 1);
		// remove one corn seed from inventory
		inventory.Add("CornSeed", -1);
		// add one corn fruit to inventory
		inventory.Add("CornFruit", 1);
		// remove one corn fruit from inventory
		inventory.Add("CornFruit", -1);
	}
}