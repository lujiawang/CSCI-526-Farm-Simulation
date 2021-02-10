using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
	private Canvas canvas;
	private RectTransform rectTransform;
	private CanvasGroup canvasGroup;

	private bool isCrop;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>().rootCanvas;
		canvasGroup = GetComponent<CanvasGroup>();

		isCrop = CompareTag("crop");
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
