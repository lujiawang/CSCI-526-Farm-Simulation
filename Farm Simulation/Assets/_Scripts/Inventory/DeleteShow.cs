using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeleteShow : MonoBehaviour
{
	InventoryUI cScript;
	GraphicRaycaster m_Raycaster;
	EventSystem m_EventSystem;
	PointerEventData m_PointerEventData;

	void Start()
	{
		cScript = this.transform.parent.gameObject.GetComponent<InventoryUI>();
		m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
		m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}


	public void ShowHide()
	{
		bool isOn = cScript.SetDeleteParam();
		// start listening for events if deletebuttons are on
		if(isOn)
			StartCoroutine(DisableDeleteIfClickedElsewhere());

		Button button = this.GetComponent<Button>();
		Image buttonImg = this.GetComponent<Image>();
		buttonImg.color = isOn ? button.colors.highlightedColor : button.colors.normalColor;
	}

	IEnumerator DisableDeleteIfClickedElsewhere()
	{
		bool endLoop = false;
		bool clickedDeleteButton = false;
		bool clickedDeleteShowButton = false;
		while(!endLoop)
		{
			// Debug.Log("disable deletebutton loop running");
			if (Input.GetKey(KeyCode.Mouse0) || (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended) )
	        {
	            //Set up the new Pointer Event
	            m_PointerEventData = new PointerEventData(m_EventSystem);
	            //Set the Pointer Event Position to that of the mouse position
	            m_PointerEventData.position = Input.mousePosition;

	            //Create a list of Raycast Results
	            List<RaycastResult> results = new List<RaycastResult>();

	            //Raycast using the Graphics Raycaster and mouse click position
	            m_Raycaster.Raycast(m_PointerEventData, results);

	            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
	            foreach (RaycastResult result in results)
	            {
	                // Debug.Log("Hit " + result.gameObject.name);
	                if(result.gameObject.name == "DeleteButton")
	                {
	                	clickedDeleteButton = true;
	                }
	                else if(result.gameObject.name == "DeleteShowButton")
	                {
	                	clickedDeleteShowButton = true;
	                }
	            }
	            if(clickedDeleteShowButton) // end loop immediately if clicked the delete toggle
	            	endLoop = true;
	            else if(!clickedDeleteButton) //else if did not click any deletebuttons
	            {
	            	endLoop = true;
	            	ShowHide();
	            }else{ //reset flags for the next event
	            	clickedDeleteButton = false;
	            	clickedDeleteShowButton = false;
	            }
	        }
	        yield return null;
		}
		
	}

}
