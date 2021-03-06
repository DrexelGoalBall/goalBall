﻿using UnityEngine;
using System.Collections;

/// <summary>
///     Loads the desired scene
/// </summary>
public class SceneChanger : MonoBehaviour 
{
    /// <summary>
    ///     Selected to play again, so reload game scene
    /// </summary>
	public void PlayAgain()
    {
		if(GameObject.Find("WinnerCarryOver") != null)
			Destroy(GameObject.Find("WinnerCarryOver"));
		Application.LoadLevel("DetailedGame");
	}

    /// <summary>
    ///     Selected to go back to Main menu, so load the Main menu scene
    /// </summary>
	public void Menu()
    {
		if(GameObject.Find("WinnerCarryOver") != null)
			Destroy(GameObject.Find("WinnerCarryOver"));
		Application.LoadLevel("MainMenu");
	}
}
