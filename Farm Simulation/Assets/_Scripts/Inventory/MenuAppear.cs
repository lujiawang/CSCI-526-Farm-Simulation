using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAppear : MonoBehaviour
{
    private float minY = -335f;
    private float maxY = 375f;
    private bool isMenu;

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
            //transform.position = new Vector2(transform.position.x, maxY);
        }
        else
        {
            anim.SetBool("isMenu", false);
            //transform.position = new Vector2(transform.position.x, minY);
        }
    }
}
