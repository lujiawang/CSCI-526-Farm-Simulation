using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickHarvest : MonoBehaviour
{
	int layerMask; // ignore player
	GameObject player;
	GameObject canvas;

	Inventory inventory;

	public static bool disableQuickHarvest = false;
	public Text cropNotification;
    // Start is called before the first frame update
    void Start()
    {
    	layerMask = LayerMask.GetMask("Player")-1;
        // player = GameObject.Find("Player");
        player = this.gameObject;
        canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];

        inventory = Inventory.instance;

        StartCoroutine(UpdateCOR());
    }

    IEnumerator UpdateCOR()
    {
    	while(true)
    	{
    		// for desktop, when left key is held down
	    	// Debug.Log(Input.GetKey(KeyCode.Mouse0));
	    	if(!disableQuickHarvest && Input.GetKey(KeyCode.Mouse0) && !TouchToMove.IsPointerOverGameObject())
	    	{
	    		// Debug.Log("MouseDown");
	    		yield return StartCoroutine(HarvestCrop());
	    	}
	    	// for mobile
	    	else if(!disableQuickHarvest && Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && 
	    		Input.GetTouch(0).phase != TouchPhase.Canceled && !TouchToMove.IsPointerOverGameObject())
	        {
	        	yield return StartCoroutine(HarvestCrop());
	    	}
	    	yield return null;
    	}
    	
    }

    IEnumerator HarvestCrop()
    {
    	// Debug.Log("HarvestCrop");
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hitInformation = Physics2D.Raycast(mousePos3D, Camera.main.transform.forward);
		if(hitInformation.collider != null)
		{
			GameObject touchedObject = hitInformation.transform.gameObject;
			GameObject cropObj = null;
			// touched cropland
			if (touchedObject.CompareTag("cropLand") && touchedObject.transform.childCount != 0)
				cropObj = touchedObject.transform.GetChild(0).gameObject;
			// or touched the crop on cropland
			else if(touchedObject.transform.parent != null && 
				touchedObject.transform.parent.gameObject.CompareTag("cropLand"))
				cropObj = touchedObject;

			// if touched a crop and crop is grown
			if(cropObj != null && cropObj.GetComponent<CropGrowing>().grown)
			{
				Vector3 playerPos = player.transform.position;
				RaycastHit2D hitInfo = Physics2D.Raycast(playerPos, Camera.main.transform.forward, 
					Mathf.Infinity, layerMask);
				// if player is not on any one cropLand
	            if(hitInfo.collider == null || !( hitInfo.transform.gameObject.CompareTag("cropLand") || 
	                ( hitInfo.transform.parent != null && hitInfo.transform.parent.gameObject.CompareTag("cropLand") ) ))
	            {
	            	ShowToast cScript = canvas.GetComponent<ShowToast>();
	                cScript.showToast("Walk to any cropland to harvest!", 1);
	            }else if(PlayerStats.Harvest && !cropObj.GetComponent<CropGrowing>().Harvested())//harvest the crop
	            {
	            	cropObj.GetComponent<CropGrowing>().SetHarvested(true);
	            	// Debug.Log("Harvest");
	            	int randomHarvest = Item.RandomHarvest(cropObj.name);
	            	// change tab if necessary
		            InventoryUI uiScript = GameObject.Find("Inventory").GetComponent<InventoryUI>();
		            uiScript.ToggleRespectiveShowButton(Item.GetItemId(cropObj.name));
	            	inventory.Add(cropObj.name, randomHarvest);

	            	// show harvest number message
	            	GameObject progressCanvas = GameObject.Find("ProgressCanvas");
	            	if(progressCanvas == null)
	            		Debug.LogWarning("Cannot find progressCanvas!!");
            		string cropLandName = cropObj.transform.parent.name;
            		int index = cropLandName[cropLandName.Length - 2] - '0';
            		switch(index)
            		{
            			case 1:
            				index = 6;
            				break;
            			case 2:
            				index = 4;
            				break;
            			case 3:
            				index = 7;
            				break;
            			case 6:
            				index = 1;
            				break;
            			case 4:
            				index = 2;
            				break;
            			case 7:
            				index = 3;
            				break;
            		}
            		Text text = progressCanvas.transform.GetChild(index).Find("HarvestNum").GetComponent<Text>();

            		ShowToast cScript = canvas.GetComponent<ShowToast>();
            		cScript.showCustomizedToast(text, "+"+randomHarvest, 1);
					
					// GameObject animObj = Instantiate(cropObj, cropObj.transform.position, cropObj.transform.rotation);
					// animObj.name = cropObj.name;
					// animObj.GetComponent<CropGrowing>().SetHarvested(true);
					// Sprite sprite = cropObj.GetComponent<CropGrowing>().GetSprite();
					// StartCoroutine(animObj.GetComponent<CropGrowing>().SetSprite(sprite));
					// Debug.Log("RealCrop: "+cropObj.transform.position);
					// Debug.Log("RealCrop: "+cropObj.transform.localPosition);
					// Debug.Log("AnimCrop: "+animObj.transform.position);
					// Debug.Log("AnimCrop: "+animObj.transform.localPosition);
					// Destroy(cropObj);
					// Animator anim = animObj.GetComponent<Animator>();
					// anim.Play("Harvest");

					Animator anim = cropObj.GetComponent<Animator>();
					anim.Play("Harvest");
					cropNotification.text = "";
					yield return null;
				}
			}
			
		}
    }
}
