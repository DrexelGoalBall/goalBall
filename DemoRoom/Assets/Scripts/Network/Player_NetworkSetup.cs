using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
///     When a player joins a game, set up the necessary components for the local player
/// </summary>
public class Player_NetworkSetup : NetworkBehaviour 
{
    // Camera for this player
	[SerializeField] private Camera playerCamera;
    // AudioListener for this player
	[SerializeField] private AudioListener audioListener;

    /// <summary>
    ///     When the local player object is set up, enable the necessary components and set its spawn position
    /// </summary>
	public override void OnStartLocalPlayer()
	{
        // Retrieve the various components necessary for this player
        GoalBallPlayerMovementV1 gbpm = GetComponent<GoalBallPlayerMovementV1>();
        PlayerInputController controller = GetComponent<PlayerInputController>();
        DebugSwapLayer DSL = GetComponent<DebugSwapLayer>();
        PlayerRumbleController ROC = GetComponent<PlayerRumbleController>();

        // Enable all components for this client
        controller.enabled = true;
        playerCamera.enabled = true;
        ROC.enabled = true;
        audioListener.enabled = true;
        DSL.enabled = true;

        // Relocate this player to there designated position
        SetSpawn(gbpm);

        // Make sure player is on the layer affected by boundaries
        gameObject.layer = LayerMask.NameToLayer("Player");
	}

    /// <summary>
    ///     Moves this player to the appropriate spawn based on its selected team and position
    /// </summary>
    /// <param name="gbpm">Movement object to change the player's position</param>
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
                gbpm.currentPosition = startPosition.position;
                break;
            }
        }

        // Tag for the current team
        string teamTag = "BluePlayer";

        // Rotation at which to spawn player and correction (defaults to rotation for blue team)
        Quaternion spawnRot = Quaternion.Euler(new Vector3(0, 90, 0));
        if (spawnLoc.name.ToLower().StartsWith("red"))
        {
            // Turn around if on the red team
            spawnRot = Quaternion.Euler(new Vector3(0, 270, 0));

            teamTag = "RedPlayer";
        }

        // Depending on the team, set the proper tag for this player
        GetComponent<Player_ID>().SetTeamTag(teamTag);

        // Move the player to the correct position and rotation
        gbpm.transform.position = spawnLoc.position;
        gbpm.transform.rotation = spawnRot;
    }
}
