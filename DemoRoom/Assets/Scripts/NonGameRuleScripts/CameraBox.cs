using UnityEngine;
using System.Collections;

public class CameraBox : MonoBehaviour {

    //Cameras
    public Camera[] cams;

    //Indexs
    int currentIndex = 0;

	// Use this for initialization
	void Start ()
    {
	   foreach (Camera c in cams)
       {
            c.enabled = false;
       }
        cams[currentIndex].enabled = true;
	}
	
	// Update is called once per frame
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
