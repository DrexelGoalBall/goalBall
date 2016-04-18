using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
///     Enables users to select what team and position they would like to play
/// </summary>
public class TeamPositionSelection : MonoBehaviour 
{
    // Reference to Network Manager Custom script
    private NetworkManager_Custom networkManager;

    // Buttons to allow user to switch teams and positions
    public List<Button> teamButtons = new List<Button>();
    public List<Button> positionButtons = new List<Button>();

    // Colors associated with the teams
    public Color redTeam, blueTeam;
    // Default color of buttons
    public Color unselected;

    // Currently selected team and position index
    private int teamIndex = -1, positionIndex = -1;

    // Flag set when all controls are initialized
    private bool initialized = false;

    /// <summary>
    ///     When the script begins, find the necessary components and set initial values
    /// </summary>
	void Start () 
    {
        networkManager = GameObject.Find("NetworkManager_Custom").GetComponent<NetworkManager_Custom>();

        // Modify the variables and UI for the selected team and position
        ChangeTeamSelection(networkManager.teamIndex);
        ChangePositionSelection(networkManager.positionIndex);

        // Initialization complete
        initialized = true;
	}

    /// <summary>
    ///     Updates the selected team index and string
    /// </summary>
    /// <param name="index">The index of the team selected</param>
    public void ChangeTeamSelection(int index)
    {
        // Check if there was actually a change in the selected team
        if (teamIndex == index)
            return;
        else
            teamIndex = index;

        // Update the underlying variables to select the team
        networkManager.teamIndex = teamIndex;
        networkManager.team = teamButtons[teamIndex].GetComponentInChildren<Text>().text;

        // Update the UI according to the selection
        for (int i = 0; i < teamButtons.Count; i++)
        {
            Color clr;

            if (i == index)
            {
                // Determine the selected team
                if (teamButtons[i].name.ToLower().Contains("red"))
                    clr = redTeam;
                else
                    clr = blueTeam;

                // Do not update the corresponding position button if it has not been initialized yet
                if (initialized)
                    positionButtons[positionIndex].GetComponent<Image>().color = clr;
            }
            else
            {
                clr = unselected;
            }

            teamButtons[i].GetComponent<Image>().color = clr;
        }
    }

    /// <summary>
    ///     Updates the selected position index and string
    /// </summary>
    /// <param name="index">The index of the position selected</param>
    public void ChangePositionSelection(int index)
    {
        // Check if there was actually a change in the selected position
        if (positionIndex == index)
            return;
        else
            positionIndex = index;

        // Update the underlying variables to select the position
        networkManager.positionIndex = positionIndex;
        networkManager.position = positionButtons[positionIndex].GetComponentInChildren<Text>().text;

        // Update the UI according to the selection
        for (int i = 0; i < positionButtons.Count; i++)
        {
            Color clr;

            if (i == index)
            {
                // Determine the selected team
                if (teamButtons[teamIndex].name.ToLower().Contains("red"))
                    clr = redTeam;
                else
                    clr = blueTeam;
            }
            else
            {
                clr = unselected;
            }

            positionButtons[i].GetComponent<Image>().color = clr;
        }
    }
}
