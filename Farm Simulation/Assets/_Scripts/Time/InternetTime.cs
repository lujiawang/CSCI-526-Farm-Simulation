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

    public delegate void OnTimeChanged();
    public OnTimeChanged onTimeChangedCallback;

	void Start()
	{
		StartCoroutine(UpdateTime());
	}

	public DateTime GetTime()
	{
		return currTime;
	}

	// updates every 10 seconds
	public IEnumerator UpdateTime()
	{
		do
		{
			yield return StartCoroutine(FetchTime());
			// Debug.Log(currTime);
			if (onTimeChangedCallback != null)
	        {
	            onTimeChangedCallback.Invoke();
	        }
			yield return new WaitForSeconds(10);
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
	        ReturnTextObj obj = JsonUtility.FromJson<ReturnTextObj>(uwr.downloadHandler.text);
	        currTime = DateTime.Parse(obj.datetime);
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