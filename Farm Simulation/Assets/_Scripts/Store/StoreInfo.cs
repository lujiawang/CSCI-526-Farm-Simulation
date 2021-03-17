using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInfo : MonoBehaviour
{
    public Transform ItemsParent;

    public static bool[] purchased;
    private static string[] cropNames;

    private int count;

    public static StoreInfo instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);

            count = ItemsParent.childCount;
            purchased = new bool[count];
            cropNames = new string[count];
            for (int i = 0; i < count; i++)
            {
                cropNames[i] = ItemsParent.GetChild(i).GetChild(0).GetChild(0).name;
            }

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool isPurchased(string cropName)
    {
        for (int i = 0; i < count; i++)
        {
            if (cropName.Equals(cropNames[i]))
            {
                return purchased[i];
            }
        }

        return false;

    }


    public void Purchase(string cropName)
    {
        for (int i = 0; i < count; i++)
        {
            if (cropName.Equals(cropNames[i]))
            {
                purchased[i] = true;
                break;
            }
        }

    }

}
