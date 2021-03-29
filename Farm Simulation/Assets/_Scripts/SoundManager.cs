using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	#region Singleton
	// Use "SoundManager.instance" to access the SoundManager instance
	public static SoundManager instance;
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("> 1 SoundManager instance found");
			return;
		}
		instance = this;
	}
	#endregion

	AudioSource[] audioSources;

	void Start()
	{
		audioSources = this.transform.Find("SoundPlayer").GetComponents<AudioSource>();
	}

	public void PlaySound(string clipName)
	{
		int index;
		switch(clipName)
		{
			case "AddItemSound":
				index = 0;
				break;
			case "RemoveItemSound":
				index = 1;
				break;
			case "RemoveItemToZeroSound":
				index = 2;
				break;
			case "BuyItemSound":
				index = 3;
				break;
			case "SellItemSound":
				index = 4;
				break;
			case "RefreshStoreSound":
				index = 5;
				break;
			case "ClickTabSound":
				index = 6;
				break;
			case "DeleteItemSound":
				index = 7;
				break;
			case "AddStarterPackageSound":
				index = 8;
				break;
			default:
				Debug.LogWarning("invalid clip name!");
				return;
		}
		PlaySound(index);
	}

	public void PlaySound(int index)
	{
		// Debug.Log(audioSources[8]);
		AudioSource audioSource = audioSources[index];
		audioSource.PlayOneShot(audioSource.clip);
		// Debug.Log("index:"+index);
	}

}
