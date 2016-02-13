using UnityEngine;
using System.Collections;

/// <summary>
/// This script plays a sound when an object collides with another object.
/// </summary>
public class MakeSoundOnTouch : MonoBehaviour {

    public AudioClip ac;
    public float timer = 0;
    public float increment = 0.05f;
    public float maxTime = 1;
    public AudioSource AS;

    /// <summary>
    /// Sets up all objects and variables that are needed to run the script.
    /// </summary>
    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
        AS.clip = ac;
    }

    /// <summary>
    /// Keeps track of the timer that regulates the frequency of the sound.
    /// </summary>
    void FixedUpdate()
    {
        if (timer <= maxTime)
        {
            timer += increment;
        }
    }

    /// <summary>
    /// Plays a sound on collision, assuming it has not already being played.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (timer >= maxTime)
        {
            AS.Play();
            timer = 0;
        }

    }
}
