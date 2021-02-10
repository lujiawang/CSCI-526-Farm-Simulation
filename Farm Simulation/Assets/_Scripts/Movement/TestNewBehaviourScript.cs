using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNewBehaviourScript : MonoBehaviour
{

	public Transform target;
	private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
    	target = GameObject.FindGameObjectWithTag("TestTarget").transform;
    	agent = GetComponent<NavMeshAgent>();
    	agent.updateRotation = false;
    	agent.updateUpAxis = false;
        
    }

    // Update is called once per frame
    void Update()
    {
    	// agent.Warp(agent.transform);
    	agent.SetDestination(target.position);
        
    }
}
