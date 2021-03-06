using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private IEnumerator WaitforSceneLoad(float time)
    {
        Time.timeScale = 0.01f;
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }

       // Time.timeScale = 1;

    }
    public void ClickEnable()
    {
        StartCoroutine(WaitforSceneLoad(0.2f));
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
