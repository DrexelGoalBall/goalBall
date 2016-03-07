using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{

    public AudioSource AS;
    public AudioClip bounce;
    public AudioClip block;
    public AudioClip roll;
    public AudioClip hold;
    public AudioClip thro;
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
        //ct = gameObject.GetComponent<CatchThrowV2s>();
        AS = gameObject.GetComponent<AudioSource>();
        AS.clip = roll;
    }


    void Update()
    {
        if (!AS.isPlaying)
        {
            AS.clip = roll;
            if (transform.parent != null)
            {
                AS.clip = hold;
            }

            if (loc.isMoving)
            {
                AS.Play();
            }
        }
    }

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

   public void Shoot()
    {
        AS.clip = thro;
        AS.Play();
    }
}
