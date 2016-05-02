using UnityEngine;
using System.Collections;
using Rewired;

/// <summary>
///     Rewired player(s) where all inputs should be checked from
/// </summary>
public class InputPlayers : MonoBehaviour 
{
    // The Rewired Player
    public static Player player0;

	/// <summary>
	///     Initializes the necessary player objects
	/// </summary>
	void Start()
    {
        player0 = ReInput.players.GetPlayer(0);
	}
}
