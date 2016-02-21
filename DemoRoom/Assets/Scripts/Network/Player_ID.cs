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
    // 
    public string prefabName = "PlayerV2";

    [SyncVar]
    private string playerTeamTag;

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
	void Update() 
	{
		if (myTransform.name == "" || myTransform.name == string.Format("{0}(Clone)", prefabName))
		{
			SetIdentity();
		}

        if (gameObject.tag.Equals("Player") && !string.IsNullOrEmpty(playerTeamTag))
        {
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
		string uniqueName = "Player " + playerNetID.ToString();
		return uniqueName;
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
