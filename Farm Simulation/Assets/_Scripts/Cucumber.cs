using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Cucumber : MonoBehaviour
{
    private SpriteRenderer sr;
    private Sprite[] sprites;
    private Sprite[] crops;
    private int runningPointer;
    private int stage;
    private int[] growingCondition;



    // Start is called before the first frame update
    void Start()
    {
        //Loading of assets using Addressable Sprite Assets
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>("Assets/_Sprites/Crops/Crop_Spritesheet.png");
        spriteHandle.Completed += LoadOnReady;

        sr = GetComponent<SpriteRenderer>();
    }
    void LoadOnReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            sprites = handleToCheck.Result;


            //initialize the rest only when all sprites are loaded
            crops = new Sprite[6];

            // to exclusively get the corn sprites
            for (int i = 0; i < crops.Length; i++)
            {
                crops[i] = sprites[i + 24];
            }
            Debug.Log("Checking " + crops[0].name);


            runningPointer = -1;
            stage = 0;
            growingCondition = new int[6];
            for (int i = 1; i < growingCondition.Length; i++)
            {
                growingCondition[i] = growingCondition[i - 1] + 3;
            }
            // Repeat method per 1 second
            InvokeRepeating("incrementByOne", 1f, 1f);



        }
    }




    void assignSprite(int index)
    {
        // changing of sprites 
        Sprite sprite = crops[index];
        sr.sprite = sprite;
    }
    void updateSprite()
    {
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
