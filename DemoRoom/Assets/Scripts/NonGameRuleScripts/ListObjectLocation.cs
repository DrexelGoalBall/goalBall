using UnityEngine;
using System.Collections;

/// <summary>
/// This script will list the last collider that an object collided with.  This is used to tell the location of an object as it enters a new trigger area.
/// </summary>
public class ListObjectLocation : MonoBehaviour
{
    public string currentArea;
    private Transform objectTransfom;
    private float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    public bool isMoving;

    public bool IsMoving
    {
        get { return isMoving; }
    }

    void Start()
    {
        objectTransfom = gameObject.GetComponent<Transform>();
    }

    void Awake()
    {
        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }

    void Update()
    {
        //Store the newest vector at the end of the list of vectors
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = objectTransfom.position;

        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
                break;
            }
        }
    }

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



