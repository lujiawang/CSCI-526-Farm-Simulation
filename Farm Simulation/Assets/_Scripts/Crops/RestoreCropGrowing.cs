using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreCropGrowing : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        if(PlayerPrefs.HasKey(this.name))
        {
        	string cropName = PlayerPrefs.GetString(this.name);
        	if(cropName == "") 
        		return;
        	RestoreCrop(cropName);
        }
    }

    void RestoreCrop(string cropName)
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
		        bool started = PlayerPrefs.GetInt(this.name +"started")==1?true:false;

		     //    IEnumerator UpdateTimeImmediately() // note that the function type IEnumerator cannot change
			    // {
			    //     yield return StartCoroutine(internetTime.FetchTime());
			    //     // This is after internetTime is updated. Do stuff here. 
			    // }

		        CropGrowing cScript = copyCrop.GetComponent<CropGrowing>();
	            cScript.SetState(runningPointer, stage, started);

		        copyCrop.SetActive(true);
	            break;
	        }
	    }        
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
    	PlayerPrefs.SetInt(landName+"started", cScript.GetStartedParam()?1:0);

    }

}
