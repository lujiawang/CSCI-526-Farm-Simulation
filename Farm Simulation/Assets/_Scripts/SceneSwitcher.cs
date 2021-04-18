using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    GameObject player;

    public static int storeCount;
    private StoreToggle storeToggle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        storeToggle = this.GetComponent<StoreToggle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void LoadSceneGeneral(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Menu to Intro
    public void MenuToIntro()
    {
        SceneManager.LoadSceneAsync("Intro");
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
            ToStoreIntro();
        }
    }

    //To store's intro
    public void ToStoreIntro()
    {
        if (storeCount == 0)
        {
            storeCount++;
            SceneManager.LoadScene("Store", LoadSceneMode.Additive);
            if (storeToggle != null)
            { 
                storeToggle.OpenStore();
            }
        }
        else
        {
            if (storeToggle != null)
            {
                storeToggle.OpenStore();
            }
        }
    }


    //To store
    public void SwitchToStore()
    {
        SceneManager.UnloadSceneAsync("Store");
        
        if (storeToggle!= null)
        {
            storeToggle.OpenStore();
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }


    //Back to main
    public void BackToMain()
    {
        SceneManager.UnloadSceneAsync("Farming_02_store");
    }


}
