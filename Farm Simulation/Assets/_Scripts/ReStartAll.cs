using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartAll : MonoBehaviour
{
    public void ReStart()
    {
    	float musciVol = 1f;
    	float SFXVol = 1f;
    	if(PlayerPrefs.HasKey("MusicVolume"))
    		musciVol = PlayerPrefs.GetFloat("MusicVolume");
    	if(PlayerPrefs.HasKey("SFXVolume"))
    		SFXVol = PlayerPrefs.GetFloat("SFXVolume");

    	PlayerPrefs.DeleteAll();

    	PlayerPrefs.SetFloat("MusicVolume", musciVol);
    	PlayerPrefs.SetFloat("SFXVolume", SFXVol);

    	Inventory inventory = Inventory.instance;
    	inventory.ReStart();
    	StoreInventory storeInventory = StoreInventory.instance;
    	storeInventory.ReStart();
    }
}
