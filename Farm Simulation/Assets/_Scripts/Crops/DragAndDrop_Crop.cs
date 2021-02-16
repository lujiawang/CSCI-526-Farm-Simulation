using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop_Crop : MonoBehaviour
{
    private bool inPosition; //whether it is on the land
    private Vector2 snapSensitivity = new Vector2(1.8f, 1.0f);

    private GameObject[] cropLands;

    // Start is called before the first frame update
    void Start()
    {
        inPosition = false;

        cropLands = GameObject.FindGameObjectsWithTag("cropLand");
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnMouseDrag()
    {
        if (!inPosition)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x, cursorPos.y); 
            
            
            foreach (GameObject cropLand in cropLands)
            {
                if (cropLand.transform.childCount == 0 && Mathf.Abs(this.transform.position.x - cropLand.transform.position.x) <= snapSensitivity.x
                    && Mathf.Abs(this.transform.position.y - cropLand.transform.position.y) <= snapSensitivity.y)
                {
                    Debug.Log("snap to " + cropLand.name);
                    this.transform.position = cropLand.transform.position; //clip to the position
                    this.transform.SetParent(cropLand.transform);
                    inPosition = true; //and stop dragging
                    break;
                }
            }
        }
    }



}
