using UnityEngine;
using System.Collections;

/// <summary>
///     In server instance of game. temporary script to carry port number to create networked game on
/// </summary>
public class ServerSetupInfo : MonoBehaviour 
{
    // Port number for the game
    public int portNumber = -1;

	/// <summary>
	///     Prevent the object containing this script from being destroyed on load, only get rid of 
    ///     this script when its information is used
	/// </summary>
	void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
	}
}
