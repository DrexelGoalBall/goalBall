using UnityEngine;
using System.Collections;

public class BallInnerSounds : MonoBehaviour {

    //Informant
    public GameObject ball;
    public float threshold = 5;

    //Audio Source
    private AudioSource AS;

    //Sounds
    public AudioClip bells;
    public AudioClip tone;

	// Use this for initialization
	void Start ()
    {
        AS = gameObject.GetComponent<AudioSource>();
        AS.clip = tone;
	}
	
	// Update is called once per frame
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
