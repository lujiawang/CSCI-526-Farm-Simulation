using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAppear : MonoBehaviour
{
    private float minY = -335f;
    private float maxY = 375f;
    public static bool isMenu;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        isMenu = false;
        transform.position = new Vector2(transform.position.x, minY);
        anim = GetComponent<Animator>();
    }

    public void MenuHideAndShow()
    {
        isMenu = !isMenu;
        if (isMenu)
        {
            anim.SetBool("isMenu", true);
            Camera.main.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
            DragAndDrop.canDrag = true;
            CameraFollow.enableCamera = false;
        }
        else
        {
            anim.SetBool("isMenu", false);

            Camera.main.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
            DragAndDrop.canDrag = false;
            CameraFollow.enableCamera = true;
        }
    }
}
