using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodEffect : MonoBehaviour
{
	Coroutine showMessageCOR;
	Coroutine changeLimitCOR;

	Transform consumePanel;

	ZoomObj zoomScript;
	FadeObj fadeScript;
    // Start is called before the first frame update
    void Start()
    {
        consumePanel = this.transform.Find("FoodConsumeMessagePanel");

        zoomScript = this.GetComponent<ZoomObj>();
        fadeScript = this.GetComponent<FadeObj>();
    }

    // fast crops for 1 hour
    public void FastForwardGrow(string name)
    {
    	showMessageCOR = StartCoroutine(ShowMessage(name, showMessageCOR));
    	int fastForwardSeconds = 1 * 60 * 60;

    	// fast forward
    	GameObject CropLands = GameObject.Find("CropLands");
    	foreach (Transform child in CropLands.transform)
    	{
    		if(child.childCount != 1) // skip empty lands and progress canvas
    			continue;
    		CropGrowing growScript = child.GetChild(0).GetComponent<CropGrowing>();
    		if(growScript.grown) // skip grown crops
    			continue;
    		int runningPointer = growScript.GetTime();
    		int stage = growScript.GetStage();

    		float totalSecondsForGrowth = growScript.GetTotalSecondsForGrowth();
    		int totalStages = growScript.GetTotalStages();

    		runningPointer += fastForwardSeconds;
	        stage += (int)(fastForwardSeconds / growScript.growTime);
	        runningPointer = runningPointer>totalSecondsForGrowth?(int)totalSecondsForGrowth:runningPointer;
	        stage = stage>totalStages?totalStages:stage;

	        GameObject copyCrop = Instantiate(growScript.gameObject, child);
	        copyCrop.name = growScript.gameObject.name;
	        Destroy(growScript.gameObject);
	        growScript = copyCrop.GetComponent<CropGrowing>();

	        growScript.SetState(runningPointer, stage);
    	}
    }

    public void HigherHarvest(string name)
    {
    	showMessageCOR = StartCoroutine(ShowMessage(name, showMessageCOR));
    	if(changeLimitCOR != null)
    		StopCoroutine(changeLimitCOR);
    	changeLimitCOR = StartCoroutine(Item.ChangeRandomHarvestLimitCOR(60)); //effect lasts for 1 minute
    }

    public IEnumerator ShowMessage(string name, Coroutine waitTillAfter)
    {
    	if(waitTillAfter != null)
            yield return waitTillAfter;
        yield return ShowMessage(name);
    }

    public IEnumerator ShowMessage(string name)
    {
    	string message2 = "";
    	switch(name)
    	{
    		case "FruitSalads":
    			message2 = "All crops grow 1 hour faster!";
    			break;
    		case "CornSuccotash":
    			message2 = "You can harvest more crops for 1 min!";
    			break;
    		default:
    			break;
    	}
        Sprite sprite = Item.GetItemSprite(name);
        Transform itemButton = consumePanel.Find("InventorySlot").Find("ItemButton");
        itemButton.Find("Text").GetComponent<Text>().text = name;
        itemButton.Find("Item").GetComponent<Image>().sprite = sprite;
        consumePanel.Find("Message1").GetComponent<Text>().text = "You ate a bowl of " + name;
        consumePanel.Find("Message2").GetComponent<Text>().text = message2;

        // Animations
        consumePanel.GetComponent<CanvasGroup>().alpha = 1f;
        yield return StartCoroutine(zoomScript.Zoom(consumePanel, true, 0.4f));
        float counter = 0f;
        float duration = 2f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(fadeScript.Fade(consumePanel, false, 0.4f));
    }
}
