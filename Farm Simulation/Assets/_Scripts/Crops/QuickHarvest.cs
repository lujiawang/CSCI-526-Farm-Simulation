using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class QuickHarvest : MonoBehaviour
{
	int layerMask; // ignore player
	GameObject player;
	GameObject canvas;

	Inventory inventory;


    // Start is called before the first frame update
    void Start()
    {
    	layerMask = LayerMask.GetMask("Player")-1;
        // player = GameObject.Find("Player");
        player = this.gameObject;
        canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];

        inventory = Inventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
    	// for desktop, when left key is held down
    	// Debug.Log(Input.GetKey(KeyCode.Mouse0));
    	if(Input.GetKey(KeyCode.Mouse0) && !TouchToMove.IsPointerOverGameObject())
    	{
    		HarvestCrop();
    	}
    	// for mobile
    	else if (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && !TouchToMove.IsPointerOverGameObject())
        {
        	HarvestCrop();
    	}
    }

    void HarvestCrop()
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
	            }else //harvest the crop
	            {
	            	inventory.Add(cropObj.name, Item.RandomHarvest(cropObj.name));				


					Animator anim = cropObj.GetComponent<Animator>();
					anim.Play("Harvest");
				}
			}
			
		}
    }
}
