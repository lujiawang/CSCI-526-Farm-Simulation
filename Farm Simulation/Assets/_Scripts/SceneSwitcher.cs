using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Intro to main
    public void IntroToMain()
    {
        SceneManager.LoadScene("Farming_01_main");
    }
    

    //To store's intro
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collide");
            SceneManager.LoadScene("Store", LoadSceneMode.Additive);
            TouchToMove.isPlayer = false;
        }
    }


    //To store
    public void SwitchToStore()
    {
        SceneManager.UnloadSceneAsync("Store");
        SceneManager.LoadScene("Farming_02_store", LoadSceneMode.Additive);
    }


    //Back to main
    public void BackToMain()
    {
        SceneManager.UnloadSceneAsync("Farming_02_store");
        TouchToMove.isPlayer = true;
    }


}
