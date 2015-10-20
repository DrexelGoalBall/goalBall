using UnityEngine;
using System.Collections;

public class MakeSoundOnTouch : MonoBehaviour {

    public AudioClip ac;
    public float timer = 0;
    public float increment = 0.05f;
    public float maxTime = 1;
    public AudioSource AS;

    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
        AS.clip = ac;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer <= maxTime)
        {
            timer += increment;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (timer >= maxTime)
        {
            AS.Play();
            timer = 0;
        }

    }
}
