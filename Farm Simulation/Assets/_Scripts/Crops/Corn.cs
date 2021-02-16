﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class Corn : MonoBehaviour
{
    // Start is called before the first frame update

    private SpriteRenderer sr;
    private Sprite[] sprites;
    private Sprite[] corns;
    private int runningPointer;
    private int stage;
    private int[] growingCondition;
    // private int nextStage;

    void Start()
    {
        //Loading of assets using Addressable Sprite Assets
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>("Assets/_Sprites/Crops/Crop_Spritesheet.png");
        spriteHandle.Completed += LoadOnReady;
        
        sr = GetComponent<SpriteRenderer>();
        stage = 0;

    }

    void LoadOnReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            sprites = handleToCheck.Result;


            //initialize the rest only when all sprites are loaded
            corns = new Sprite[6];

            // to exclusively get the corn sprites
            for (int i = 0; i < corns.Length; i++)
            {
                corns[i] = sprites[i + 18];
            }
            // Debug.log("checking " + corns[0].name);

            runningPointer = -1;
            growingCondition = new int[6];
            for (int i = 1; i < growingCondition.Length; i++)
            {
                growingCondition[i] = growingCondition[i - 1] + 2;
            }
            // Repeat method per 1 second
            InvokeRepeating("incrementByOne", 1f, 1f);



        }
    }




    void assignSprite(int index)
    {
        // changing of sprites 
        Sprite sprite = corns[index];
        sr.sprite = sprite;
    }
    void updateSprite()
    {
        //callback function not yet done
        if (growingCondition == null) return;
        
        if (stage == growingCondition.Length)
        {
            CancelInvoke("incrementByOne");

        }
        else if (runningPointer == growingCondition[stage])
        {

            assignSprite(stage);
            stage++;
        }
    }


    void incrementByOne()
    {
        if (stage < growingCondition.Length)
        {
            runningPointer++;
        }   
    }



    // Update is called once per frame
    void Update()
    {
       updateSprite();
        
    }
}