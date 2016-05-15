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
    // Reference to the ServerLink script
    public ServerLink serverLink;

    // Main Menu Components
    private bool joining = false;
    private Text networkStatusText;

    // Game Components
    private Button disconnectButton;
    private Text capacityText;
    private Text connInfoText;

    // Indices and corresponding strings of selected team and position
    public int teamIndex = 0, positionIndex = 0;
    public string team = "", position = "";

    // Tag string to check for server setup info object
    public string serverSetupInfoTag = "ServerSetupInfo";

    /// <summary>
    ///     When started, update the UI for the loaded scene and sets the maximum allowed connections
    /// </summary>
    void Start()
    {
        // Set UI up for the scene this script was first loaded for  
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
        // Check if we are in a networked game
        if (NetworkManager.singleton.isNetworkActive)
        {
            if (capacityText != null)
            {
                // Get all of the players for both teams
                GameObject[] redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
                GameObject[] bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
                GameObject[] players = redPlayers.Concat(bluePlayers).ToArray();
                // Show current player count
                capacityText.text = string.Format("{0}/{1}", players.Length, NetworkManager.singleton.maxConnections);
            }

            if (connInfoText != null)
            {
                if (NetworkServer.active)
                {
                    string conn = "Server";
                    if (NetworkClient.active)
                        conn = "Host";
                    // Display connection information for this Server/Host
                    connInfoText.text = string.Format("{0}  |  Address = {1}  |  Port = {2}", conn, Network.player.ipAddress, NetworkManager.singleton.networkPort) ;
                }
                else if (NetworkClient.active)
                {
                    // Display connection information for this client
                    connInfoText.text = string.Format("Client  |  Address = {0}  |  Port = {1}", NetworkManager.singleton.networkAddress, NetworkManager.singleton.networkPort);
                }
            }
        }
        else
        {
            // Determine if this instance is for a server
            GameObject serverSetupInfo = GameObject.FindGameObjectWithTag(serverSetupInfoTag);
            if (serverSetupInfo != null)
            {
                // Get the port to create the match on
                NetworkManager.singleton.networkPort = serverSetupInfo.GetComponent<ServerSetupInfo>().portNumber;
                // Remove the server setup info object as it has served its purpose
                //GameObject.Destroy(serverInfo);
                // Start the game
                ServerDirect();
            }
        }
    }

    /// <summary>
    ///     Attempt to start a network game on the Goalball server
    /// </summary>
    public void GoalballServer()
    {
        // Connect to the server if it is not already connected
        if (!serverLink.connector.isConnected())
        {
            serverLink.ConnectToServer();
        }

        // If the server is connected, request to create a game to join
        if (serverLink.connector.isConnected())
        {
            serverLink.connector.CreateGame();
        }
    }

    /// <summary>
    ///     Starts a network game without a local player
    /// </summary>
    public void ServerDirect()
    {
        UpdateStatusUI("Creating local match...");
        // Update the connection information and start as a server
        NetworkManager.singleton.networkAddress = "localhost";
        NetworkManager.singleton.StartServer();
    }

    /// <summary>
    ///     Starts a network game with a local player
    /// </summary>
    public void HostDirect()
    {
        UpdateStatusUI("Hosting local match...");
        // Update the connection information and start as a host
        NetworkManager.singleton.networkAddress = "localhost";
        NetworkManager.singleton.StartHost();
    }

    /// <summary>
    ///     Attempt to join a network game at the provided address and port with a local player
    /// </summary>
    private void JoinDirect()
    {
        string address = "localhost";
        // Check if an address was provided
        GameObject addressField = GameObject.Find("AddressField");
        if (addressField != null)
        {
            string temp = addressField.GetComponent<InputField>().text;
            if (!string.IsNullOrEmpty(temp))
                address = temp;
        }
        JoinDirect(address, NetworkManager.singleton.networkPort);
    }

    /// <summary>
    ///     Attempt to join a network game at the provided address and port with a local player
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void JoinDirect(string address, int port)
    {
        // Check if the player is already joining a game
        if (!joining)
        {
            joining = true;
            // Update the connection information
            NetworkManager.singleton.networkAddress = address;
            NetworkManager.singleton.networkPort = port;            
            UpdateStatusUI(string.Format("Joining match on {0}...", NetworkManager.singleton.networkAddress));
            // Join as a client
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
    /// <param name="level">Level number</param>
    void OnLevelWasLoaded(int level)
    {
        if (NetworkManager.singleton.offlineScene.Equals(Application.loadedLevelName))
        {
            // Set menu UI if the current level is the offline scene
            StartCoroutine(SetupMenuSceneUI());
        }
        else if (NetworkManager.singleton.onlineScene.Equals(Application.loadedLevelName))
        {
            // Set up the game UI otherwise
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
        Debug.Log("Client Connect");
        joining = false;
        base.OnClientConnect(conn);
    }

    /// <summary>
    ///     When client disconnects, handles moving back to menu
    ///     Overridden to display whether join failed and update joining bool
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Client Disconnect");
        if (joining)
        {
            UpdateStatusUI("Failed to join.");
            Debug.Log(conn.address + " - " + conn.hostId);
        }
        joining = false;
        base.OnClientDisconnect(conn);
    }

    /// <summary>
    ///     On server, when client joins, add player
    ///     Overridden to update joining bool
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Server Connect");
        joining = false;
        base.OnServerConnect(conn);
    }

    /// <summary>
    ///     On server, when client disconnects, remove player connection
    ///     Overridden to update joining bool
    /// </summary>
    /// <param name="conn">Connection from the client</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Server Disconnect");
        joining = false;
        base.OnServerDisconnect(conn);
    }

    /// <summary>
    ///     Hook called when client is stopped
    ///     Overridden to update joining bool
    /// </summary>
    public override void OnStopClient()
    {
        Debug.Log("Stop Client");
        joining = false;
        //base.OnStopClient();
    }

    /// <summary>
    ///     Hook called when host is stopped
    ///     Overridden to update joining bool
    /// </summary>
    public override void OnStopHost()
    {
        Debug.Log("Stop Host");
        joining = false;
        //base.OnStopHost();
    }

    /// <summary>
    ///     Hook called when server is stopped
    ///     Overridden to update joining bool
    /// </summary>
    public override void OnStopServer()
    {
        Debug.Log("Stop Server");
        joining = false;
        base.OnStopServer();
    }

    /// <summary>
    ///     Finds and sets up UI elements in the menu scene
    /// </summary>
    IEnumerator SetupMenuSceneUI()
    {
        yield return new WaitForSeconds(0.3f);

        GameObject networkStatusObject = GameObject.Find("NetworkStatusText");
        if (networkStatusObject != null)
        {
            networkStatusText = networkStatusObject.GetComponent<Text>();
        }

        GameObject serverButton = GameObject.Find("ServerButton");
        if (serverButton != null)
        {
            serverButton.GetComponent<Button>().onClick.RemoveAllListeners();
            serverButton.GetComponent<Button>().onClick.AddListener(ServerDirect);
        }

        GameObject hostButton = GameObject.Find("HostButton");
        if (hostButton != null)
        {
            hostButton.GetComponent<Button>().onClick.RemoveAllListeners();
            hostButton.GetComponent<Button>().onClick.AddListener(HostDirect);
        }

        GameObject joinButton = GameObject.Find("JoinButton");
        if (joinButton != null)
        {
            joinButton.GetComponent<Button>().onClick.RemoveAllListeners();
            joinButton.GetComponent<Button>().onClick.AddListener(JoinDirect);
        }
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

    /// <summary>
    ///     Updates the UI status element
    /// </summary>
    /// <param name="status">String containing the updated status</param>
    public void UpdateStatusUI(string status)
    {
        if (networkStatusText != null)
        {
            networkStatusText.text = status;
        }
    }
}