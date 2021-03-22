using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class CropGrowing : MonoBehaviour
{
    //summary: script starts the growing process of a crop


    //variables

    private SpriteRenderer sr;
    private Sprite[] crops;
    private int runningPointer;
    private int stage;
    private int[] growingCondition;
    private bool inPosition;
    public int growTime;
    public string cropName;
    public GameObject Notification;
    private Text notificationText;
    //private Dictionary<int, Sprite> cropAssets;
    public bool grown; //make it public so player will know whether it is grown
    public int reward;
    public AssetReferenceSprite[] addrs = new AssetReferenceSprite[6];
    private int cropNum = 0;
    private bool started;
    // Start is called before the first frame update
    void Start()
    {
        setUpAssets();
        //Loading of assets using AssetReferenceSprite
        sr = GetComponent<SpriteRenderer>();
        
        foreach (AssetReferenceSprite newSprite in addrs)
        {
            newSprite.LoadAssetAsync().Completed += SpriteLoaded;
        }

        notificationText = Notification.transform.GetChild(0).GetComponent<Text>();

        
        stage = 0;
        inPosition = false;
        grown = false;
        
    }

    void setUpAssets()
    {
        //cropAssets = new Dictionary<int, Sprite>();

        crops = new Sprite[6];
        runningPointer = 0;
        growingCondition = new int[6];

        //initialize to random variable if not specified in the inspector;
        if (growTime == 0) growTime = 3;
        for (int i = 1; i < growingCondition.Length; i++)
        {
            growingCondition[i] = growingCondition[i - 1] + growTime;
        }
        started = false;

    }




    void SpriteLoaded(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite res = obj.Result;
            string resName = res.name;
            int index = resName[resName.Length - 1] - '0';
            crops[index] = obj.Result;
            cropNum++;
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
            //signify that crop has fully grown
            grown = true;
            notificationText.text = "1 or more crops are ready for harvesting.";
            



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


    public int harvestCrop()
    {
        if (!grown) return -1;


        //TODO: obtain reward and remove from parent
        return reward;

    }
    // Get the current stage of the crop
    public int GetStage()
    {
        return stage;
    }
    //Get time (in seconds) since crop has started growing
    public int GetTime()
    {
        return runningPointer;
    }



    // Update is called once per frame
    void Update()
    {
        
        if(cropNum == 6 && started == false)
        {
            // Repeat method per 1 second
            assignSprite(0);
            InvokeRepeating("incrementByOne", 1f, 1f);
            started = true;
        }

        //only update when crop is attached to the parent
        if (this.transform.parent != null && this.transform.parent.name != "CropPlaceholder" && !grown)
        {
            //start growing condition
            inPosition = true;
            growCrop();
            
        }
    }
}
