using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
///     Determines the current frames per second the game is running at
/// </summary>
public class FPS : MonoBehaviour 
{
    // Number of frames since last update
    int frameCount = 0;
    // Number of milliseconds since last update
    float dt = 0.0f;
    // Most recent frames per second calculation
    float fps = 0.0f;
    // Number of times to update per second
    float updateRate = 4.0f;  // 4 updates

    /// <summary>
    ///     Counts the total number of frames since the last update to calculate the fps
    /// </summary>
    void Update()
    {
        // Increase frame and change in time counters
        frameCount++;
        dt += Time.deltaTime;
        // Check if the required amount of time has passed
        if (dt > 1.0 / updateRate)
        {
            // Update fps calculation
            fps = frameCount / dt;
            GetComponent<Text>().text = Convert.ToString(fps);
            // Reset values for next update
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }
}
