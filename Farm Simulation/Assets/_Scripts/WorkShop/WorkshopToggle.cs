using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class WorkshopToggle : MonoBehaviour
{
    // GameObject sceneParent;

    void Start()
    {
        // if(this.name == "BackToMainButton")
        //     sceneParent = GameObject.Find("SceneParent");
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         OpenWorkshop();
    //     }
    // }

    public void OpenCloseWorkShop()
    {
        if(SceneManager.GetActiveScene().name == "Workshop")
            CloseWorkshop();
        else
            OpenWorkshop();
    }

    void OpenWorkshop()
    {
        // Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync("Workshop", LoadSceneMode.Single);
    }

    void CloseWorkshop()
    {
        // Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync("Farming_01_main", LoadSceneMode.Single);
    }

}
