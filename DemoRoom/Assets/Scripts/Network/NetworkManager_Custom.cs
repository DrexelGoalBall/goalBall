using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager
{
    /// <summary>
    ///     Custom NetworkManager that handles all facets of connecting and disconnecting to games
    /// </summary>

    // Main Menu Components
    private bool joining = false;
    private Text networkInfoText;

    // Game Components
    private Button disconnectButton;
    private Text capacityText;
    public Text playerElementText;
    private Text connInfoText;

    // Indices and corresponding strings of selected team and position
    public int teamIndex = 0, positionIndex = 0;
    public string team = "", position = "";

    void Start()
    {
        // Update the UI depending on the scene that was loaded
        OnLevelWasLoaded(Application.loadedLevel);
        // Set the maximum number of players
        NetworkManager.singleton.maxConnections = 6;
        //NetworkManager.singleton.matchSize = 6;
    }

    void Update()
    {
        if (NetworkManager.singleton.isNetworkActive)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (capacityText != null)
            {
                capacityText.text = string.Format("{0}/{1}", players.Length, NetworkManager.singleton.maxConnections);
            }

            if (connInfoText != null)
            {
                if (NetworkServer.active)
                {
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

    public void ServerDirect()
    {
        networkInfoText.text = "Creating local match...";
        NetworkManager.singleton.networkAddress = "localhost";
        //NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartServer();
    }

    public void HostDirect()
    {
        networkInfoText.text = "Hosting local match...";
        NetworkManager.singleton.networkAddress = "localhost";
        //NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartHost();
    }

    public void JoinDirect()
    {
        if (!joining)
        {
            joining = true;
            string address = GameObject.Find("AddressField").GetComponent<InputField>().text;
            if (string.IsNullOrEmpty(address))
                address = "localhost";
            NetworkManager.singleton.networkAddress = address;
            //NetworkManager.singleton.networkPort = 7777;
            networkInfoText.text = string.Format("Joining match on {0}...", address);
            NetworkManager.singleton.StartClient();
        }
    }

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

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            StartCoroutine(SetupMenuSceneButtons());
        }
        else
        {
            SetupOtherSceneButtons();
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        joining = false;
        Debug.Log("Client Connect");
    }

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

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        joining = false;
        Debug.Log("Server Connect");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
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

    public override void OnStopClient()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            GameObject g = ball;
            while (g.transform.parent != null)
            {
                g = g.transform.parent.gameObject;
            }

            CatchThrowV2 ct = g.GetComponent<CatchThrowV2>();
            if (ct != null && ct.isLocalPlayer)
            {
                Debug.Log("Ball is going to be deleted, save it!");
                //ct.DropBall();
            }
            //NetworkIdentity ni = g.GetComponent<NetworkIdentity>();
            //if (ni != null && ni.isLocalPlayer)
            //    g.transform.parent = null;
        }

        //base.OnStopClient();

        joining = false;
        Debug.Log("Stop Client");
    }

    public override void OnStopHost()
    {
        //base.OnStopHost();

        joining = false;
        Debug.Log("Stop Host");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        joining = false;
        Debug.Log("Stop Server");
    }

    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.3f);

        networkInfoText = GameObject.Find("NetworkInfoText").GetComponent<Text>();

        ShowDirectMenu();
    }

    void SetupOtherSceneButtons()
    {
        disconnectButton = GameObject.Find("Disconnect").GetComponent<Button>();
        disconnectButton.onClick.RemoveAllListeners();
        disconnectButton.onClick.AddListener(NetworkManager.singleton.StopHost);

        capacityText = GameObject.Find("Capacity").GetComponent<Text>();

        connInfoText = GameObject.Find("ConnectionInfo").GetComponent<Text>();
    }

    void ShowDirectMenu()
    {
        GameObject.Find("ServerButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ServerButton").GetComponent<Button>().onClick.AddListener(ServerDirect);

        GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(HostDirect);

        GameObject.Find("JoinButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("JoinButton").GetComponent<Button>().onClick.AddListener(JoinDirect);
    }
}