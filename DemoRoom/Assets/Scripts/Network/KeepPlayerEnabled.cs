using UnityEngine;
using System.Collections;

public class KeepPlayerEnabled : MonoBehaviour 
{
    // Array of objects to enable
    public GameObject[] objectsToKeepOn;

    void Start()
    {
        foreach (GameObject X in objectsToKeepOn)
        {
            X.SetActive(true);
        }
    }


	// Update is called once per frame
	void Update () {
	
	}
}
