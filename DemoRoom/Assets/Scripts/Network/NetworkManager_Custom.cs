using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager
{
    // Main Menu Components
    private Button directButton, onlineButton;
    private GameObject directPanel, onlinePanel;
    private bool direct = false, online = false, joining = false;
    private GameObject roomsPanel;
    public Button roomsListButton;
    private Text networkInfoText;

    // Game Components
    private Button disconnectButton;
    private Text capacityText;
    public Text playerElementText;
    private Text connInfoText;

    //int playerCount = 0;

    void Start()
    {
        OnLevelWasLoaded(Application.loadedLevel);
        NetworkManager.singleton.maxConnections = 6;
        NetworkManager.singleton.matchSize = 6;
    }

    void Update()
    {
        if (NetworkManager.singleton.isNetworkActive)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (capacityText != null)
            {
                //capacityText.text = GetPlayerCount();
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

            /*if (players.Length != playerCount)
            {
                GameObject playerList = GameObject.Find("PlayerList");

                var children = new List<GameObject>();
                foreach (Transform child in playerList.transform) children.Add(child.gameObject);
                children.ForEach(child => Destroy(child));

                for (int i = 0; i < players.Length; i++)
                {
                    Player_ID id = players[i].GetComponent<Player_ID>();
                    string pinfo = string.Format("Player {0}", id.netId.Value);
                    if (id.isLocalPlayer)
                        pinfo = string.Format("*** {0} ***", pinfo);
                    Text txt = Instantiate(playerElementText);
                    txt.text = pinfo;
                    txt.transform.SetParent(playerList.transform);
                }

                playerCount = players.Length;
            }*/

            /*if (NetworkServer.connections != null && NetworkServer.connections.Count > players)
            {
                GameObject playerList = GameObject.Find("PlayerList");

                var children = new List<GameObject>();
                foreach (Transform child in playerList.transform) children.Add(child.gameObject);
                children.ForEach(child => Destroy(child));

                foreach (var connection in NetworkServer.connections)
                {
                    if (connection != null)
                    {
                        Text txt = Instantiate(playerElementText);
                        Debug.Log(connection.address);
                        txt.text = connection.address;
                        txt.transform.SetParent(playerList.transform);
                    }
                }

                players = NetworkServer.connections.Count;
            }*/
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

    public void CreateOnlineRoom()
    {
        if (NetworkManager.singleton.matchInfo == null)
        {
            string roomName = GameObject.Find("RoomNameField").GetComponent<InputField>().text;
            if (string.IsNullOrEmpty(roomName))
                roomName = "default";
            networkInfoText.text = string.Format("Creating match '{0}'...", roomName);
            NetworkManager.singleton.matchName = roomName;
            NetworkManager.singleton.matchMaker.CreateMatch(NetworkManager.singleton.matchName, NetworkManager.singleton.matchSize, true, "", NetworkManager.singleton.OnMatchCreate);
        }
    }

    public void JoinOnline(MatchDesc match)
    {
        if (!joining)
        {
            joining = true;
            networkInfoText.text = string.Format("Joining match '{0}'...", match.name);
            NetworkManager.singleton.matchName = match.name;
            NetworkManager.singleton.matchSize = (uint)match.currentSize;
            NetworkManager.singleton.matchMaker.JoinMatch(match.networkId, "", NetworkManager.singleton.OnMatchJoined);
        }
    }

    public void ListOnlineRooms()
    {
        if (NetworkManager.singleton.matchInfo == null)
        {
            networkInfoText.text = "Finding online matches...";
            NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", NetworkManager.singleton.OnMatchList);
        }
        else
        {
            roomsPanel.SetActive(false);
        }
    }

    public override void OnMatchList(ListMatchResponse matchListResponse)
    {
        GameObject roomsList = GameObject.Find("RoomsListElements");
        if (roomsList != null)
        {
            foreach (Transform child in roomsList.transform)
                Destroy(child.gameObject);
        }

        NetworkManager.singleton.matches = matchListResponse.matches;

        if (NetworkManager.singleton.matches != null && NetworkManager.singleton.matches.Count > 0)
        {
            roomsPanel.SetActive(true);

            int count = 0;

            foreach (var match in NetworkManager.singleton.matches)
            {
                if (match.currentSize < match.maxSize)
                {
                    count++;
                    Button btn = Instantiate(roomsListButton);
                    btn.GetComponentInChildren<Text>().text = string.Format("{0} ({1}/{2})", match.name, match.currentSize, match.maxSize);
                    btn.transform.SetParent(roomsList.transform);
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(delegate { JoinOnline(match); });
                }
            }

            if (count > 0)
                networkInfoText.text = string.Format("Found {0} match(es).", count);
            else
                networkInfoText.text = "No matches available to join.";
        }
        else
        {
            networkInfoText.text = "No matches available to join.";
            roomsPanel.SetActive(false);
        }
    }
    
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Add Player");
        Transform spawnLoc = this.startPositions[NetworkManager.singleton.numPlayers];
        //foreach (Transform t in this.startPositions)
        //    Debug.Log(t.name);
        Quaternion spawnRot = Quaternion.Euler(new Vector3(0, 90, 0)); ;
        int sideCorrection = 1;
        //Debug.Log(spawnLoc.name);
        if (spawnLoc.name.StartsWith("Red"))
        {
            spawnRot = Quaternion.Euler(new Vector3(0, 270, 0));
            sideCorrection = -1;
        }
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, spawnLoc.position, spawnRot);
        //Debug.Log(sideCorrection);
        player.GetComponent<GoalBallPlayerMovementV1>().sideCorrection = sideCorrection;
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

        direct = false;
        online = false;

        directButton = GameObject.Find("Direct").GetComponent<Button>();
        directButton.onClick.RemoveAllListeners();
        directButton.onClick.AddListener(ShowDirectMenu);

        onlineButton = GameObject.Find("Online").GetComponent<Button>();
        onlineButton.onClick.RemoveAllListeners();
        onlineButton.onClick.AddListener(ShowOnlineMenu);

        directPanel = GameObject.Find("DirectPanel");
        directPanel.SetActive(false);
        onlinePanel = GameObject.Find("OnlinePanel");
        onlinePanel.SetActive(false);
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
        if (direct)
        {
            direct = false;
            directPanel.SetActive(false);
        }
        else
        {
            direct = true;
            directPanel.SetActive(true);

            if (online)
            {
                online = false;
                onlinePanel.SetActive(false);
                StopMatchMaker();
            }

            GameObject.Find("ServerButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("ServerButton").GetComponent<Button>().onClick.AddListener(ServerDirect);

            GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(HostDirect);

            GameObject.Find("JoinButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("JoinButton").GetComponent<Button>().onClick.AddListener(JoinDirect);
        }
    }

    void ShowOnlineMenu()
    {
        if (online)
        {
            online = false;
            onlinePanel.SetActive(false);
            StopMatchMaker();
        }
        else
        {
            online = true;
            onlinePanel.SetActive(true);
            StartMatchMaker();

            if (direct)
            {
                direct = false;
                directPanel.SetActive(false);
            }

            GameObject.Find("CreateButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("CreateButton").GetComponent<Button>().onClick.AddListener(CreateOnlineRoom);

            GameObject.Find("FindButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("FindButton").GetComponent<Button>().onClick.AddListener(ListOnlineRooms);

            roomsPanel = GameObject.Find("RoomsPanel");
        }
    }

    public string GetPlayerCount()
    {
        return string.Format("{0}/{1}", NetworkManager.singleton.numPlayers, NetworkManager.singleton.maxConnections);
    }
}