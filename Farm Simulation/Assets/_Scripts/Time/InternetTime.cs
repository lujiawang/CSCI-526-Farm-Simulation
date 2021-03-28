using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
 
public class InternetTime : MonoBehaviour
{
	#region Singleton
    // Use "InternetTime.instance" to access the Inventory instance and perform Add/Remove function
    public static InternetTime instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("> 1 InternetTime instance found");
            return;
        }
        instance = this;
    }
    #endregion

    private string url = "http://worldtimeapi.org/api/ip";
    private DateTime currTime;

    public int fetchInterval;
    private int minimumInterval = 3;

    public delegate void OnTimeChanged();
    public OnTimeChanged onTimeChangedCallback;

    [SerializeField]
    public bool useTimeForCrops = false;

	void Start()
	{
		StartCoroutine(UpdateTime());
	}

	void OnDestroy()
	{
		SaveInternetTime();
	}

	void SaveInternetTime()
	{
		// yield return StartCoroutine(FetchTime());
		// save exit internet time
    	PlayerPrefs.SetString("exitInternetTime", currTime.ToString());
    	// Debug.Log(currTime);
	}

	public DateTime GetTime()
	{
		return currTime;
	}

	// updates every 10 seconds
	private IEnumerator UpdateTime()
	{
		do
		{
			yield return StartCoroutine(FetchTime());
			yield return new WaitForSeconds(fetchInterval>minimumInterval?fetchInterval:minimumInterval);
		}while(true);
		
	}

	public IEnumerator FetchTime()
	{
		UnityWebRequest uwr = UnityWebRequest.Get(url);
	    yield return uwr.SendWebRequest();

	    if (uwr.isNetworkError)
	    {
	        Debug.Log("Error While Sending: " + uwr.error);
	    }
	    else
	    {
	    	string jsonData = uwr.downloadHandler.text;
	    	// Debug.Log(jsonData);
	    	// delete wrong bytes
	    	int wrongCharCounter = 0;
	    	while(true)
	    	{
	    		if(jsonData[wrongCharCounter] != '{')
	    			wrongCharCounter++;
	    		else
	    			break;
	    	}
	    	jsonData = jsonData.Substring(wrongCharCounter);
	    	// Debug.Log(jsonData);
	    	if(jsonData.Contains("datetime"))
	    	{
	    		ReturnTextObj obj = JsonUtility.FromJson<ReturnTextObj>(jsonData);
		        currTime = DateTime.Parse(obj.datetime);
		        // Debug.Log(currTime);
				if (onTimeChangedCallback != null)
		        {
		            onTimeChangedCallback.Invoke();
		        }
	    	}
	        
	    }
	}

	// public void CallBack(DateTime obj)
	// {
	// 	currTime = obj;
	// }

	// public IEnumerator GetRequest(string uri, Action<DateTime> callback = null)
	// {
	//     UnityWebRequest uwr = UnityWebRequest.Get(uri);
	//     yield return uwr.SendWebRequest();

	//     if (uwr.isNetworkError)
	//     {
	//         Debug.Log("Error While Sending: " + uwr.error);
	//     }
	//     else
	//     {
	//         ReturnTextObj obj = JsonUtility.FromJson<ReturnTextObj>(uwr.downloadHandler.text);
	//         // currTime = DateTime.Parse(obj.currentDateTime);
	//         if(callback != null)
	//         	callback(DateTime.Parse(obj.currentDateTime));
	//     }
	// }
	 
	
}