using UnityEngine;
using System.Collections;

public class CameraBox : MonoBehaviour {
    /// <summary>
    /// This script allows players to switch between cameras in the cams variable.
    /// NOTE: this is mainly a debugging script used to see the whole field of play.
    /// </summary>

    //Cameras
    public Camera[] cams;

    //Indexs
    int currentIndex = 0;

    /// <summary>
    /// Initiazes all of the variables that need to be initialized and makes sure only one camera is on.
    /// </summary>
    void Start ()
    {
	   foreach (Camera c in cams)
       {
            c.enabled = false;
       }
        cams[currentIndex].enabled = true;
	}

    /// <summary>
    /// Detects when player presses the K key and switches the camera.
    /// </summary>
    void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.K))
        {
            if (cams.Length > 1)
            {
                cams[currentIndex].enabled = false;
                if (currentIndex == cams.Length - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex = currentIndex + 1;
                }
                cams[currentIndex].enabled = true;
            }
        }
	}
}
