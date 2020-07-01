using System.Collections;
using UnityEngine;

public class FrameCheck : MonoBehaviour
{
    float deltaTime = 0.0f;

    float msec;
    float fps;
    float worstFps = 100f;
    string text;

    void Awake()
    {
        int w = Screen.width, h = Screen.height;

        StartCoroutine("worstReset");
    }

    IEnumerator worstReset() 
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            worstFps = 100f;
        }
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;

        if (fps < worstFps)
            worstFps = fps;
        text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ") //worst : " + worstFps.ToString("F1");
    }
}