using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
///     Displays the latency for this client to the server
/// </summary>
public class Player_Latency : NetworkBehaviour 
{
    // 
	private NetworkClient nClient;
    // 
	private int latency;
	private Text latencyText;

    /// <summary>
    ///     When the local player object is set up, find the necessary components in the scene
    /// </summary>
	public override void OnStartLocalPlayer()
	{
		nClient = GameObject.Find("NetworkManager_Custom").GetComponent<NetworkManager>().client;
		latencyText = GameObject.Find("LatencyText").GetComponent<Text>();
	}

    /// <summary>
    ///     Every frame, show the latency
    /// </summary>
	void Update () 
	{
		ShowLatency();
	}

    /// <summary>
    ///     If it is the local player, determine and display the latency
    /// </summary>
	void ShowLatency()
	{
		if (isLocalPlayer)
		{
			latency = nClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}
}
