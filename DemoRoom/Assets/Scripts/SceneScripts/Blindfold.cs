using UnityEngine;
using System.Collections;

/// <summary>
/// This is a debugging script to blind fold the player on button command.
/// </summary>
public class Blindfold : MonoBehaviour {

    public GameObject blindfold;

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.B))
        {
            if (blindfold.activeSelf) blindfold.SetActive(false);
            else blindfold.SetActive(true);
                
        }
	}
}
