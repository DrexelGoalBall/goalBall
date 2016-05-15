using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using SharpConnect;
using System.Security.Permissions;

/// <summary>
///     Provides a link between the Unity game and the Goalball Game Manager (GGM) application
/// </summary>
public class ServerLink : MonoBehaviour
{
    // Reference to NetworkManager_Custom script
    public NetworkManager_Custom networkManager;

    // Connector that allows this class to communicate with the GGM
    public Connector connector = new Connector();

    // The IP to look for the GGM
    public string gameManagerIP = "localhost";

    // The most recent message received from the server
    string lastMessage;

    /// <summary>
    ///     Log any messages received from server and handle special logic for commands received
    /// </summary>
    void Update()
    {
        if (connector.message != Connector.RCV_JOIN_GGM)
        {
            // Check if the response has changed
            if (connector.res != lastMessage)
            {
                Debug.Log(connector.res);
                lastMessage = connector.res;

                // Check if command received is port
                if (connector.message.StartsWith(Connector.RCV_PORT))
                {
                    // Determine if a port was sent
                    int portNum;
                    if (int.TryParse(connector.res, out portNum))
                    {
                        // Try to join the game on the given port
                        Debug.Log("IP: " + gameManagerIP + " Port: " + portNum);
                        networkManager.JoinDirect(gameManagerIP, portNum);
                    }
                }
            }
        }
    }

    /// <summary>
    ///     When the game is closed, let the GGM know you are leaving
    /// </summary>
    void OnApplicationQuit()
    {
        try
        {
            connector.Disconnect();
        }
        catch { }
    }

    /// <summary>
    ///     Attempts to join the GGM with the given IP
    /// </summary>
    public void ConnectToServer()
    {
        Debug.Log("Connected = " + connector.Connect(gameManagerIP, System.Environment.MachineName));
        if (connector.res != "")
        {
            Debug.Log(connector.res);
        }
    }
}