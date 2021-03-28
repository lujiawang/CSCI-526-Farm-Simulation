using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RestoreCropGrowing : MonoBehaviour
{
    // Start is called before the first frame update
	InternetTime internetTime;
	
    void Start()
    {
        if(PlayerPrefs.HasKey(this.name))
        {
        	string cropName = PlayerPrefs.GetString(this.name);
        	if(cropName == "") 
        		return;
        	StartCoroutine(RestoreCrop(cropName));
        }
    }

    IEnumerator RestoreCrop(string cropName)
    {
    	GameObject CropParent = GameObject.Find("CropPlaceholder");
    	// Debug.Log(CropParent.name);
    	foreach (Transform child in CropParent.transform)
	    {
	        if (child.name  == cropName)
	        {
	        	GameObject cropObj = child.gameObject;
	        	GameObject copyCrop = Instantiate(cropObj);
		        copyCrop.name = cropObj.name;
		        copyCrop.transform.position = this.transform.position;
		        copyCrop.transform.SetParent(this.transform);

		        // handle CropGrowing related stats
		        int runningPointer = PlayerPrefs.GetInt(this.name + "runningPointer");
		        int stage = PlayerPrefs.GetInt(this.name +"stage");

		        CropGrowing cScript = copyCrop.GetComponent<CropGrowing>();
	            
		        // the following block uses internet time for growing crops
                internetTime = InternetTime.instance;
		        if(internetTime.useTimeForCrops)
		        {
    		        yield return StartCoroutine(internetTime.FetchTime());
    		        if(PlayerPrefs.HasKey("exitInternetTime"))
    		        {
    		        	// Debug.Log("exitInternetTime: "+PlayerPrefs.GetString("exitInternetTime"));
    		        	DateTime prevInternetTime = DateTime.Parse(PlayerPrefs.GetString("exitInternetTime"));
    		        	TimeSpan diffInternetTime = internetTime.GetTime().Subtract(prevInternetTime);
    		        	Debug.Log("diffInternetTime: "+diffInternetTime.TotalSeconds);
    		        	double diffSeconds = diffInternetTime.TotalSeconds;
    		        	if(diffSeconds < 0)
    		        		diffSeconds = 0d;
    
    		        	float totalSecondsForGrowth = cScript.GetTotalSecondsForGrowth();
    		        	int totalStages = cScript.GetTotalStages();
    		        	runningPointer += (int)diffSeconds;
    		        	stage += (int)(diffSeconds / cScript.growTime);
    		        	runningPointer = runningPointer>totalSecondsForGrowth?(int)totalSecondsForGrowth:runningPointer;
    		        	stage = stage>totalStages?totalStages:stage;
    		        }
		        }

		        cScript.SetState(runningPointer, stage);

		        copyCrop.SetActive(true);
	            break;
	        }
	    }
	    yield return null;
    }

    void OnDestroy()
    {
    	SaveCropGrowing();
    }

    void SaveCropGrowing()
    {
    	string landName = this.name;
    	if(this.transform.childCount == 0)
    	{
    		PlayerPrefs.SetString(landName, "");
    		return;
    	}
    	GameObject crop = this.transform.GetChild(0).gameObject;
    	CropGrowing cScript = crop.GetComponent<CropGrowing>();

    	PlayerPrefs.SetString(landName, crop.name);
    	PlayerPrefs.SetInt(landName+"runningPointer", cScript.GetTime());
    	PlayerPrefs.SetInt(landName+"stage", cScript.GetStage());
    }

}
