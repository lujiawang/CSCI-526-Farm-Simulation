using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistObjManager : MonoBehaviour
{
	private static GameObject gameManagerInstance;
	private static GameObject canvasInstance;
	void Awake()
    {
    	string name = this.name;
        DontDestroyOnLoad(this.gameObject);
        // string sceneName = SceneManager.GetActiveScene().name;
        switch(name)
        {
        	case "GameManager":
        		if (gameManagerInstance == null)
		    		gameManagerInstance = this.gameObject;
		    	else
		    		Destroy(this.gameObject);
		    	break;
	    	case "Canvas":
	    		if (canvasInstance == null)
		    		canvasInstance = this.gameObject;
		    	else
		    		Destroy(this.gameObject);
		    	break;
		    default:
		    	Debug.LogWarning("ScenePersistObjManager error");
		    	break;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if(this == null)
            return;
        string name = this.name;
        // Debug.Log("sceneName: " + sceneName + "; Obj: "+ name);
        if(name == "GameManager")
        {
            InternetTime internetTime = InternetTime.instance;
            if (sceneName == "Main_Menu" || sceneName == "Intro")
            {
                // internetTime.StopOrStartCor(true); //stop updating time
                Inventory.instance.onItemChangedCallback = null;
                StoreInventory.instance.onStoreItemChangedCallback = null;
                // this.GetComponent<Inventory>().enabled = false;
                // this.GetComponent<StoreInventory>().enabled = false;
            }
            else
            {
                // internetTime.StopOrStartCor(false); //start updating time
                // this.GetComponent<Inventory>().enabled = true;
                // this.GetComponent<StoreInventory>().enabled = true;
            }
        }
        else if(name == "Canvas")
        {
            if (sceneName == "Main_Menu" || sceneName == "Intro")
            {
                Destroy(this.gameObject);
                // this.gameObject.SetActive(false);
            }else{
                // this.gameObject.SetActive(true);
            }
        }
        
    }
 
    // void Start()
    // {
    // 	cropLands = GameObject.Find("CropLands");
    // 	SceneManager.sceneLoaded += OnSceneLoaded;
    // }
    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    // 	Debug.Log(scene);
    // 	if(SceneManager.GetActiveScene().name != "Farming_01_main")
    //     {
    //         cropLands.SetActive(false);
    //     }else
    //     {
    //     	cropLands.SetActive(true);
    //     }
    // }
}
