using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class Corn : MonoBehaviour
{
    // Start is called before the first frame update

    private SpriteRenderer sr;
    private Sprite[] sprites;
    private Sprite[] crops;
    private int runningPointer;
    private int stage;
    private int[] growingCondition;
    private bool inPosition;
    public int growTime;
    void Start()
    {
        //Loading of assets using Addressable Sprite Assets
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>("Assets/_Sprites/Crops/Crop_Spritesheet.png");
        spriteHandle.Completed += LoadOnReady;

        sr = GetComponent<SpriteRenderer>();
        stage = 0;
        inPosition = false;

    }

    void LoadOnReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        //callback function
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            sprites = handleToCheck.Result;


            //initialize the rest only when all sprites are loaded
            crops = new Sprite[6];

            // to exclusively get the corn sprites
            for (int i = 0; i < crops.Length; i++)
            {
                crops[i] = sprites[i + 18];
            }
            // Debug.log("checking " + corns[0].name);

            runningPointer = 0;
            growingCondition = new int[6];

            //initialize to random variable if not specified in the inspector;
            if (growTime == 0) growTime = 3;
            for (int i = 1; i < growingCondition.Length; i++)
            {
                growingCondition[i] = growingCondition[i - 1] + growTime;
            }
            //Starting sprite is always the seed
            assignSprite(0);


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
    void growCrop()
    {
        //when callback function not yet done
        if (growingCondition == null) return;


        //crop is fulling grown
        if (stage == growingCondition.Length)
        {
            CancelInvoke("incrementByOne");

            //TODO: Collect Reward;
        }
        else if (runningPointer == growingCondition[stage])
        {

            assignSprite(stage);
            stage++;
        }
    }


    void incrementByOne()
    {
        if (!inPosition) return;
        if (stage < growingCondition.Length)
        {
            runningPointer++;
        }
    }



    // Update is called once per frame
    void Update()
    {


        //only update when crop is attached to the parent
        if (this.transform.parent != null && this.transform.parent.name != "CropPlaceholder")
        {
            //start growing condition
            inPosition = true;

            growCrop();
        }
    }
}
