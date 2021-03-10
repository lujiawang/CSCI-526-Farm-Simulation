﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{

    private GameObject harvesetPalceholder;
    private List<GameObject> children;

    // Start is called before the first frame update
    void Start()
    {
        harvesetPalceholder = GameObject.FindGameObjectWithTag("Harvests");
        foreach (Transform child in this.transform)
        {
            
            foreach (Transform harvest in harvesetPalceholder.transform)
            {
                if (harvest.name.Equals(child.GetChild(0).GetChild(0).name))
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
