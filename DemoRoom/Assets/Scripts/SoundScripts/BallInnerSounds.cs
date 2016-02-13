using UnityEngine;
using System.Collections;

/// <summary>
/// Plays the sounds that the ball makes on the inside of the ball.  Mainly simulates the bells in the balls.
/// </summary>
public class BallInnerSounds : MonoBehaviour {

    //Informant
    public GameObject ball;
    public float threshold = 5;

    //Audio Source
    private AudioSource AS;

    //Sounds
    public AudioClip bells;
    public AudioClip tone;

    /// <summary>
    /// Initializes the objects and varibles that are used by the script.
    /// </summary>
    void Start ()
    {
        AS = gameObject.GetComponent<AudioSource>();
        AS.clip = tone;
	}

    /// <summary>
    /// Plays the sounds inside the ball based on the velocity that the ball is moving.
    /// </summary>
    void FixedUpdate ()
    {
	    if (ball.GetComponent<Rigidbody>().velocity.magnitude > threshold)
        {
            if (AS.clip == tone)
            {
                AS.clip = bells;
                AS.Play();
            }
        }
        else
        {
            if (AS.clip == bells)
            {
                AS.clip = tone;
                AS.Play();
            }
        }
	}
}
