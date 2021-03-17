using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	private AudioSource audioSource;
	public AudioClip addSound, removeSound;

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

	void Start()
	{
		audioSource = this.GetComponent<AudioSource>();
	}

    public void PlaySound(string sound)
    {
		Debug.Log("here");
		//audioSource.PlayOneShot(addSound);
		/*if (sound.Equals("add"))
        {
			audioSource.PlayOneShot(addSound);
		}*/ 
		if (sound.Equals("remove"))
        {
			audioSource.PlayOneShot(removeSound);
        }
		//AudioClip audioClip = Resources.Load<AudioClip>("sounds/"+sound);
		// audioSource.clip = audioClip;
		//audioSource.PlayOneShot(audioClip);
	}
}
