using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersistObjManager : MonoBehaviour
{
	private static GameObject gameManagerInstance;
	private static GameObject canvasInstance;
	void Awake()
    {
    	string name = this.name;
        DontDestroyOnLoad(this.gameObject);
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
