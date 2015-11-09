using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPointTest : MonoBehaviour 
{
    public List<GameObject> spawns = new List<GameObject>();

	// Use this for initialization
	void Start () 
    {
	    foreach (GameObject s in spawns)
        {
            s.SetActive(true);
        }
	}
}
