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

    // public static bool isPlayer;

    public Animator animator;


    /*cropLand info: */
    public static string landName = "";
    public Text cropName;



    // Start is called before the first frame update
    void Start()
    {
        // isPlayer = true;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // initialize player's starter position
        if(!PlayerPrefs.HasKey("PlayerPositionX"))
        {
            PlayerPrefs.SetFloat("PlayerPositionX", 0.45f);
            PlayerPrefs.SetFloat("PlayerPositionY", -1.5f);
            PlayerPrefs.SetFloat("PlayerPositionZ", 0f);
        }
        this.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPositionX"),
            PlayerPrefs.GetFloat("PlayerPositionY"),PlayerPrefs.GetFloat("PlayerPositionZ"));

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

        if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject() )
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

    // OnDestroy is called before exiting the game
    void OnDestroy()
    {
        // save player's new position
        PlayerPrefs.SetFloat("PlayerPositionX", this.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", this.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", this.transform.position.z);
    }

    public static bool IsPointerOverGameObject(){
        //check mouse
        if(EventSystem.current.IsPointerOverGameObject())
            return true;
         
        //check touch
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ){
            if(EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }
         
        return false;
    }


    // public void PlayerEnable()
    // {
    //     isPlayer = !isPlayer;
    //     Debug.Log("isPlayer: " + isPlayer);
    // }




    /* Set the player's destination to the nearest cropland. */
    public Vector3 SnapToLand(Vector3 target)
    {

        //get the current touched object
        RaycastHit2D hitInformation = Physics2D.Raycast(target, Camera.main.transform.forward);

        GameObject touchedObject = null;
        

        if (hitInformation.collider != null)
        {
            //We should have hit something with a 2D Physics collider!
            touchedObject = hitInformation.transform.gameObject;
            PassGameObject(touchedObject);
            // for those planted lands
            if (touchedObject.transform.parent != null && touchedObject.transform.parent.CompareTag("cropLand"))
            {
                TouchToMove.landName = touchedObject.transform.parent.name;
                Debug.Log("Go to harvest -> " + landName);

                
                CropGrowing cropGrowing = touchedObject.GetComponent<CropGrowing>();
                if(cropGrowing != null)
                {
                    if (cropGrowing.grown)
                    
                        cropName.text = touchedObject.name + " is grown";
                        
                    
                        
                        
                    else
                        cropName.text = touchedObject.name + " is not grown yet";
                        
                    
                        
                }
                
                return touchedObject.transform.parent.position;
            }

            else if (touchedObject.CompareTag("cropLand"))
            {
                TouchToMove.landName = touchedObject.name;
                Debug.Log("Go to harvest -> " + landName);
                //handle case where crop land is touched instead of the crop itself 
                if (touchedObject.transform.childCount > 0)
                {
                    GameObject childObject = touchedObject.transform.GetChild(0).gameObject;
                    CropGrowing cropGrowing = childObject.GetComponent<CropGrowing>();

                    if (cropGrowing != null)
                    {
                        if (cropGrowing.grown)

                            cropName.text = cropGrowing.name + " is grown";




                        else
                            cropName.text = cropGrowing.name + " is not grown yet";



                    }

                    return childObject.transform.position;




                }

                cropName.text = "Drag a crop here to grow it!";
                Debug.Log("Go to -> " + landName);
                return touchedObject.transform.position;

            }
            

        }
        PassGameObject(touchedObject);
        landName = "";

        return target;
    }

    public void PassGameObject(GameObject touchedObject)
    {
        //detect if player is near a crop land that allows for collection
        
        GameObject player = GameObject.Find("Player");
        Harvest hvt = player.GetComponent<Harvest>();
        hvt.SetObject(touchedObject);
    }
    

    
}
