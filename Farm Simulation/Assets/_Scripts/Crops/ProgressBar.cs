using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//[ExecuteInEditMode()]

public class ProgressBar : MonoBehaviour
{
    public float maximum;
    public float current;

    public Image mask;
    public Image fill;

    private GameObject[] CropLands;
    private GameObject relatedLand;

    // Start is called before the first frame update
    void Start()
    {
        CropLands = GameObject.FindGameObjectsWithTag("cropLand");

        int index = this.name[7] - '0';
        relatedLand = CropLands[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (relatedLand.transform.childCount > 0)
        {
            GameObject cropObj = relatedLand.transform.GetChild(0).gameObject;
            CropGrowing crop = cropObj.GetComponent<CropGrowing>();
            if (crop != null)
            {
                this.GetComponent<Image>().enabled = true;
                this.transform.GetChild(0).gameObject.SetActive(true);
                maximum = crop.growingCondition.Length;
                current = crop.stage;
                GetCurrentFill();
            }
        }
        else
        {
            this.GetComponent<Image>().enabled = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void GetCurrentFill()
    {
        float fillAmount = current / maximum;
        mask.fillAmount = fillAmount;
    }
}
