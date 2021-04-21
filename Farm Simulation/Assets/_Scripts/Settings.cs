// this script is attached to SettingsPanel->Settings

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    // SoundManager soundManager;

	void Start()
	{
        SceneManager.sceneLoaded += OnSceneLoaded;
		if(PlayerPrefs.HasKey("MusicVolume"))
		{
			float vol = PlayerPrefs.GetFloat("MusicVolume");
			this.transform.Find("MusicGroup").Find("VolumeSlider").GetComponent<Slider>().value = vol;
			SetMusicVol(vol);
		}

		if(PlayerPrefs.HasKey("SFXVolume"))
		{
			float vol = PlayerPrefs.GetFloat("SFXVolume");
			this.transform.Find("SFXGroup").Find("VolumeSlider").GetComponent<Slider>().value = vol;
			SetSFXVol(vol);
		}
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(this == null)
            return;
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            float vol = PlayerPrefs.GetFloat("MusicVolume");
            this.transform.Find("MusicGroup").Find("VolumeSlider").GetComponent<Slider>().value = vol;
            SetMusicVol(vol);
        }

        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            float vol = PlayerPrefs.GetFloat("SFXVolume");
            this.transform.Find("SFXGroup").Find("VolumeSlider").GetComponent<Slider>().value = vol;
            SetSFXVol(vol);
        }
    }

	public void SetMusicVol(float vol)
    {
    	// float p = Mathf.Log10(vol) * 1;
    	// vol = Mathf.Pow(10, p);
    	// Debug.Log("Gets Called: " + vol);
    	PlayerPrefs.SetFloat("MusicVolume", vol);

    	for(float i = 0f; i < 1f; i += Time.deltaTime)
		{
			if(GameObject.Find("background_audio") != null)
			{
                Debug.Log("Found");
	    		GameObject.Find("background_audio").GetComponent<AudioSource>().volume = vol;
	    		break;
			}
		}
    }

    public void SetSFXVol(float vol)
    {
    	// float p = Mathf.Log10(vol) * 5;
    	// vol = Mathf.Pow(10, p);
    	// Debug.Log("Gets Called: " + vol);
    	PlayerPrefs.SetFloat("SFXVolume", vol);
    	// this.GetComponent<SoundManager>().SFXVolume = vol;
    	SoundManager.SFXVolume = vol;
    }

    public void BackToStart()
    {
    	if(SceneManager.GetActiveScene().name != "Main_Menu")
	    	SceneManager.LoadSceneAsync("Main_Menu", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
    	Application.Quit();
    }

    public void CloseSettings()
    {
    	this.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
    	this.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;

    	if(SceneManager.GetActiveScene().name == "Workshop")
    	{
    		GameObject.Find("WorkshopCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
    		GameObject.Find("WorkshopCanvas").GetComponent<CanvasGroup>().alpha = 1f;
    	}
    }

    public void OpenSettings()
    {
    	this.transform.parent.GetComponent<CanvasGroup>().alpha = 1f;
    	this.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;

    	if(SceneManager.GetActiveScene().name == "Workshop")
    	{
    		GameObject.Find("WorkshopCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
    		GameObject.Find("WorkshopCanvas").GetComponent<CanvasGroup>().alpha = 0f;
    	}
    }
}
