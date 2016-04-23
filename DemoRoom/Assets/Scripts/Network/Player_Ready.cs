﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
///     Allows player to ready up to start the game
/// </summary>
public class Player_Ready : NetworkBehaviour
{
    // Flag to tell if this player is ready
    [SyncVar] private bool ready = false;
    
    // Button user should press to ready up
    public string readyButton = "Throw";

	/// <summary>
	///     Checks for users input to ready/unready
	/// </summary>
	void Update()
    {
        if (Input.GetButtonDown(readyButton))
        {
            CmdReadyUp(!ready);
        }
	}

    /// <summary>
    ///     Command sent to server to let player ready up
    /// </summary>
    /// <param name="rdy"></param>
    [Command]
    void CmdReadyUp(bool rdy)
    {
        ready = rdy;
    }

    /// <summary>
    ///     Whether or not this player is ready
    /// </summary>
    /// <returns>True if player is ready, False otherwise</returns>
    public bool isReady()
    {
        return ready;
    }
}