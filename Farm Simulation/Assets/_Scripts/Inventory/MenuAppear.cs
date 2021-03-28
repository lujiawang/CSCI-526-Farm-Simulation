using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAppear : MonoBehaviour
{
    private float minY = -335f;
    // private float maxY = 375f;
    public static bool isMenu;

    private Animator anim;

    CanvasGroup parentPanel;

    // Start is called before the first frame update
    void Start()
    {
        isMenu = false;
        transform.position = new Vector2(transform.position.x, minY);
        anim = GetComponent<Animator>();

        parentPanel = this.transform.Find("ParentPanel").gameObject.GetComponent<CanvasGroup>();
        parentPanel.blocksRaycasts = isMenu;
    }

    public void MenuHideAndShow()
    {
        isMenu = !isMenu;
        if (isMenu)
        {
            anim.SetBool("isMenu", true);
            // Camera.main.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
            // CameraFollow.enableCamera = false;
        }
        else
        {
            anim.SetBool("isMenu", false);
            // Camera.main.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
            // CameraFollow.enableCamera = true;
        }
        parentPanel.blocksRaycasts = isMenu;
        // reset scrollview position
        // ScrollHeight cScript = GetComponent<ScrollHeight>();
        // if(this.transform.GetChild(0).GetChild(0).gameObject.name != "ItemsParent")
        // {
        //     Debug.LogWarning("fatal: wrong object!!");
        //     return;
        // }
        // cScript.UpdateHeight(this.transform.GetChild(0).GetChild(0), false);
    }
}
