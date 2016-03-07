using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{

    public AudioSource AS;
    public AudioClip bounce;
    public AudioClip block;
    public AudioClip roll;
    //public float timer = 0;
    //public float increment = 0.05f;
    //public float maxTime = 1;
    private ListObjectLocation loc;

    /// <summary>
    /// Sets up all objects and variables that are needed to run the script.
    /// </summary>
    void Start()
    {
        loc = gameObject.GetComponent<ListObjectLocation>();
        AS = gameObject.GetComponent<AudioSource>();
        AS.clip = roll;
    }

    /// <summary>
    /// Keeps track of the timer that regulates the frequency of the sound.
    /// </summary>
    /*void FixedUpdate()
    {
        if (timer <= maxTime)
        {
            timer += increment;
        }
    }*/

    void Update()
    {
        if (!AS.isPlaying)
        {
            AS.clip = roll;
            if (loc.isMoving)
            {
                AS.Play();
            }
        }
    }


    /// <summary>
    /// Plays a sound on collision, assuming it has not already being played.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        /*if (timer >= maxTime)
        {
            AS.Play();
            timer = 0;
        }*/

        if (collision.gameObject.name == "Floor")
        {
            AS.clip = bounce;
            AS.Play();
        }

        if (collision.gameObject.tag == "Player")
        {
            AS.clip = block;
            AS.Play();
        }


    }
}
