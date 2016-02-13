using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
///     Enables users to select what team and position they would like to play
/// </summary>
public class TeamPositionSelection : MonoBehaviour 
{
    // 
    NetworkManager_Custom networkManager;

    // 
    public List<Button> teamButtons = new List<Button>();
    public List<Button> positionButtons = new List<Button>();

    // 
    public Color redTeam, blueTeam, unselected;

    // 
    private int teamIndex = -1, positionIndex = -1;

    // 
    private bool initialized = false;

    /// <summary>
    ///     When the script begins, find the necessary components and set initial values
    /// </summary>
	void Start () 
    {
        networkManager = GameObject.Find("NetworkManager_Custom").GetComponent<NetworkManager_Custom>();

        // 
        ChangeTeamSelection(networkManager.teamIndex);
        ChangePositionSelection(networkManager.positionIndex);

        // 
        initialized = true;
	}

    /// <summary>
    ///     Updates the selected team index and string
    /// </summary>
    /// <param name="index">The index of the team selected</param>
    public void ChangeTeamSelection(int index)
    {
        // 
        if (teamIndex == index)
            return;
        else
            teamIndex = index;

        // 
        networkManager.teamIndex = teamIndex;
        networkManager.team = teamButtons[teamIndex].GetComponentInChildren<Text>().text;

        // 
        for (int i = 0; i < teamButtons.Count; i++)
        {
            Color clr;
            
            // 
            if (i == index)
            {
                // 
                if (teamButtons[i].name.ToLower().Contains("red"))
                    clr = redTeam;
                else
                    clr = blueTeam;

                // 
                if (initialized)
                    positionButtons[positionIndex].GetComponent<Image>().color = clr;
            }
            else
            {
                clr = unselected;
            }

            // 
            teamButtons[i].GetComponent<Image>().color = clr;
        }
    }

    /// <summary>
    ///     Updates the selected position index and string
    /// </summary>
    /// <param name="index">The index of the position selected</param>
    public void ChangePositionSelection(int index)
    {
        // 
        if (positionIndex == index)
            return;
        else
            positionIndex = index;

        // 
        networkManager.positionIndex = positionIndex;
        networkManager.position = positionButtons[positionIndex].GetComponentInChildren<Text>().text;

        // 
        for (int i = 0; i < positionButtons.Count; i++)
        {
            Color clr;

            // 
            if (i == index)
            {
                // 
                if (teamButtons[teamIndex].name.ToLower().Contains("red"))
                    clr = redTeam;
                else
                    clr = blueTeam;
            }
            else
            {
                clr = unselected;
            }

            // 
            positionButtons[i].GetComponent<Image>().color = clr;
        }
    }
}
