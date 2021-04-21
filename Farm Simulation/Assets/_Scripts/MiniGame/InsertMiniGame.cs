using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InsertMiniGame : MonoBehaviour
{
    public string MiniGameName;
    // Start is called before the first frame update

    public static int firstEntryPrice = 100;

    public int price = firstEntryPrice; // the current minigame entry price

    void Start()
    {
        // set current entry price
        // price = firstEntryPrice;
        if(PlayerPrefs.HasKey("minigameEntryPrice"))
            price = PlayerPrefs.GetInt("minigameEntryPrice");
        else
        {
            PlayerPrefs.SetInt("minigameEntryPrice", firstEntryPrice);
            PlayerPrefs.SetInt("storeUnlockStage", 1);
        }

        // change price tag
        if(this.transform.Find("Price") != null)
            this.transform.Find("Price").GetComponent<Text>().text = "" + price;
    }

    public void NextStageEntryPrice()
    {
        switch(price)
        {
            case 100:
                price = 300;
                PlayerPrefs.SetInt("minigameEntryPrice", price);
                PlayerPrefs.SetInt("storeUnlockStage", 2);
                break;
            case 300:
                price = 500;
                PlayerPrefs.SetInt("minigameEntryPrice", price);
                PlayerPrefs.SetInt("storeUnlockStage", 3);
                break;
            case 500:
                price = 1000;
                PlayerPrefs.SetInt("minigameEntryPrice", price);
                PlayerPrefs.SetInt("storeUnlockStage", 4);
                break;
            case 1000:
                if(PlayerPrefs.GetInt("storeUnlockStage") < 5)
                    PlayerPrefs.SetInt("storeUnlockStage", 5);
                else
                    PlayerPrefs.SetInt("storeUnlockStage", 6);
                break;
            default:
                break;
        }
    }

    public void OpenCloseMiniGame(bool win)
    {

        if (SceneManager.GetActiveScene().name == MiniGameName)
        {
            if(win) // minigame succeeded
            {
                SoundManager.instance.PlaySound(11);
                NextStageEntryPrice();
                // show winning notification
                StartCoroutine(CloseMiniGameCOR(true));
            }else // minigame failed
            {
                // show losing notification
                StartCoroutine(CloseMiniGameCOR(false));
            }
            
        }
        else
        {
            // if currecy is enough to enter minigame
            if(PlayerStats.Currency >= price)
            {
                PlayerStats.ChangeCurrency(-price);
                StartCoroutine(OpenMiniGameCOR());
            }else
            {
                ShowToast cScript = GameObject.Find("Canvas").GetComponent<ShowToast>();
                cScript.showToast("You don't have enough coins!", 1);
            }
        }
    }

    IEnumerator CloseMiniGameCOR(bool didWin)
    {
        ZoomObj zoomScript = GameObject.Find("SceneCanvas").GetComponent<ZoomObj>();
        FadeObj fadeScript = GameObject.Find("SceneCanvas").GetComponent<FadeObj>();
        // Animations
        Transform panel = null;
        if(didWin)
        {
            if(PlayerPrefs.GetInt("storeUnlockStage") > 5)
                panel = GameObject.Find("WinGeneralMessagePanel").transform;
            else
            {
                panel = GameObject.Find("WinMessagePanel").transform;
                Transform itemLayout = panel.GetChild(0).Find("ItemLayout");
                int unlockStage = PlayerPrefs.GetInt("storeUnlockStage");
                int id = (unlockStage - 1) * Item.tierDivideLength + Item.seedIdUpperLimit + 1;
                foreach(Transform slot in itemLayout)
                {
                    string name = Item.GetItemName(id);
                    Sprite sprite = Item.GetItemSprite(name);
                    slot.GetChild(0).Find("Text").GetComponent<Text>().text = name;
                    slot.GetChild(0).Find("Item").GetComponent<Image>().sprite = sprite;
                    id++;
                }
            }
        }
        else
            panel = GameObject.Find("LoseMessagePanel").transform;
        panel.GetComponent<CanvasGroup>().alpha = 1f;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        yield return StartCoroutine(zoomScript.Zoom(panel, true, 0.4f));
        float counter = 0f;
        float duration = 2f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(fadeScript.Fade(panel, false, 0.4f));
        CloseMiniGame();
    }

    IEnumerator OpenMiniGameCOR()
    {
        ZoomObj zoomScript = GetComponentInParent<Canvas>().rootCanvas.GetComponent<ZoomObj>();
        FadeObj fadeScript = GetComponentInParent<Canvas>().rootCanvas.GetComponent<FadeObj>();
        // Animations
        Transform panel = GameObject.Find("MinigameStartMessagePanel").transform;
        panel.GetComponent<CanvasGroup>().alpha = 1f;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        yield return StartCoroutine(zoomScript.Zoom(panel, true, 0.4f));
        float counter = 0f;
        float duration = 2f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(fadeScript.Fade(panel, false, 0.4f));
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
