using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        if(GameObject.Find("Inventory") != null)
        {
            StartCoroutine(SwitchSlotsInteractability(GameObject.Find("Inventory"), false));
        }
    }

    void CloseWorkshop()
    {
        // Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync("Farming_01_main", LoadSceneMode.Single);
        if(GameObject.Find("Inventory") != null)
        {
            StartCoroutine(SwitchSlotsInteractability(GameObject.Find("Inventory"), true));
        }
    }

    IEnumerator SwitchSlotsInteractability(GameObject inventory, bool onOrOff)
    {
        // Debug.Log(onOrOff);
        yield return null;
        Transform itemsParentObj = inventory.transform.GetChild(0).GetChild(0);
        // Debug.Log(itemsParentObj.childCount);
        foreach(Transform slot in itemsParentObj)
        {
            // Debug.Log("switched!");
            string itemName = slot.GetChild(0).Find("Text").GetComponent<Text>().text;
            GameObject itemObj = slot.GetChild(0).Find(itemName).gameObject;
            if(itemObj.name.Contains("Seed"))
            {
                CanvasGroup itemCanvasGroup = itemObj.GetComponent<CanvasGroup>();
                itemCanvasGroup.interactable = onOrOff;
                itemCanvasGroup.blocksRaycasts = onOrOff;
                // Debug.Log(itemCanvasGroup.blocksRaycasts);
            }
        }
    }

}
