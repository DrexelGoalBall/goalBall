﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Latency : NetworkBehaviour {

	private NetworkClient nClient;
	private int latency;
	private Text latencyText;

	public override void OnStartLocalPlayer ()
	{
		nClient = GameObject.Find("NetworkManager_Custom").GetComponent<NetworkManager>().client;
		latencyText = GameObject.Find("LatencyText").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () 
	{
		ShowLatency();
	}

	void ShowLatency ()
	{
		if(isLocalPlayer)
		{
			latency = nClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}
}
