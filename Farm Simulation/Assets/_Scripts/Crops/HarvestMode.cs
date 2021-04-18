using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestMode : MonoBehaviour
{

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        PlayerStats.Harvest = false;
    }

    public void HarvestModeToggle()
    {
        PlayerStats.Harvest = !PlayerStats.Harvest;
    }

    
}
