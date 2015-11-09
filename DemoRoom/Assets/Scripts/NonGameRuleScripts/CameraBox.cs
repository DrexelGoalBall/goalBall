using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraBox : MonoBehaviour {

    //Cameras
    public List<Camera> cams = new List<Camera>();

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
            if (cams.Count > 1)
            {
                cams[currentIndex].enabled = false;
                if (currentIndex == cams.Count - 1)
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
