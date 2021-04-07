using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestMode : MonoBehaviour
{

    private void Start()
    {
        
    }

    public void HarvestModeToggle()
    {
        PlayerStats.Harvest = !PlayerStats.Harvest;
    }
}
