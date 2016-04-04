using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

/// <summary>
///     Synchronizes the rotation of the player over the network
/// </summary>
public class Player_SyncRotation : NetworkBehaviour 
{
    // 
	[SyncVar (hook = "OnPlayerRotSynced")] private float syncPlayerRotation;
    // 
	[SyncVar (hook = "OnCamRotSynced")] private float syncCamRotation;

    // 
	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform camTransform;

    // 
	private float lerpRate = 20;

    // 
	private float lastPlayerRot;
	private float lastCamRot;
    // 
	private float threshold = 0.3f;

    // 
	private List<float> syncPlayerRotList = new List<float>();
    // 
	private List<float> syncCamRotList = new List<float>();
    // 
	private float closeEnough = 0.4f;
    // 
	[SerializeField] private bool useHistoricalInterpolation;

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
		TransmitRotations();
	}

    /// <summary>
    ///     If not the local player, lerp to display correct rotation
    /// </summary>
	void LerpRotations()
	{
        // 
		if (!isLocalPlayer)
		{
            // 
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
    void OnPlayerRotSynced(float latestPlayerRot)
    {
        syncPlayerRotation = latestPlayerRot;
        syncPlayerRotList.Add(syncPlayerRotation);
    }

    /// <summary>
    ///     On the client, adds the provided camera rotation to the list for historical lerping
    /// </summary>
    /// <param name="latestCamRot">Latest rotation of camera</param>
    [Client]
    void OnCamRotSynced(float latestCamRot)
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
        // 
		if(syncPlayerRotList.Count > 0)
		{
            // 
			LerpPlayerRotation(syncPlayerRotList[0]);

            // 
			if(Mathf.Abs(playerTransform.localEulerAngles.z - syncPlayerRotList[0]) < closeEnough)
			{
				syncPlayerRotList.RemoveAt(0);
			}

			//Debug.Log(syncPlayerRotList.Count.ToString() + " syncPlayerRotList Count");
		}

        // 
		if(syncCamRotList.Count > 0)
		{
            // 
			LerpCamRot(syncCamRotList[0]);

            // 
			if(Mathf.Abs(camTransform.localEulerAngles.x - syncCamRotList[0]) < closeEnough)
			{
				syncCamRotList.RemoveAt(0);
			}

			//Debug.Log(syncCamRotList.Count.ToString() + " syncCamRotList Count");
		}
	}

    /// <summary>
    ///     Lerps the rotation of the player
    /// </summary>
    /// <param name="rotAngle">Angle to lerp to</param>
	void LerpPlayerRotation(float rotAngle)
	{
		Vector3 playerNewRot = new Vector3(0, rotAngle, 0);
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
        // 
		if (Mathf.Abs(rot1-rot2) > threshold)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
