using UnityEngine;
using System.Collections;

/// <summary>
///     Sets up any aspects of the game and loads the correct scene based on the instance loaded
/// </summary>
public class Initialization : MonoBehaviour 
{
    // Scene to load when a server instance of the game is run
    public string serverScene;

    // Scene to load when a client instance of the game is run
    public string clientScene;

    // For server instance, object to provide the port number to pass to the network manager
    public GameObject serverSetupInfoPrefab;

	/// <summary>
	///     Check which scene should be loaded
	/// </summary>
	void Start() 
    {
        // Determine if a port argument was provide, signify a server instance
        string portCheck = "+port:";
        string[] args = System.Environment.GetCommandLineArgs();
        foreach (string arg in args)
        {
            if (arg.Contains(portCheck))
            {
                // Port argument found, try to retrieve the provided port number
                string strPortNum = arg.Substring(arg.IndexOf(portCheck) + portCheck.Length);
                int portNum;
                if (int.TryParse(strPortNum, out portNum))
                {
                    // Instantiate temporary object to pass port number to network manager
                    GameObject serverInfo = GameObject.Instantiate(serverSetupInfoPrefab);
                    serverInfo.GetComponent<ServerSetupInfo>().portNumber = portNum;
                    // Load the scene for the server
                    Application.LoadLevel(serverScene);
                }
            }
        }

        // Otherwise, load the scene for the client
        Application.LoadLevel(clientScene);
	}
}
