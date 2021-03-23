using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestStats : MonoBehaviour
{
    /// Summary: Script is called when the harvest button is clicked. Different notifications are shown depending on where the player is standing
    



    //public static Dictionary<string, int> cropCount;
    public GameObject failPanel;
    public GameObject successPanel;
    private Text inventoryText;
    private Text warningText;
    //private GameObject[] cropLands;
    void Awake()
    {
        //cropLands = GameObject.FindGameObjectsWithTag("cropLand");
        inventoryText = successPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        warningText = failPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        //cropCount = new Dictionary<string, int>();
    }

    

    public void AddToInventory(string cropName, int reward)
    {
        //script is attached to the player.
        //adds crop to the dictionary that represents the current inventory of the player
        
        if (cropName == "") return;

        if (!PlayerStats.cropCount.ContainsKey(cropName))
        {
            PlayerStats.cropCount[cropName] = 0;
        }
        PlayerStats.cropCount[cropName] += 1;
        PlayerStats.ChangeCurrency(reward);
        PrintNotification(cropName);
    }


    void PrintNotification(string cropName)
    {
        inventoryText.text = "";
        inventoryText.text += "Hurray! You have collected:";
        StartCoroutine(ActivateSuccessPanel(cropName));
    }

    public void ActivateFail(int index)
    {
        switch (index)
        {
            case 1:
                warningText.text = "You are not near any crops!";
                break;
            case 2:
                warningText.text = "There's nothing on the field!";
                break;
            case 3:
                warningText.text = "The crop has not finished growing";
                break;
            default:
                warningText.text = "Nothing to collect";
                break;
        }
            
        StartCoroutine(ActivateFailPanel());
    }



    void PrintCrops(Dictionary<string, int> cropCount)
    {
        inventoryText.text = "";


        inventoryText.text += "Congrats, you have collected: \n";

        foreach (KeyValuePair<string, int> crop in cropCount)
        {
            Debug.Log(crop.Value + "x " + crop.Key + "\n");

        }
        

    }

    private IEnumerator ActivateFailPanel()
    {
        failPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        failPanel.SetActive(false);
    }
    private IEnumerator ActivateSuccessPanel(string cropName)
    {
        GameObject crop = successPanel.transform.Find(cropName).gameObject;
        successPanel.SetActive(true);
        crop.SetActive(true);
        yield return new WaitForSeconds(2);

        successPanel.SetActive(false);
        crop.SetActive(false);
        PrintCrops(PlayerStats.cropCount);
    }
}
