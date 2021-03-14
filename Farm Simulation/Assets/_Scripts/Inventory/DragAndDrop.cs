﻿using System.Collections;
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
    public static bool canDrag; //whether it is on the way

    GameObject warning;
    private Vector3 initialPos;

    private Camera cam;

    Inventory inventory;
    


    private void Awake()
    {
        //get pre-defined children
        CropParent = GameObject.Find("CropPlaceholder"); //the placeholder object named Crops in scene

        warning = GameObject.FindGameObjectWithTag("Warning");


        foreach (Transform child in CropParent.transform)
        {
            if (child.name.Equals(this.name))
            {
                realCrop = child.gameObject;
                break;
            }
        }

        inPosition = false;
        canDrag = MenuAppear.isMenu;


        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>().rootCanvas;
        canvasGroup = GetComponent<CanvasGroup>();
        
        
        initialPos = rectTransform.anchoredPosition;

        cam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!inPosition && canDrag)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            canDrag = true;
        }
        // Debug.Log("OnBeginDrag");


    }

    public void OnDrag(PointerEventData eventData)
    {
        if (TouchToMove.landName == "")
        {
            warning.transform.GetChild(0).gameObject.SetActive(true);//the warning button
            warning.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Please walk to a crop land first"; //warning text
            Time.timeScale = 0f;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canDrag = false;
        }
        
        else if (!inPosition && canDrag)
        {
            // Debug.Log("OnDrag");
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!inPosition && canDrag) //when drag out from the menu, switch to the real crop
        {
            // Debug.Log("OnEndDrag");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canDrag = false;
        } else
        {
            Debug.Log("drag");
            canDrag = false;
        }


        if (!canDrag)
        {
            rectTransform.anchoredPosition = initialPos;
            canDrag = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
    }

    // Update is called once per frame
    void Update()
    {

        if (!inPosition && canDrag)
        {
            Vector2 pos = cam.ScreenToWorldPoint(transform.position);

            //get the current touched object
            RaycastHit2D hitInformation = Physics2D.Raycast(pos, Camera.main.transform.forward);

            if (TouchToMove.landName != "" && hitInformation.collider != null)
            {
                GameObject cropLand = GameObject.Find(TouchToMove.landName);

                //whether it hit the overall CropLands
                GameObject touchedObject = hitInformation.transform.gameObject;

                //if trying to plant on non-empty land, return to original position
                if(cropLand.transform.childCount > 0)
                {
                    warning.transform.GetChild(0).gameObject.SetActive(true);
                    warning.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Please walk to an empty crop land";
                    rectTransform.anchoredPosition = initialPos;
                    Time.timeScale = 0f;

                    canvasGroup.alpha = 1f;
                    canvasGroup.blocksRaycasts = true;
                    canDrag = false;
                }

                else if (touchedObject.CompareTag("cropLand") && cropLand != null)
                {
                    Debug.Log("plant on: " + cropLand.name);

                    touchedObject.GetComponent<AudioSource>().Play();

                    this.gameObject.SetActive(false);
                    realCrop.SetActive(true);
                    realCrop.transform.position = cropLand.transform.position; //clip to the position
                    realCrop.transform.SetParent(cropLand.transform);
                    inPosition = true; //and stop dragging

                    inventory.Add(touchedObject.transform.GetChild(0).gameObject.name, -1);


                }

            }
        }
    }
}
