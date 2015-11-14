using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{

    //KeepTrackOfTime
    private float time;
    public int lengthOfTimer = 120;
    private bool paused = true;

    public Timer(int pLengthOfTimer)
    {
        lengthOfTimer = pLengthOfTimer;
        time = lengthOfTimer;
    } 

    // Update is called once per frame
    void Update()
    {
        if (paused) return;
        if (time < 0) return;

        time -= Time.deltaTime;
        Debug.Log (time);
    }

    //Setters and getters
    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }

    public void SetTime(float t)
    {
        time = t;
    }

    public void SetLengthOfTimer(int length)
    {
        lengthOfTimer = length;
        time = (float)length;
    }

    public void Reset()
    {
        time = lengthOfTimer;
    }

    public float getTime()
    {
        return time;
    }

    public string getTimeString()
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }
}
