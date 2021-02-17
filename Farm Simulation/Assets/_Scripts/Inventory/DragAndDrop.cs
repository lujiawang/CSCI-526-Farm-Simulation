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

	GameObject CropParent;
	GameObject realCrop; //the corresponding object that pre-saved inside CropParent;

	private bool inPosition; //whether it is on the land
	private Vector2 snapSensitivity = new Vector2(1.8f, 1.0f);

	private GameObject[] cropLands;

	private Camera cam;


	private void Awake()
	{
		//get pre-defined children
		CropParent = GameObject.Find("CropPlaceholder"); //the placeholder object named Crops in scene

		foreach (Transform child in CropParent.transform)
        {
			if (child.name.Equals(this.name))
            {
				realCrop = child.gameObject;
				break;
            }
        }

		inPosition = false;

		cropLands = GameObject.FindGameObjectsWithTag("cropLand");


		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>().rootCanvas;
		canvasGroup = GetComponent<CanvasGroup>();

		cam = Camera.main;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
        if (!inPosition) { 
			canvasGroup.alpha = 0.6f;
			canvasGroup.blocksRaycasts = false;
		}
		// Debug.Log("OnBeginDrag");
		
		
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!inPosition)
		{
			// Debug.Log("OnDrag");
			rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!inPosition) //when drag out from the menu, switch to the real crop
		{
			// Debug.Log("OnEndDrag");
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;

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
		if (!inPosition)
		{
			Vector2 pos = cam.ScreenToWorldPoint(transform.position);
			foreach (GameObject cropLand in cropLands)
			{
				if (cropLand.transform.childCount == 0 && Mathf.Abs(pos.x - cropLand.transform.position.x) <= snapSensitivity.x
					&& Mathf.Abs(pos.y - cropLand.transform.position.y) <= snapSensitivity.y)
				{
					this.gameObject.SetActive(false);
					realCrop.SetActive(true);
					realCrop.transform.position = cropLand.transform.position; //clip to the position
					realCrop.transform.SetParent(cropLand.transform);
					inPosition = true; //and stop dragging
					break;
				}
			}
		}
	}
}
