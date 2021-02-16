using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
	private Canvas canvas;
	private RectTransform rectTransform;
	private CanvasGroup canvasGroup;

	private bool inMenu;
	GameObject CropParent;
	GameObject realCrop; //the corresponding object that pre-saved inside CropParent;

	private Camera cam;

	private void Awake()
	{
		//get pre-defined children
		CropParent = GameObject.Find("CropPlaceholder"); //the placeholder object named Crops in scene

		string name = this.name;//this.GetComponent<Image>().sprite.name;
		foreach (Transform child in CropParent.transform)
        {
			if (child.name.Equals(name))
            {
				realCrop = child.gameObject;
				break;
            }
        }

		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>().rootCanvas;
		canvasGroup = GetComponent<CanvasGroup>();

		inMenu = true; // see if the crop is in Canvas or the real World
		cam = Camera.main;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
        if (inMenu) { 
			canvasGroup.alpha = 0.6f;
			canvasGroup.blocksRaycasts = false;
		}
		// Debug.Log("OnBeginDrag");
		
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (inMenu)
		{
			// Debug.Log("OnDrag");
			rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (inMenu) //when drag out from the menu, switch to the real crop
		{
			// Debug.Log("OnEndDrag");
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;

			inMenu = false;
			this.gameObject.SetActive(false);
			realCrop.SetActive(true);
			Vector2 pos = cam.ScreenToWorldPoint(transform.position);
			realCrop.transform.position = pos;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		// Debug.Log("OnPointerDown");
	}

	public void OnDrop(PointerEventData eventData){

	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
	}
}
