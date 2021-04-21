using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HarvestMode : MonoBehaviour
{

    public GameObject harvestModeObj;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerStats.Harvest = false;
        TouchToMove.disablePlayerMovement = false;
        if(harvestModeObj != null)
            harvestModeObj.SetActive(false);
    }

    // private void OnDestroy()
    // {
    //     PlayerStats.Harvest = false;
    //     TouchToMove.disablePlayerMovement = false;
    // }

    public void HarvestModeToggle()
    {
        PlayerStats.Harvest = !PlayerStats.Harvest;
        TouchToMove.disablePlayerMovement = PlayerStats.Harvest;

        harvestModeObj.SetActive(PlayerStats.Harvest);
    }

    
}
