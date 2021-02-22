using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    public class Crop
    {
        public string cropName;
        public int reward;

        public Crop(string cn, int rwd)
        {
            cropName = cn;
            reward = rwd;
        }
    }



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
                
                gain = reward;
                Crop crop = new Crop(gScript.cropName, reward);
                Debug.Log("Sold " + crop.cropName + " for: " + crop.reward + " coins!");
            }

            //return specifics of the crop. Change function to return Crop object later on.

            
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
