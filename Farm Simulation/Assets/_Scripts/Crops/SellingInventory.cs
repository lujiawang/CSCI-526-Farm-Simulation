using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SellingInventory : MonoBehaviour
{
    // to access player's current inventory
    private int check = 0;
    void Start()
    {
        
    }


    void PrintInventory()
    {
        if (PlayerStats.cropCount == null) return;
        foreach (KeyValuePair<string, int> crop in PlayerStats.cropCount)
        {
            Debug.Log(crop.Value + "x " + crop.Key + "\n");

        }
    }


    void RemoveFromInventory(string name)
    {
        PlayerStats.cropCount[name] -= 1;
    }

    void SellCrops()
    {
        //separate sell?
    }



    void BuySeeds(string name, int cost)
    {
        if (!PlayerStats.seedsCount.ContainsKey(name))
        {
            PlayerStats.seedsCount[name] = 0;
        }
        PlayerStats.seedsCount[name] += 1;
        PlayerStats.Currency -= cost;

    }

    private void Update()
    {
        //just to test out if inventory can be accessed across scenes
        if (check < 1)
        {
            check++;
            PrintInventory();
            
        }
    }
}
