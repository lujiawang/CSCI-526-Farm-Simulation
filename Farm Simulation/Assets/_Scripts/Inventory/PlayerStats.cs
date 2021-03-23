﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class PlayerStats 
{
    //store player stats
    
    public static Dictionary<string, int> cropCount = new Dictionary<string, int>();

    private static int currency = PlayerPrefs.GetInt("Currency", 0);
    public static int Currency
    {
        get { return currency;  }
        set { currency = value;  }
    }
    
    public static Dictionary<string, int> seedsCount = new Dictionary<string, int>();

    public static void ChangeCurrency(int val)
    {
        GameObject currencyUI = GameObject.Find("CurrencyUI");
        CurrencyChange cScript = currencyUI.GetComponent<CurrencyChange>();
        cScript.UpdateCurrency(val);

    }
    
}
