using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using System.Linq;

/// <summary>
///     Custom NetworkManager that handles all facets of connecting and disconnecting to games
/// </summary>
public class NetworkManager_Custom : NetworkManager
{
    // Main Menu Components
    private bool joining = false;
    private Text networkInfoText;

    // Game Components
    private Button disconnectButton;
    private Text capacityText;
    public Text playerElementText;
    private Text connInfoText;

    public GameObject ball;

    // Indices and corresponding strings of selected team and position
    public int teamIndex = 0, positionIndex = 0;
    public string team = "", position = "";

    /// <summary>
    ///     When started, updates the UI depending on the loaded scene and sets the maximum allowed connections
    /// </summary>
    void Start()
    {
        // Update the UI depending on the scene that was loaded
        OnLevelWasLoaded(Application.loadedLevel);
        // Set the maximum number of players
        NetworkManager.singleton.maxConnections = 6;
        //NetworkManager.singleton.matchSize = 6;
    }

    /// <summary>
    ///     Updates the in-game UI to display the connection information and current number of players connected
    /// </summary>
    void Update()
    {
        // 
        if (NetworkManager.singleton.isNetworkActive)
        {
            // Spawn new ball if one is not there
            //if (NetworkServer.active && GameObject.FindGameObjectWithTag("Ball") == null)
            //    NetworkServer.Spawn(ball);

            // 
            GameObject[] redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
            GameObject[] bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
            GameObject[] players = redPlayers.Concat(bluePlayers).ToArray();

            // 
            if (capacityText != null)
            {
                capacityText.text = string.Format("{0}/{1}", players.Length, NetworkManager.singleton.maxConnections);
            }

            // 
            if (connInfoText != null)
            {
                // 
                if (NetworkServer.active)
                {
                    // 
                    string conn = "Server";
                    if (NetworkClient.active)
                        conn = "Host";
                    connInfoText.text = string.Format("{0}  |  Address = {1}  |  Port = {2}", conn, Network.player.ipAddress, NetworkManager.singleton.networkPort) ;
                }
                else if (NetworkClient.active)
                {
                    connInfoText.text = string.Format("Client  |  Address = {0}  |  Port = {1}", NetworkManager.singleton.networkAddress, NetworkManager.singleton.networkPort);
                }
            }
        }
    }

    /// <summary>
    ///     Starts a network game without a local player
    /// </summary>
    public void ServerDirect()
    {
        // 
        networkInfoText.text = "Creating local match...";
        NetworkManager.singleton.networkAddress = "localhost";
        //NetworkManager.singleton.networkPort = 7777;
        // 
        NetworkManager.singleton.StartServer();
    }

    /// <summary>
    ///     Starts a network game with a local player
    /// </summary>
    public void HostDirect()
    {
        // 
        networkInfoText.text = "Hosting local match...";
        NetworkManager.singleton.networkAddress = "localhost";
        //NetworkManager.singleton.networkPort = 7777;
        // 
        NetworkManager.singleton.StartHost();
    }

    /// <summary>
    ///     Tries to join a network game at the provided address and port with a local player
    /// </summary>
    public void JoinDirect()
    {
        // 
        if (!joining)
        {
            // 
            joining = true;
            // 
            string address = GameObject.Find("AddressField").GetComponent<InputField>().text;
            if (string.IsNullOrEmpty(address))
                address = "localhost";
            NetworkManager.singleton.networkAddress = address;
            //NetworkManager.singleton.networkPort = 7777;
            networkInfoText.text = string.Format("Joining match on {0}...", address);
            // 
            NetworkManager.singleton.StartClient();
        }
    }

    /// <summary>
    ///     When a client connects to the game, move it to the landing point before actually spawning in position
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    /// <param name="playerControllerId">Id associated with the connected client's player</param>
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Add Player");
        
        // Get the landing point to spawn player
        Transform spawnLoc = GameObject.Find("LandingPoint").transform;
        // Create the player
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, spawnLoc.position, Quaternion.identity);
        // Add the player to the game
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    /// <summary>
    ///     Sets up the UI based on the loaded scene
    /// </summary>
    void OnLevelWasLoaded(int level)
    {
        // 
        if (level == 0)
        {
            StartCoroutine(SetupMenuSceneUI());
        }
        else
        {
            SetupGameSceneUI();
        }
    }

    /// <summary>
    ///     When client connects, sets client to ready and adds player
    ///     Overridden to update joining bool
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        joining = false;
        Debug.Log("Client Connect");
    }

    /// <summary>
    ///     When client disconnects, handles moving back to menu
    ///     Overridden to display whether join failed and update joining bool
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (joining)
        {
            networkInfoText.text = "Failed to join.";
            Debug.Log(conn.address + " - " + conn.hostId);
            //if (conn.address.Trim().Equals("127.0.0.1"))
            //    HostDirect();
        }
        joining = false;
        Debug.Log("Client Disconnect");

        base.OnClientDisconnect(conn);
    }

    /*public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        
        joining = false;
        Debug.Log("Client Error");
    }*/

    /// <summary>
    ///     On server, when client joins, add player
    ///     Overridden to update joining bool
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        joining = false;
        Debug.Log("Server Connect");
    }

    /// <summary>
    ///     On server, when client disconnects, remove player connection
    ///     Overridden to update joining bool
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        //if (ball != null)
        //{
        //    GameObject g = ball;
        //    while (g.transform.parent != null)
        //    {
        //        g = g.transform.parent.gameObject;
        //    }

        //    CatchThrowV2 ct = g.GetComponent<CatchThrowV2>();
        //    if (ct != null)
        //    {
        //        Debug.Log("Ball is going to be deleted, save it!");
        //        ball.transform.parent = null;
        //        ct.Drop();
        //    }
        //    //NetworkIdentity ni = g.GetComponent<NetworkIdentity>();
        //    //if (ni != null && ni.isLocalPlayer)
        //    //    g.transform.parent = null;
        //}

        base.OnServerDisconnect(conn);

        joining = false;
        Debug.Log("Server Disconnect");
    }
    
    /*public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
        
        joining = false;
        Debug.Log("Server Error");
    }*/

    /// <summary>
    ///     Hook called when client is stopped
    ///     Overridden to update joining bool
    /// </summary>
    public override void OnStopClient()
    {
        //base.OnStopClient();

        joining = false;
        Debug.Log("Stop Client");
    }

    /// <summary>
    ///     Hook called when host is stopped
    ///     Overridden to update joining bool
    /// </summary>
    public override void OnStopHost()
    {
        //base.OnStopHost();

        joining = false;
        Debug.Log("Stop Host");
    }

    /// <summary>
    ///     Hook called when server is stopped
    ///     Overridden to update joining bool
    /// </summary>
    public override void OnStopServer()
    {
        base.OnStopServer();

        joining = false;
        Debug.Log("Stop Server");
    }

    /// <summary>
    ///     Finds and sets up UI elements in the menu scene
    /// </summary>
    IEnumerator SetupMenuSceneUI()
    {
        yield return new WaitForSeconds(0.3f);

        networkInfoText = GameObject.Find("NetworkInfoText").GetComponent<Text>();

        GameObject.Find("ServerButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ServerButton").GetComponent<Button>().onClick.AddListener(ServerDirect);

        GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(HostDirect);

        GameObject.Find("JoinButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("JoinButton").GetComponent<Button>().onClick.AddListener(JoinDirect);
    }

    /// <summary>
    ///     Finds and sets up UI elements in the game scene
    /// </summary>
    void SetupGameSceneUI()
    {
        disconnectButton = GameObject.Find("Disconnect").GetComponent<Button>();
        disconnectButton.onClick.RemoveAllListeners();
        disconnectButton.onClick.AddListener(NetworkManager.singleton.StopHost);

        capacityText = GameObject.Find("Capacity").GetComponent<Text>();

        connInfoText = GameObject.Find("ConnectionInfo").GetComponent<Text>();
    }
}