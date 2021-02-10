using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class TouchToMove : MonoBehaviour
{
    
	private NavMeshAgent agent;
	private Rigidbody2D rb;

	// public Transform target;
	private NavMeshHit hit;

    public GameObject InventorySystem;
    private CanvasGroup inventCanvas;
    private bool isPlayer;


    // Start is called before the first frame update
    void Start()
    {
        isPlayer = true;
    	agent = GetComponent<NavMeshAgent>();
    	rb = GetComponent<Rigidbody2D>();
    	agent.updateRotation = false;
    	agent.updateUpAxis = false;

        inventCanvas = InventorySystem.GetComponent<CanvasGroup>();
    	// target = GameObject.FindGameObjectWithTag("TestTarget").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.Log(isPlayer);            
        if (isPlayer)
        {
            inventCanvas.alpha = 0f; //this makes everything transparent
            inventCanvas.blocksRaycasts = false; //this prevents the UI element to receive input events
        }
        else
        {
            inventCanvas.alpha = 1f; 
            inventCanvas.blocksRaycasts = true;

        }

        if (Input.GetKeyDown(KeyCode.M)) {

            isPlayer = !isPlayer;
        }

    	Vector3 targetPos = rb.position;

    	/* the following block is for Smartphone */

    	// int i = 0;
    	// // loop over all the touches
    	// while(i < Input.touchCount){
    	// 	// boundary limits go in here
    	// 	if(true){
    	// 		targetPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
    	// 		targetPos.z = 0;
    	// 		Debug.Log(targetPos);
    	// 		agent.SetDestination(target.position);
    	// 	}
    	// 	i++;
    	// }

    	if(Input.GetMouseButtonDown(0) && isPlayer){
			targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPos.z = 0;
			// Debug.Log(targetPos);
			if(!NavMesh.Raycast(rb.position, targetPos, out hit, NavMesh.AllAreas)){
				NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas);
				targetPos = hit.position;
				// Debug.Log("Invalid Point. Newly Generated Point: "+targetPos);
			}else{
				// Debug.Log("Workable Point: "+targetPos);
			}
			agent.SetDestination(targetPos);
    	}

        
    }
}
