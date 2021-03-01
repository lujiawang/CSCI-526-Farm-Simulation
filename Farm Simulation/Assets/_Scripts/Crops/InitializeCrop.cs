using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class InitializeCrop : MonoBehaviour
{
    // Start is called before the first frame update


    public AssetReference cropAsset;
    void Start()
    {
        cropAsset.InstantiateAsync();
    }
}
