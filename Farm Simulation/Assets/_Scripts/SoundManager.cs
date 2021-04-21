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
        if (instance != null)
        {
            // Debug.LogWarning("> 1 SoundManager instance found");
            return;
        }
        instance = this;
    }
    #endregion

    AudioSource[] audioSources;
    public static float SFXVolume = 1f;

    void Start()
    {
        audioSources = this.transform.Find("SoundPlayer").GetComponents<AudioSource>();
    }

    public bool PlaySound(string clipName)
    {
        int index;
        switch (clipName)
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
            case "planting-seeds": // plant seed
                index = 9;
                break;
            case "Bell": // harvest crop
                index = 10;
                break;
            case "FoundRecipeSound":
                index = 11;
                break;
            case "CookSuccessSound":
                index = 12;
                break;
            case "CookFailureSound":
                index = 13;
                break;
            case "CookStartSound":
                index = 14;
                break;
            case "EatSound":
                index = 15;
                break;
            case "item_get":
                index = 16;
                break;
            default:
                Debug.LogWarning("invalid clip name!");
                return false;
        }
        return PlaySound(index);
    }

    // returns false if the did not play sound successfully
    public bool PlaySound(int index)
    {
        // if(audioSources != null && audioSources.Length > index && audioSources[index] != null && audioSources[index].clip != null)
        if (audioSources != null)
        {
            AudioSource audioSource = audioSources[index];
            audioSource.volume = SFXVolume;
            audioSource.PlayOneShot(audioSource.clip);
            return true;
        }
        return false;

        // Debug.Log("index:"+index);
    }

    public bool PlayMultiple(int index, int times)
    {
        bool play = false;
        if (audioSources != null)
        {
            AudioSource audioSource = audioSources[index];
            audioSource.volume = SFXVolume;
            audioSource.PlayOneShot(audioSource.clip);


            play = true;
            for (int i = 1; i < times; i++)
            {
                StartCoroutine(playSoundWithDelay(audioSource, i));
            }
        }
        return play;
    }

    IEnumerator playSoundWithDelay(AudioSource audioSource, int times)
    {
        yield return new WaitForSeconds(audioSource.clip.length * times);
        audioSource.PlayOneShot(audioSource.clip);
        Debug.Log("Played" + times);
    }


}
