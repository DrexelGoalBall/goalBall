using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

/// <summary>
///     Synchronizes the necessary rotations of the player over the network
/// </summary>
public class Player_SyncRotation : NetworkBehaviour 
{
    // Player's rotation (z-axis) float to sync clients with from server
	[SyncVar (hook = "SyncPlayerRotation")] private float syncPlayerRotation;
    // Player's camera rotation (x-axis) float to sync clients with from server
	[SyncVar (hook = "SyncCameraRotation")] private float syncCamRotation;

    // Current transformation values for player
	[SerializeField] private Transform playerTransform;
	// Current transformation values for camera
    [SerializeField] private Transform camTransform;

    // The default rate to lerp
	private float lerpRate = 20;

    // The previous rotation value of this player when last rotated
	private float lastPlayerRot;
    // The previous rotation value of this camera when last rotated
    private float lastCamRot;
    // Difference between current rotation and last rotation to consider updating
	private float rotationThreshold = 0.3f;

    // Whether or not to use historical interpolating
    [SerializeField] private bool useHistoricalInterpolation;
    // For historical interpolation, list of player rotations necessary to lerp through
	private List<float> syncPlayerRotList = new List<float>();
    // For historical interpolation, list of camera rotations necessary to lerp through
	private List<float> syncCamRotList = new List<float>();
    // For historical lerping, amount between current rotation and next rotation in list to consider reached
	private float closeEnough = 0.4f;

    /// <summary>
    ///     Every frame, lerp the rotation of this non-local player
    /// </summary>
	void Update()
	{
		LerpRotations();
	}

    /// <summary>
    ///     Every fixed frame, send the significant changes in rotation of local player to server
    /// </summary>
	void FixedUpdate()
	{
        if (isClient)
        {
            TransmitRotations();
        }
	}

    /// <summary>
    ///     If not the local player, lerp to display correct rotation
    /// </summary>
	void LerpRotations()
	{
		if (!isLocalPlayer)
		{
            // Determine which lerping has been selected to be used
			if (useHistoricalInterpolation)
			{
				HistoricalInterpolation();
			}
			else
			{
				OrdinaryLerping();
			}
		}
	}

    /// <summary>
    ///     On server, updates the rotation for this player so all clients can lerp this player to it
    /// </summary>
    /// <param name="playerRot">Rotation of the player sent from client to populate to others</param>
    /// <param name="playerRot">Rotation of the camera sent from client to populate to others</param>
    [Command]
    void CmdProvideRotationsToServer(float playerRot, float camRot)
    {
        syncPlayerRotation = playerRot;
        syncCamRotation = camRot;
    }

    /// <summary>
    ///     On the local client, sends significant player/camera rotation changes to server to be populated for ordinary lerping
    /// </summary>
    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer)
        {
            // Determine if player has rotated enough
            if (CheckIfBeyondThreshold(playerTransform.localEulerAngles.z, lastPlayerRot) || CheckIfBeyondThreshold(camTransform.localEulerAngles.x, lastCamRot))
            {
                lastPlayerRot = playerTransform.localEulerAngles.z;
                lastCamRot = camTransform.localEulerAngles.x;
                CmdProvideRotationsToServer(lastPlayerRot, lastCamRot);
            }
        }
    }

    /// <summary>
    ///     On the client, adds the provided player rotation to the list for historical lerping
    /// </summary>
    /// <param name="latestPlayerRot">Latest rotation of player</param>
    [Client]
    void SyncPlayerRotation(float latestPlayerRot)
    {
        syncPlayerRotation = latestPlayerRot;
        syncPlayerRotList.Add(syncPlayerRotation);
    }

    /// <summary>
    ///     On the client, adds the provided camera rotation to the list for historical lerping
    /// </summary>
    /// <param name="latestCamRot">Latest rotation of camera</param>
    [Client]
    void SyncCameraRotation(float latestCamRot)
    {
        syncCamRotation = latestCamRot;
        syncCamRotList.Add(syncCamRotation);
    }

    /// <summary>
    ///     Lerps the player/camera by providing it the most recent rotation everytime
    /// </summary>
    void OrdinaryLerping()
    {
        LerpPlayerRotation(syncPlayerRotation);
        LerpCamRot(syncCamRotation);
    }

    /// <summary>
    ///     Lerps the player/camera using a list of all rotations needed to lerp through
    /// </summary>
	void HistoricalInterpolation()
	{
        // Check if there are any player rotations to sync to
		if (syncPlayerRotList.Count > 0)
		{
            // Lerp the rotation of the player
			LerpPlayerRotation(syncPlayerRotList[0]);

			if (Mathf.Abs(playerTransform.localEulerAngles.z - syncPlayerRotList[0]) < closeEnough)
			{
                // Remove the rotation from the list when we have gotten close enough to it
				syncPlayerRotList.RemoveAt(0);
			}
		}

        // Check if there are any camera rotations to sync to
		if (syncCamRotList.Count > 0)
		{
            // Lerp the rotation of the camera 
			LerpCamRot(syncCamRotList[0]);

			if (Mathf.Abs(camTransform.localEulerAngles.x - syncCamRotList[0]) < closeEnough)
			{
                // Remove the rotation from the list when we have gotten close enough to it
				syncCamRotList.RemoveAt(0);
			}
		}
	}

    /// <summary>
    ///     Lerps the rotation of the player
    /// </summary>
    /// <param name="rotAngle">Angle to lerp to</param>
	void LerpPlayerRotation(float rotAngle)
	{
        Vector3 playerNewRot = new Vector3(0, 0, rotAngle);
		playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.Euler(playerNewRot), lerpRate * Time.deltaTime);
	}

    /// <summary>
    ///     Lerps the rotation of the camera
    /// </summary>
    /// <param name="rotAngle">Angle to lerp to</param>
	void LerpCamRot(float rotAngle)
	{
		Vector3 camNewRot = new Vector3(rotAngle, 0, 0);
		camTransform.localRotation = Quaternion.Lerp(camTransform.localRotation, Quaternion.Euler(camNewRot), lerpRate * Time.deltaTime);
	}

    /// <summary>
    ///     Determines if the difference between the two values is larger than the threshold
    /// </summary>
    /// <param name="rot1"></param>
    /// <param name="rot2"></param>
	bool CheckIfBeyondThreshold(float rot1, float rot2)
	{
		return (Mathf.Abs(rot1 - rot2) > rotationThreshold);
	}
}
