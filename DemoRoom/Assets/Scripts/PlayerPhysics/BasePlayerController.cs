using UnityEngine;
using System.Collections;

public class BasePlayerController : MonoBehaviour {
    
    /// <summary>
    /// This script is a basic player controler that is no longer used in the game.
    /// </summary>

    //Movement
    public float playerSpeed = 5;
    public float maxSpeed = 10;

    void Start ()
    {
    }

	// Update is called once per frame
	void Update ()
    {
        //Get Inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(transform.forward * Time.deltaTime* playerSpeed * vertical);
        transform.Translate(transform.right * Time.deltaTime * playerSpeed * horizontal);


    }
}
