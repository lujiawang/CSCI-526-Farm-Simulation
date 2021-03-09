using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats 
{
    //store player stats
    
    public static Dictionary<string, int> cropCount = new Dictionary<string, int>();

    private static int currency;
    public static int Currency
    {
        get { return currency;  }
        set { currency = value;  }
    }

    public static Dictionary<string, int> seedsCount = new Dictionary<string, int>();
}
