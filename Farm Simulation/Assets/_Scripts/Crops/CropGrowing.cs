using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using System.Collections;

public class CropGrowing : MonoBehaviour
{
    //summary: script starts the growing process of a crop


    //variables

    private SpriteRenderer sr;
    private Sprite[] crops;
    private int runningPointer = 0;
    public int stage = 0;
    public float[] growingCondition;
    private bool inPosition = false;
    public float growTime;
    public string cropName;
    //public GameObject Notification;
    //private Text notificationText;
    //private Dictionary<int, Sprite> cropAssets;
    public bool grown = false; //make it public so player will know whether it is grown
    public int reward;
    public AssetReferenceSprite[] addrs = new AssetReferenceSprite[6];
    private int cropNum = 0;
    private bool started = false;
    private GameObject canvas;


    public AudioClip HarvestSound;

    private bool harvested = false;

    private bool doReassignSprite = false; // For RestoreCropGrowing.cs
    // Start is called before the first frame update
    void Start()
    {
        SetUpAssets();
        //Loading of assets using AssetReferenceSprite
        sr = GetComponent<SpriteRenderer>();
        canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];
        foreach (AssetReferenceSprite newSprite in addrs)
        {
            newSprite.LoadAssetAsync().Completed += SpriteLoaded;
        }

        //notificationText = Notification.transform.GetChild(0).GetComponent<Text>();

        // stage = 0;
        // inPosition = false;
        // grown = false;
        
    }

    void SetUpAssets()
    {
        //cropAssets = new Dictionary<int, Sprite>();

        crops = new Sprite[6];
        // runningPointer = 0;
        growingCondition = new float[6];

        //initialize to random variable if not specified in the inspector;
        // if (growTime <= 0) growTime = 3;
        growTime = Item.GetGrowTime(this.name);
        for (int i = 1; i < growingCondition.Length; i++)
        {
            growingCondition[i] = growingCondition[i - 1] + growTime;
        }
        // started = false;

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


    void AssignSprite(int index)
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
            ShowToast cScript = canvas.GetComponent<ShowToast>();
            cScript.showToast( name+ " is ready for harvesting.",2);
        }
        else if (runningPointer >= growingCondition[stage])
        {
            // Debug.Log("assgining");
            AssignSprite(stage);
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
    public float GetTotalSecondsForGrowth()
    {
        SetUpAssets();
        return growingCondition[growingCondition.Length-1];
    }
    public int GetTotalStages()
    {
        SetUpAssets();
        return growingCondition.Length;
    }
    public bool Harvested()
    {
        return harvested;
    }
    public void SetHarvested()
    {
        harvested = true;
    }

    // this should manually set the crop stage and renders image correctly
    public void SetState(int runningPointer, int stage)
    {
        // this.grown = grown;
        this.runningPointer = runningPointer;
        this.stage = stage;
        this.started = true;

        this.doReassignSprite = true;
        this.GetComponent<SpriteRenderer>().sprite = null;

        // Debug.Log("runningPointer: "+runningPointer);
        // Debug.Log("stage: "+stage);
        InvokeRepeating("incrementByOne", 1f, 1f);
    }



    // Update is called once per frame
    void Update()
    {
        // Debug.Log("runningPointer: "+runningPointer);
        // Debug.Log("stage: "+stage);

        // Assigne Sprite if sprite is incorrect
        if(doReassignSprite && cropNum == 6)
        {
            AssignSprite(stage>5?5:stage);
            doReassignSprite = false;
        }

        if(cropNum == 6 && started == false)
        {
            // Repeat method per 1 second
            AssignSprite(0);
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

    public void HarvestAudio()
    {
        AudioSource audioSource = this.GetComponent<AudioSource>();
        audioSource.PlayOneShot(HarvestSound);
    }
    public void HarvestAnim()
    {
        Destroy(this.gameObject);
    }
}
