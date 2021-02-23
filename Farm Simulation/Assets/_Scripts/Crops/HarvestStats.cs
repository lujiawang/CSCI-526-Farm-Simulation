using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private GameObject[] cropLands;
    void Awake()
    {
        cropLands = GameObject.FindGameObjectsWithTag("cropLand");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sweep()
    {
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

        PrintCrops(cropCount);
    }

    void PrintCrops(Dictionary<string,int> cropCount)
    {
        foreach(KeyValuePair<string,int> crop in cropCount) {
            Debug.Log("Congrats, you have collected: " + crop.Value + "x " + crop.Key);
        }
    }
}
