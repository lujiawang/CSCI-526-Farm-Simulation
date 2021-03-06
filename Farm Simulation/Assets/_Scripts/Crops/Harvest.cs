using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Harvest : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject childObject;
    public Text notificationText;
    public HarvestStats hs;
    void Start()
    {
        

    }

    public void SetObject(GameObject gameObject)
    {
        childObject = gameObject;
    }

    public void CollectCrop()
    {
        
        //clicked on nothing
        
        if (childObject == null || childObject.transform.parent == null)
        {
            Debug.Log("Not clicking anywhere near the fields");
            hs.ActivateFail(1);
            return;
        }
        

        //clicked on cropLand instead

        if (childObject.CompareTag("cropLand"))
        {
            //check for child just in case

            if (childObject.transform.childCount <= 0)
            {
                Debug.Log("Theres nothing on the field");
                hs.ActivateFail(2);
                return;
            }

            GameObject checkChild = childObject.transform.GetChild(0).gameObject;
            
            if (checkChild)
            {
                CropGrowing gScript= checkChild.GetComponent<CropGrowing>();
                if (gScript.grown)
                {
                    string cropName = gScript.name;
                    Debug.Log("Harvesting " + cropName);
                    hs.AddToInventory(cropName);
                    notificationText.text = "";
                    Destroy(checkChild);
                    return;
                }

            }


            
            
        } 


        //clicked on the crop itself

        CropGrowing cropGrowing = childObject.GetComponent<CropGrowing>();

        if (cropGrowing == null)
        {
            Debug.Log("Collecting right after planting");
            hs.ActivateFail(3);
            return;
        }


        if (cropGrowing.grown)
        {
            string cropName = childObject.name;
            Debug.Log("Harvesting " + cropName);
            hs.AddToInventory(cropName);
            notificationText.text = "";
            Destroy(childObject);
        }
        else
        {
            Debug.Log("Has yet to grow");
            hs.ActivateFail(3);
            return;
        }
    }

}
