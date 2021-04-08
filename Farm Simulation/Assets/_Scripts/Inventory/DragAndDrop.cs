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

    // private GameObject player;

    // GameObject CropParent;
    // GameObject realCrop; //the corresponding object that pre-saved inside CropParent;

    Transform buttonParent; //the itembutton parent

    private bool endDrag; // flag to end dragging

    private Vector3 initialPos;

    Inventory inventory;

    private int layerMask;

    SoundManager soundManager;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>().rootCanvas;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // player = GameObject.FindGameObjectsWithTag("Player")[0];

        // //get pre-defined children
        // CropParent = GameObject.Find("CropPlaceholder"); //the placeholder object named Crops in scene

        // foreach (Transform child in CropParent.transform)
        // {
        //     if (child.name + "Seed" == (this.name))
        //     {
        //         realCrop = child.gameObject;
        //         break;
        //     }
        // }

        buttonParent = this.transform.parent;

        // disable dragability of "Item" if is not seed
        if(!this.name.Contains("Seed"))
        {
            this.GetComponent<CanvasGroup>().interactable = false;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }else if(!StoreToggle.isStoreOpen) // enable dragability of "Item" if store is closed
        {
            this.GetComponent<CanvasGroup>().interactable = true;
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        endDrag = false;
        
        initialPos = rectTransform.anchoredPosition;

        inventory = Inventory.instance;

        layerMask = LayerMask.GetMask("Player")-1;

        soundManager = SoundManager.instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TouchToMove.disablePlayerMovement = true;
        QuickHarvest.disableQuickHarvest = true;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        // reparent the crop so it is not affected by Mask component of parentPanel
        this.transform.SetParent(canvas.transform);
        // Debug.Log("OnBeginDrag");
    }

    

    public void OnDrag(PointerEventData eventData)
    {  
        if(endDrag)
            return;
        // continue dragging
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if(!TouchToMove.IsPointerOverGameObject()){
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 mousePos2D = new Vector2(mousePos3D.x, mousePos3D.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(mousePos3D, Camera.main.transform.forward);

            if(hitInformation.collider != null)
            {
                GameObject touchedObject = hitInformation.transform.gameObject;
                // // touched occupied land or crop itslef
                // if( (touchedObject.CompareTag("cropLand") && touchedObject.transform.childCount != 0) || 
                //     ( touchedObject.transform.parent != null && touchedObject.transform.parent.CompareTag("cropLand") ))
                // {
                //     ShowToast cScript = canvas.GetComponent<ShowToast>();
                //     cScript.showToast("Cannot plant on occupied land!", 1);
                //     endDrag = true;
                //     canvasGroup.alpha = 1f;
                //     canvasGroup.blocksRaycasts = true;
                //     // set back to original inventoryslot parent
                //     this.transform.SetParent(buttonParent);
                //     rectTransform.anchoredPosition = initialPos;
                // }

                // touched empty land
                if (touchedObject.CompareTag("cropLand") && touchedObject.transform.childCount == 0)
                {
                    GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
                    Vector3 playerPos = player.transform.position;
                    RaycastHit2D hitInfo = Physics2D.Raycast(playerPos, Camera.main.transform.forward, 
                        Mathf.Infinity, layerMask);
                    // if player is not on any one cropLand
                    if(hitInfo.collider == null || !( hitInfo.transform.gameObject.CompareTag("cropLand") || 
                        ( hitInfo.transform.parent != null && hitInfo.transform.parent.gameObject.CompareTag("cropLand") ) ))
                    {
                        // Debug.Log("LayerMask: "+layerMask);
                        // Debug.Log("Collided: "+hitInfo.transform.gameObject.name);
                        ShowToast cScript = canvas.GetComponent<ShowToast>();
                        cScript.showToast("Walk to any cropland to plant!", 1);
                        endDrag = true;
                        canvasGroup.alpha = 1f;
                        canvasGroup.blocksRaycasts = true;
                        // set back to original inventoryslot parent
                        this.transform.SetParent(buttonParent);
                        rectTransform.anchoredPosition = initialPos;
                    }else // plant the crop
                    {
                        PlantCrop(touchedObject);
                        bool droppedToZero = true;
                        foreach(Item item in inventory.items)
                        {
                            if(item.Name() == this.name)
                            {
                                droppedToZero = false;
                                break;
                            }
                        }
                        if(droppedToZero)
                        {
                            Destroy(this.gameObject);
                            TouchToMove.disablePlayerMovement = false;
                            QuickHarvest.disableQuickHarvest = false;
                            endDrag = false;
                        }
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("onenddrag");
        TouchToMove.disablePlayerMovement = false;
        QuickHarvest.disableQuickHarvest = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        endDrag = false;
        if(buttonParent != null && buttonParent.name != "ItemButton")
        {
            Destroy(this.gameObject);
            return;
        }
        // set back to original inventoryslot parent
        this.transform.SetParent(buttonParent);
        rectTransform.anchoredPosition = initialPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    private void PlantCrop(GameObject cropLand)
    {
        soundManager.PlaySound(9);

        GameObject CropParent = GameObject.Find("CropPlaceholder"); //the placeholder object named Crops in scene
        foreach (Transform child in CropParent.transform)
        {
            if (child.name + "Seed" == (this.name))
            {
                GameObject copyCrop = Instantiate(child.gameObject);
                copyCrop.name = child.gameObject.name;
                copyCrop.transform.position = cropLand.transform.position;
                copyCrop.transform.SetParent(cropLand.transform);
                copyCrop.SetActive(true);

                inventory.Add(this.name, -1);
                break;
            }
        }
        
    }

}
