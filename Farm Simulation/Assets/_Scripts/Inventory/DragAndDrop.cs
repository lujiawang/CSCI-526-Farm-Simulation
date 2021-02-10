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

	private bool isCrop;

	private Camera cam;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>().rootCanvas;
		canvasGroup = GetComponent<CanvasGroup>();

		isCrop = CompareTag("crop");
		cam = Camera.main;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
        if (isCrop) { 
			canvasGroup.alpha = 0.6f;
			canvasGroup.blocksRaycasts = false;
		}
		// Debug.Log("OnBeginDrag");
		
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (isCrop)
		{
			// Debug.Log("OnDrag");
			rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (isCrop)
		{
			// Debug.Log("OnEndDrag");
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;

			GameObject CropParent = GameObject.Find("Crops");
			GameObject newItem = new GameObject(this.name);
			newItem.transform.SetParent(CropParent.transform);

			newItem.AddComponent<SpriteRenderer>();
			SpriteRenderer sr = newItem.GetComponent<SpriteRenderer>();
			sr.sprite = this.GetComponent<Image>().sprite;
			

			Vector2 pos = cam.ScreenToWorldPoint(transform.position);
			newItem.transform.position = pos;

			newItem.transform.localScale = new Vector2(0.5f, 0.5f);
			
			gameObject.SetActive(false);
			
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
