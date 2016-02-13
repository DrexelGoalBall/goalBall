using UnityEngine;
using System.Collections;

public class ListObjectLocation : MonoBehaviour {
    /// <summary>
    /// This script will list the last collider that an object collided with.  This is used to tell the location of an object as it enters a new trigger area.
    /// </summary>

    public string currentArea;

    /// <summary>
    /// Detects when the object enters a trigger and updates the currentArea if the object is not tagged with Floor.
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Floor") return;
        currentArea = col.name;
        Debug.Log(col.name);
    }

}
