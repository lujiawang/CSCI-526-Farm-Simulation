using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InsertMiniGame : MonoBehaviour
{
    public string MiniGameName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenCloseMiniGame()
    {
        if (SceneManager.GetActiveScene().name == MiniGameName)
            CloseMiniGame();
        else
            OpenMiniGame();
    }


    void OpenMiniGame()
    {
        SceneManager.LoadSceneAsync("Match 3", LoadSceneMode.Single);
    
        if (GameObject.Find("Inventory") != null)
        {
            StartCoroutine(SwitchSlotsInteractability(GameObject.Find("Inventory"), false));
        }
    }

    void CloseMiniGame()
    {
        // Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync("Farming_01_main", LoadSceneMode.Single);
        if (GameObject.Find("Inventory") != null)
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
        foreach (Transform slot in itemsParentObj)
        {
            // Debug.Log("switched!");
            string itemName = slot.GetChild(0).Find("Text").GetComponent<Text>().text;
            GameObject itemObj = slot.GetChild(0).Find(itemName).gameObject;
            if (itemObj.name.Contains("Seed"))
            {
                CanvasGroup itemCanvasGroup = itemObj.GetComponent<CanvasGroup>();
                itemCanvasGroup.interactable = onOrOff;
                itemCanvasGroup.blocksRaycasts = onOrOff;
                // Debug.Log(itemCanvasGroup.blocksRaycasts);
            }
        }
    }
}
