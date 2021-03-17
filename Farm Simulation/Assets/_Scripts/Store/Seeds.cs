using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Seeds : MonoBehaviour
{
    Inventory inventory;
    
    private Button button;

    private string cropName;
    private int index;

    StoreInfo store;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;

        button = this.GetComponent<Button>();
        cropName = this.transform.GetChild(0).name;

        store = StoreInfo.instance;

        if (store.isPurchased(cropName))
            this.gameObject.SetActive(false);
    }



    public void AddSeeds()
    {
        string seedName =  cropName + "Seed";

        inventory.Add(seedName, 1);
        
        this.gameObject.SetActive(false);
        store.Purchase(cropName);
    }

}
