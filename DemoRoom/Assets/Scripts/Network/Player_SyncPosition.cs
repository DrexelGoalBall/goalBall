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
    // Position vector to sync clients with from server
	[SyncVar (hook = "SyncPositionValues")] private Vector3 syncPos;

    // Current transformation values
	[SerializeField] private Transform myTransform;

    // The current rate at which lerping is occurring
    private float lerpRate;
    // The default rate to lerp
	private float normalLerpRate = 16;
    // For historical lerping, the rate to lerp when updates are coming faster than they can be lerped to
	private float fasterLerpRate = 27;
    // For historical lerping, highest number of positions in list before faster rate is used
    public int catchUpTreshold = 10;

    // The previous position of this player when last moved
	private Vector3 lastPos;
    // Difference between current position and last position to consider updating
	private float positionThreshold = 0.1f;

    // Whether or not to use historical lerping
    [SerializeField] private bool useHistoricalLerping = false;
    // For historical lerping, list of positions necessary to lerp through
	private List<Vector3> syncPosList = new List<Vector3>();
    // For historical lerping, distance between current position and next position in list to consider reached
	private float closeEnough = 0.11f;

    /// <summary>
    ///     Sets up the initial values
    /// </summary>
	void Start()
	{
		lerpRate = normalLerpRate;
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
            // Determine which lerping has been selected to be used
			if (useHistoricalLerping)
			{
				HistoricalLerping();
			}
			else
			{
				OrdinaryLerping();
			}
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
	}

    /// <summary>
    ///     On the local client, sends significant position changes to server to be populated for ordinary lerping
    /// </summary>
	[ClientCallback]
	void TransmitPosition()
	{
		if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > positionThreshold)
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
    ///     Lerps the player by providing it the most recent position every time
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
        // Check if there are any positions to sync to
		if (syncPosList.Count > 0)
		{
            // Move this object toward the next position
			myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

			if (Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
			{
                // Remove the position from the list when we have gotten close enough to it
				syncPosList.RemoveAt(0);
			}

            // Increase the rate if necessary to catch up with position updates
            if (syncPosList.Count > catchUpTreshold)
			{
				lerpRate = fasterLerpRate;
			}
			else
			{
				lerpRate = normalLerpRate;
			}
		}
	}
}
