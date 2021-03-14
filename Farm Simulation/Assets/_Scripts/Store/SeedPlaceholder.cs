using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlaceholder : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(this);
        //DontDestroyOnLoad(player); 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
