using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/// <summary>
/// This class controls what happens when we need to make an XBOX controller Rumble
/// </summary>
public class RumbleController : MonoBehaviour {

    private float endTime = 0f;
    private float time = 0f;
    private float lStrength = 0f;
    private float rStrength = 0f;
	
	// Update is called once per frame
    /// <summary>
    /// Controls the vibration of the player one controller.  When the above variables update it will rumble accordingly.
    /// </summary>
	void Update ()
    {
	    if (endTime >= time)
        {
            GamePad.SetVibration(PlayerIndex.One, lStrength, rStrength);
            time += Time.deltaTime;
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }
    }

    /// <summary>
    /// Rumble the controller with the following parameters
    /// </summary>
    /// <param name="t">How long with the rumble last</param>
    /// <param name="lS">The left strength of the rumble</param>
    /// <param name="rS">the right strength of the rumble.</param>
    public void BasicRumble(float t, float lS, float rS)
    {
        time = 0f;
        endTime = t;
        lStrength = lS;
        rStrength = rS;
    }

    /// <summary>
    /// This function allows for bursts of Vibration
    /// </summary>
    /// <param name="bursts"></param>
    /// <param name="lS"></param>
    /// <param name="rS"></param>
    public void RumbleBursts(int bursts, float lS, float rS)
    {
        float delays = 0;
        for (int i=0; i < bursts; i++)
        {
            StartCoroutine(RumbleAfterDelay(delays,.2f,lS,rS));
            delays += .1f;
            StartCoroutine(RumbleAfterDelay(delays, .2f, 0, 0));
            delays += .1f;
        }
    }

    /// <summary>
    /// Coroutine made to have short rumbles over time. 
    /// </summary>
    /// <param name="delay">How long to wait before running code.</param>
    /// <param name="t">Time to pass into basic rumble</param>
    /// <param name="lS">left strength to pass into basic rumble</param>
    /// <param name="rS">right strength to pass into basic rumble</param>
    IEnumerator RumbleAfterDelay(float delay, float t, float lS, float rS)
    {
        yield return new WaitForSeconds(delay);
        BasicRumble(t, lS, rS);
    }


}
