using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class FPS : MonoBehaviour 
{
    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.

    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            fps = frameCount / dt;
            GetComponent<Text>().text = Convert.ToString(fps);
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }
}
