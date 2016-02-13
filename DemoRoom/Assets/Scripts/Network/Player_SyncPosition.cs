using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
///     Synchronizes the position of the player over the network
/// </summary>
[NetworkSettings (channel = 0, sendInterval = 0.01f)]
public class Player_SyncPosition : NetworkBehaviour 
{
    // 
	[SyncVar (hook = "SyncPositionValues")]
	private Vector3 syncPos;

    // 
	[SerializeField] Transform myTransform;
	
    // 
    private float lerpRate;
	private float normalLerpRate = 16;
	private float fasterLerpRate = 27;

    // 
	private Vector3 lastPos;
    // 
	private float threshold = 0.1f;

    // 
	private List<Vector3> syncPosList = new List<Vector3>();
    // 
	[SerializeField] private bool useHistoricalLerping = false;
    // 
	private float closeEnough = 0.11f;

    // 
    //private Transform playerStartTransform;

    /// <summary>
    ///     Sets up the initial values
    /// </summary>
	void Start()
	{
		lerpRate = normalLerpRate;

        //playerStartTransform = transform;
	}

    /// <summary>
    ///     Every frame, lerp the position of this non-local player
    /// </summary>
	void Update()
	{
		LerpPosition();
	}

    /// <summary>
    ///     Every fixed frame, send significant changes in position of local player to server
    /// </summary>
	void FixedUpdate() 
	{
		TransmitPosition();
	}

    /// <summary>
    ///     If not the local player, lerp to display correct position
    /// </summary>
	void LerpPosition()
	{
		if (!isLocalPlayer)
		{
			if (useHistoricalLerping)
			{
				HistoricalLerping();
			}
			else
			{
				OrdinaryLerping();
			}

			//Debug.Log(Time.deltaTime.ToString());
		}
	}

    /// <summary>
    ///     On server, updates the position for this player so all clients can lerp this player to it
    /// </summary>
    /// <param name="pos">Position sent from client to populate to others</param>
	[Command]
	void CmdProvidePositionToServer (Vector3 pos)
	{
		syncPos = pos;
		Debug.Log("Command called");
	}

    /// <summary>
    ///     On the local client, sends significant position changes to server to be populated for ordinary lerping
    /// </summary>
	[ClientCallback]
	void TransmitPosition()
	{
		if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
		{
			CmdProvidePositionToServer(myTransform.position);
			lastPos = myTransform.position;
		}
	}

    /// <summary>
    ///     On the client, adds the provided position to the list for historical lerping
    /// </summary>
    /// <param name="latestPos">Latest position of player</param>
	[Client]
	void SyncPositionValues(Vector3 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add(syncPos);
	}

    /// <summary>
    ///     Lerps the player by providing it the most recent position everytime
    /// </summary>
	void OrdinaryLerping()
	{
		myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
	}

    /// <summary>
    ///     Lerps the player using a list of all positions needed to lerp through
    /// </summary>
	void HistoricalLerping()
	{
        // 
		if(syncPosList.Count > 0)
		{
            // 
			myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

            // 
			if(Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
			{
				syncPosList.RemoveAt(0);
			}

            // 
			if(syncPosList.Count > 10)
			{
				lerpRate = fasterLerpRate;
			}
			else
			{
				lerpRate = normalLerpRate;
			}

			//Debug.Log(syncPosList.Count.ToString());
		}
	}
}
