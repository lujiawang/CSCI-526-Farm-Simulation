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

    

    public void AddToInventory(string cropName)
    {
        //script is attached to the player.
        //adds crop to the dictionary that represents the current inventory of the player
        
        if (cropName == "") return;

        if (!PlayerStats.cropCount.ContainsKey(cropName))
        {
            PlayerStats.cropCount[cropName] = 0;
        }
        PlayerStats.cropCount[cropName] += 1;
        PrintNotification(cropName);
    }


    void PrintNotification(string cropName)
    {
        inventoryText.text = "";


        inventoryText.text += "Congrats, you have collected: \n";

        inventoryText.text += 1 + "x " + cropName + "\n";
        Debug.Log(inventoryText.text);
        StartCoroutine(ActivateSuccessPanel());
    }



    //public void Sweep()
    //{
    //    bool cropExists = false;
    //    cropCount = new Dictionary<string, int>();
    //    foreach (GameObject cropLand in cropLands)
    //    {
    //        if (cropLand.transform.childCount > 0)
    //        {
    //            // get the crop from each cropLand
    //            GameObject childObject = cropLand.transform.GetChild(0).gameObject;
    //            Debug.Log("Just clicked something");
    //            if (childObject)
    //            {
    //                CropGrowing gScript = childObject.GetComponent<CropGrowing>();


    //                Debug.Log("Time to harvest");

    //                if (childObject == null) return;
    //                int reward = gScript.harvestCrop();

    //                if (reward > 0)
    //                {
    //                    //crop is grown
    //                    string name = gScript.cropName;
    //                    if (!cropCount.ContainsKey(name))
    //                    {
    //                        cropCount[name] = 0;
    //                    }

    //                    cropCount[name] += 1;
    //                    cropExists = true;
    //                    //put back into original parent and Set inactive


    //                    //childObject.SetActive(false);
    //                    //childObject.transform.SetParent(GameObject.Find("CropPlaceholder").transform);

    //                    //Destroy gameobject
    //                    Destroy(childObject);
           
    //                }
    //            }

    //        }
    //        else
    //        {
    //            Debug.Log("None grown yet!");
    //        }
    //    }
    //    if (!cropExists)
    //    {
    //        StartCoroutine(ActivateFailPanel());
    //    }
    //    else
    //    {
    //        PrintCrops(cropCount);
    //    }
    //}

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



    void PrintCrops(Dictionary<string,int> cropCount)
    {
        inventoryText.text = "";

        
        inventoryText.text += "Congrats, you have collected: \n";

        foreach(KeyValuePair<string,int> crop in cropCount) {
            inventoryText.text += crop.Value + "x " + crop.Key + "\n";
           
        }
        inventoryText.text += "Sell your stock at the store.";
        Debug.Log(inventoryText.text);
        StartCoroutine(ActivateSuccessPanel());
        
    }

    private IEnumerator ActivateFailPanel()
    {
        failPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        failPanel.SetActive(false);
    }
    private IEnumerator ActivateSuccessPanel()
    {
        successPanel.SetActive(true);
        yield return new WaitForSeconds(2);

        successPanel.SetActive(false);
    }
}
