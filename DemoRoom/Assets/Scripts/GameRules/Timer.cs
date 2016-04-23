using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// This is the low level timer that all other scripts use to keep track of time.
/// </summary>
public class Timer : NetworkBehaviour
{
    //KeepTrackOfTime
    [SyncVar]
    private float time;
    public int lengthOfTimer = 120;
    [SyncVar]
    private bool paused = true;

    /// <summary>
    /// Initialize a time that will run a given length.
    /// </summary>
    /// <param name="pLengthOfTimer"></param>
    public Timer(int pLengthOfTimer)
    {
        lengthOfTimer = pLengthOfTimer;
        time = lengthOfTimer;
    }

    /// <summary>
    /// Uses Time.deltatime to update the timer.
    /// </summary>
    void Update()
    {
        if (paused) return;
        if (time < 0) return;

        if (isServer)
            time -= Time.deltaTime;
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void Pause()
    {
        if (isServer)
            paused = true;
    }

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    public void Resume()
    {
        if (isServer)
            paused = false;
    }

    /// <summary>
    /// Sets the current value of the timer to the value t.
    /// </summary>
    /// <param name="t"></param>
    public void SetTime(float t)
    {
        if (isServer)
            time = t;
    }

    /// <summary>
    /// Sets the length of the timer to the value of length.
    /// </summary>
    /// <param name="length"></param>
    public void SetLengthOfTimer(int length)
    {
        lengthOfTimer = length;
        if (isServer)
            time = (float)length;
    }

    //Resets the timer to its current length.
    public void Reset()
    {
        if (isServer)
            time = lengthOfTimer;
    }

    /// <summary>
    /// Gets the current time of the timer as a float.
    /// </summary>
    /// <returns></returns>
    public float getTime()
    {
        return time;
    }

    /// <summary>
    /// Gets the current length of the timer in a formated time string.
    /// </summary>
    /// <returns></returns>
    public string getTimeString()
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Gets whether the timer is paused
    /// </summary>
    /// <returns></returns>
    public bool isPaused()
    {
        return paused;
    }
}
