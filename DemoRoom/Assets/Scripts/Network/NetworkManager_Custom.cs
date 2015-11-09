using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager
{
    // Main Menu Components
    private Button localButton, onlineButton;
    private GameObject localPanel, onlinePanel;
    private bool local = false, online = false, joining = false;
    private GameObject roomsPanel;
    public Button roomsListButton;
    private Text networkInfoText;

    // Game Components
    private Button disconnectButton;

    void Start()
    {
        OnLevelWasLoaded(Application.loadedLevel);

        NetworkManager.singleton.matchSize = 6;
    }

    public void HostLocal()
    {
        networkInfoText.text = "Creating local match...";
        NetworkManager.singleton.networkAddress = "localhost";
        NetworkManager.singleton.networkPort = 7777;
        NetworkManager.singleton.StartHost();
    }

    public void JoinLocal()
    {
        if (!joining)
        {
            joining = true;
            networkInfoText.text = "Joining local match...";
            NetworkManager.singleton.networkAddress = "localhost";
            NetworkManager.singleton.networkPort = 7777;
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
        foreach (Transform t in this.startPositions)
            Debug.Log(t.name);
        Quaternion spawnRot = Quaternion.identity;
        int sideCorrection = 1;
        if (spawnLoc.name.StartsWith("Red"))
        {
            spawnRot = Quaternion.Euler(new Vector3(0, 180, 0));
            sideCorrection = -1;
        }
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, spawnLoc.position, spawnRot);
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
        joining = false;
        Debug.Log("Client Connect");
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if (joining)
        {
            networkInfoText.text = "Failed to join.";
            Debug.Log(conn.address + " - " + conn.hostId);
            if (conn.address.Trim().Equals("127.0.0.1"))
                HostLocal();
        }
        joining = false;
        Debug.Log("Client Disconnect");
    }

    /*public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        joining = false;
        Debug.Log("Client Error");
    }*/

    public override void OnServerConnect(NetworkConnection conn)
    {
        joining = false;
        Debug.Log("Server Connect");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        joining = false;
        Debug.Log("Server Disconnect");
    }
    
    /*public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        joining = false;
        Debug.Log("Server Error");
    }*/

    public override void OnStopClient()
    {
        joining = false;
        Debug.Log("Stop Client");
    }

    public override void OnStopHost()
    {
        joining = false;
        Debug.Log("Stop Host");
    }

    public override void OnStopServer()
    {
        joining = false;
        Debug.Log("Stop Server");
    }

    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.3f);

        networkInfoText = GameObject.Find("NetworkInfoText").GetComponent<Text>();

        local = false;
        online = false;

        localButton = GameObject.Find("Local").GetComponent<Button>();
        localButton.onClick.RemoveAllListeners();
        localButton.onClick.AddListener(ShowLocalMenu);

        onlineButton = GameObject.Find("Online").GetComponent<Button>();
        onlineButton.onClick.RemoveAllListeners();
        onlineButton.onClick.AddListener(ShowOnlineMenu);

        localPanel = GameObject.Find("LocalPanel");
        localPanel.SetActive(false);
        onlinePanel = GameObject.Find("OnlinePanel");
        onlinePanel.SetActive(false);
    }

    void SetupOtherSceneButtons()
    {
        disconnectButton = GameObject.Find("Disconnect").GetComponent<Button>();
        disconnectButton.onClick.RemoveAllListeners();
        disconnectButton.onClick.AddListener(NetworkManager.singleton.StopHost);
    }

    void ShowLocalMenu()
    {
        if (local)
        {
            local = false;
            localPanel.SetActive(false);
        }
        else
        {
            local = true;
            localPanel.SetActive(true);

            if (online)
            {
                online = false;
                onlinePanel.SetActive(false);
                StopMatchMaker();
            }

            GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(HostLocal);

            GameObject.Find("JoinButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("JoinButton").GetComponent<Button>().onClick.AddListener(JoinLocal);
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

            if (local)
            {
                local = false;
                localPanel.SetActive(false);
            }

            GameObject.Find("CreateButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("CreateButton").GetComponent<Button>().onClick.AddListener(CreateOnlineRoom);

            GameObject.Find("FindButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("FindButton").GetComponent<Button>().onClick.AddListener(ListOnlineRooms);

            roomsPanel = GameObject.Find("RoomsPanel");
        }
    }
}