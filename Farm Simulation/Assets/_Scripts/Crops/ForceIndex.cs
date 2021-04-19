using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceIndex : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	int index = this.name[this.name.Length - 2] - '0';
        this.transform.SetSiblingIndex(index);
    }

}
