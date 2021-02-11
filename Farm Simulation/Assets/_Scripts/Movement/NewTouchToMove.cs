using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.EventSystems;

public class NewTouchToMove : MonoBehaviour, IPointerDownHandler
{
    private string MenuName = "";

    private GameObject playerObj;


	private NavMeshAgent agent;
	private Rigidbody2D rb;

	// public Transform target;
	private NavMeshHit hit;

    private bool isPlayer;


    // Start is called before the first frame update
    void Start()
    {
        isPlayer = true;
        playerObj = GameObject.FindWithTag("Player");
    	agent = playerObj.GetComponent<NavMeshAgent>();
    	rb = playerObj.GetComponent<Rigidbody2D>();
    	agent.updateRotation = false;
    	agent.updateUpAxis = false;

    	// target = GameObject.FindGameObjectWithTag("TestTarget").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 targetPos = rb.position;


        if (isPlayer)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            Debug.Log("go!");
            if (!NavMesh.Raycast(rb.position, targetPos, out hit, NavMesh.AllAreas))
            {
                NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas);
                targetPos = hit.position;
                Debug.Log("Invalid Point. Newly Generated Point: "+targetPos);
            }
            else
            {
                Debug.Log("Workable Point: "+targetPos);
            }
            agent.SetDestination(targetPos);
        }
    }

    public void PlayerEnable()
    {
        isPlayer = !isPlayer; 
        Debug.Log("isPlayer: "+ isPlayer);
    }
}
