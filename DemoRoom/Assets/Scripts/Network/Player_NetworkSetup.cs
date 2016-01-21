using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour 
{
    /// <summary>
    ///     When a player joins a game, set up the necessary components for the local player
    /// </summary>

    // Camera for this player
	[SerializeField] Camera playerCamera;
    // AudioListener for this player
	[SerializeField] AudioListener audioListener;

	// Use this for initialization
	public override void OnStartLocalPlayer ()
	{
        // Retrieve the movement script for this player and enable it
        GoalBallPlayerMovementV1 gbpm = GetComponent<GoalBallPlayerMovementV1>();
        gbpm.enabled = true;
        // Update the spawn position to the selected team and position
        SetSpawn(gbpm);
        // Enable the camera and audiolistener
        playerCamera.enabled = true;
		audioListener.enabled = true;
	}

    // Moves this player to the appropriate spawn based on its selected team and position
    private void SetSpawn(GoalBallPlayerMovementV1 gbpm)
    {
        // Get the NetworkManager
        NetworkManager_Custom networkManager = GameObject.Find("NetworkManager_Custom").GetComponent<NetworkManager_Custom>();
        
        // Location to spawn the player
        Transform spawnLoc = gbpm.transform;
        foreach (Transform startPosition in networkManager.startPositions)
        {
            // Check if this start position corresponds with the selected team and position
            string spName = startPosition.name.ToLower();
            if (spName.Contains(networkManager.team.ToLower()) && spName.Contains(networkManager.position.ToLower()))
            {
                // This is the position we want
                spawnLoc = startPosition;
                break;
            }
        }

        // Rotation at which to spawn player and correction  (defaults to rotation for blue team)
        Quaternion spawnRot = Quaternion.Euler(new Vector3(0, 90, 0));
        if (spawnLoc.name.ToLower().StartsWith("red"))
        {
            // Turn around if on the red team
            spawnRot = Quaternion.Euler(new Vector3(0, 270, 0));
        }

        // Move the player to the correct position and rotation
        gbpm.transform.position = spawnLoc.position;
        gbpm.transform.rotation = spawnRot;
    }
}
