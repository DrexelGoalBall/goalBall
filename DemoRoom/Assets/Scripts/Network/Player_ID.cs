using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
///     Sets up a unique identity for the player
/// </summary>
public class Player_ID : NetworkBehaviour 
{
    // String containing unique id to sync to all connections
	[SyncVar] private string playerUniqueIdentity;
    // String containing the tag for this player's team to sync to all connections
    [SyncVar] private string playerTeamTag;

    // The id for this object provided by the network connection
	private NetworkInstanceId playerNetID;
    // Current transformation values
	private Transform myTransform;
    
    // Name of the prefab, since by default Unity adds duplicates with (Clone)
    public string prefabName = "PlayerV2";

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
    ///     Set the identity and tag of this object if it has not been properly set yet
    /// </summary>
	void Update() 
	{
		if (myTransform.name == "" || myTransform.name == string.Format("{0}(Clone)", prefabName))
		{
            // Name of object is not currently unique, so try to set it
			SetIdentity();
		}

        if (gameObject.tag.Equals("Player") && !string.IsNullOrEmpty(playerTeamTag))
        {
            // Team tag has not been updated, so try to update it
            gameObject.tag = playerTeamTag;
        }
	}

    /// <summary>
    ///     On server, update the identity for this player so all clients will as well
    /// </summary>
    /// <param name="name">Name of player sent from client to populate to others</param>
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
        return "Player " + playerNetID.ToString();
	}

    /// <summary>
    ///     On server, update the tag for this player so all clients will as well
    /// </summary>
    /// <param name="tag">Team tag of player sent from client to populate to others</param>
    [Command]
    void CmdTellServerMyTeamTag(string tag)
    {
        playerTeamTag = tag;
    }

    /// <summary>
    ///     On client, get the network identity for this player
    /// </summary>
    [Client]
    public void SetTeamTag(string tag)
    {
        if (isLocalPlayer)
            CmdTellServerMyTeamTag(tag);
    }
}
