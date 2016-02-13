using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
///     Sets up a unique identity for the player
/// </summary>
public class Player_ID : NetworkBehaviour 
{
    // 
	[SyncVar] private string playerUniqueIdentity;
    // 
	private NetworkInstanceId playerNetID;
    // 
	private Transform myTransform;

    /// <summary>
    ///     When the local player object is set up, get and set its unique identity
    /// </summary>
	public override void OnStartLocalPlayer()
	{
		GetNetIdentity();
		SetIdentity();
	}

    /// <summary>
    ///     Initializes variables when script starts
    /// </summary>
	void Awake() 
	{
		myTransform = transform;
	}

    /// <summary>
    ///     Checks whether identity is not unique and updates it accordingly
    /// </summary>
	void Update () 
	{
		if(myTransform.name == "" || myTransform.name == "PlayerV2(Clone)")
		{
			SetIdentity();
		}
	}

    /// <summary>
    ///     On server, update the identity for this player so all clients will as well
    /// </summary>
    /// <param name="name">Name of player set from client to populate to others</param>
    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }

    /// <summary>
    ///     On client, get the network identity for this player
    /// </summary>
	[Client]
	void GetNetIdentity()
	{
		playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdTellServerMyIdentity(MakeUniqueIdentity());
	}

    /// <summary>
    ///     If local player, make unique identity, otherwise set the provided identity
    /// </summary>
	void SetIdentity()
	{
		if (!isLocalPlayer)
		{
			myTransform.name = playerUniqueIdentity;
		}
		else
		{
			myTransform.name = MakeUniqueIdentity();
		}
	}

    /// <summary>
    ///     Determine the unique identity based on its network instance id
    /// </summary>
	string MakeUniqueIdentity()
	{
		string uniqueName = "Player " + playerNetID.ToString();
		return uniqueName;
	}
}
