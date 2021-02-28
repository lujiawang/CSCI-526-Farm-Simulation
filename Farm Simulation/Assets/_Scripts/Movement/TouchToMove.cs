using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchToMove : MonoBehaviour
{
    private string MenuName = "";


    private NavMeshAgent agent;
    private Rigidbody2D rb;

    // public Transform target;
    private NavMeshHit hit;

    public static bool isPlayer;

    public Animator animator;



    /*cropLand info: */
    public static string landName = "";



    // Start is called before the first frame update
    void Start()
    {
        isPlayer = true;
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

        // animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        // Debug.Log("Horizontal: "+Input.GetAxis("Horizontal"));
        // animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        // Debug.Log("Velocity: "+ agent.velocity);
        animator.SetFloat("Horizontal", agent.velocity.normalized.x);
        //Debug.Log("Horizontal: "+ agent.velocity.normalized.x);
        animator.SetFloat("Vertical", agent.velocity.normalized.y);
        //Debug.Log("Vertical: "+ agent.velocity.normalized.y);


        /* Need to do sth like !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId), check Input.touchCount > 0 first */

        if (Input.GetMouseButtonDown(0) && isPlayer && EventSystem.current.currentSelectedGameObject== null)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            // Debug.Log(targetPos);
            if (!NavMesh.Raycast(rb.position, targetPos, out hit, NavMesh.AllAreas))
            {
                NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas);
                targetPos = hit.position;
                // Debug.Log("Invalid Point. Newly Generated Point: "+targetPos);
            }
            else
            {
                // Debug.Log("Workable Point: "+targetPos);
            }

            targetPos = SnapToLand(targetPos);


            agent.SetDestination(targetPos);
        }

    }


    public void PlayerEnable()
    {
        isPlayer = !isPlayer;
        Debug.Log("isPlayer: " + isPlayer);
    }




    /* Set the player's destination to the nearest cropland. */
    public Vector3 SnapToLand(Vector3 target)
    {

        //get the current touched object
        RaycastHit2D hitInformation = Physics2D.Raycast(target, Camera.main.transform.forward);

        if (hitInformation.collider != null)
        {
            //We should have hit something with a 2D Physics collider!
            GameObject touchedObject = hitInformation.transform.gameObject;

            // for those planted lands
            if (touchedObject.transform.parent != null && touchedObject.transform.parent.CompareTag("cropLand"))
            {
                TouchToMove.landName = touchedObject.transform.parent.name;
                Debug.Log("Go to harvest -> " + landName);
                return touchedObject.transform.parent.position;
            }

            else if (touchedObject.CompareTag("cropLand"))
            {
                TouchToMove.landName = touchedObject.name;
                Debug.Log("Go to -> " + landName);
                return touchedObject.transform.position;

            }

        }
        landName = "";

        return target;
    }
}
