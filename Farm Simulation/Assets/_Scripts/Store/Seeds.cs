using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Seeds : MonoBehaviour
{
    private GameObject player;
    private GameObject seedsPlaceholder;
    private GameObject Inventory;
    private Button button;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seedsPlaceholder = GameObject.FindGameObjectWithTag("Seeds");
        Inventory = GameObject.FindGameObjectWithTag("Inventory");

        button = GetComponent<Button>();
    }



    public void AddSeeds()
    {
        string seedName = this.transform.GetChild(0).name;

        foreach (Transform child in this.Inventory.transform.GetChild(0).GetChild(0))
        {
                if (seedName.Equals(child.GetChild(0).GetChild(0).name))
                {
                    child.gameObject.SetActive(true);
                    break;
                }
        }
        button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
