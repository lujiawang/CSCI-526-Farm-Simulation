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


    // Start is called before the first frame update
    void Start()
    {
    	agent = GetComponent<NavMeshAgent>();
    	rb = GetComponent<Rigidbody2D>();
    	agent.updateRotation = false;
    	agent.updateUpAxis = false;

    	// target = GameObject.FindGameObjectWithTag("TestTarget").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
    	
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

    	if(Input.GetMouseButtonDown(0)){
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
