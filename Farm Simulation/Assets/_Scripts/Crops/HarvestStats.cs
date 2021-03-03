using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestStats : MonoBehaviour
{
    // Start is called before the first frame update
    //public class Crop
    //{
    //    public string cropName;
    //    public int reward;

    //    public Crop(string cn, int rwd)
    //    {
    //        cropName = cn;
    //        reward = rwd;
    //    }
    //}



    private Dictionary<string, int> cropCount;
    public GameObject failPanel;
    public GameObject successPanel;
    private Text inventoryText;
    private GameObject[] cropLands;
    void Awake()
    {
        cropLands = GameObject.FindGameObjectsWithTag("cropLand");
        inventoryText = successPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sweep()
    {
        bool cropExists = false;
        cropCount = new Dictionary<string, int>();
        foreach (GameObject cropLand in cropLands)
        {
            if (cropLand.transform.childCount > 0)
            {
                // get the crop from each cropLand
                GameObject childObject = cropLand.transform.GetChild(0).gameObject;
                Debug.Log("Just clicked something");
                if (childObject)
                {
                    CropGrowing gScript = childObject.GetComponent<CropGrowing>();


                    Debug.Log("Time to harvest");

                    if (childObject == null) return;
                    int reward = gScript.harvestCrop();

                    if (reward > 0)
                    {
                        //crop is grown
                        string name = gScript.cropName;
                        if (!cropCount.ContainsKey(name))
                        {
                            cropCount[name] = 0;
                        }

                        cropCount[name] += 1;
                        cropExists = true;
                        //put back into original parent and Set inactive


                        //childObject.SetActive(false);
                        //childObject.transform.SetParent(GameObject.Find("CropPlaceholder").transform);

                        //Destroy gameobject
                        Destroy(childObject);
           
                    }
                }

            }
            else
            {
                Debug.Log("None grown yet!");
            }
        }
        if (!cropExists)
        {
            StartCoroutine(ActivateFailPanel());
        }
        else
        {
            PrintCrops(cropCount);
        }
    }

    void PrintCrops(Dictionary<string,int> cropCount)
    {
        inventoryText.text = "";

        Debug.Log(inventoryText.text);
        inventoryText.text += "Congrats, you have collected: \n";

        foreach(KeyValuePair<string,int> crop in cropCount) {
            inventoryText.text += crop.Value + "x " + crop.Key + "\n";
           
        }
        inventoryText.text += "Sell your stock at the store.";
        StartCoroutine(ActivateSuccessPanel());
        
    }

    private IEnumerator ActivateFailPanel()
    {
        failPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        inventoryText.text = "";
        failPanel.SetActive(false);
    }
    private IEnumerator ActivateSuccessPanel()
    {
        successPanel.SetActive(true);
        yield return new WaitForSeconds(2);

        successPanel.SetActive(false);
    }
}
