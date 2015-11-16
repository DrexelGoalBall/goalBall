using UnityEngine;
using System.Collections;

public class ListObjectLocation : MonoBehaviour {

    public string currentArea;

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "floor") return;
        currentArea = col.name;
        Debug.Log(col.name);
    }

}
