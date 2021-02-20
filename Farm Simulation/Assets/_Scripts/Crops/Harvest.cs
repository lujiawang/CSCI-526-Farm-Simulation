using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject childObject;
    private CropGrowing gScript;
    private int gain;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collectCrop()
    {
        if (this.transform.childCount > 0)
        {
            childObject = this.transform.GetChild(0).gameObject;
            Debug.Log("Just clicked something");
            if(childObject) gScript = childObject.GetComponent<CropGrowing>();


            Debug.Log("Time to harvest");

            if (childObject == null) return;
            int reward = gScript.harvestCrop();
            if (reward > 0)
            {
                Debug.Log("Sold " + gScript.cropName + " for: " + reward + " coins!");
                gain = reward;
            }

        }
        else
        {
            Debug.Log("None grown yet!");
        }

       
    }

    public int getReward()
    {
        return gain;
    }
}
