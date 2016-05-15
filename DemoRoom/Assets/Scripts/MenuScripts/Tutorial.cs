using UnityEngine;
using System.Collections;
using MenuTools;

/// <summary>
///     Sets up the Tutorial menu for user interaction
/// </summary>
// Used to navigate and manage the tutorial menu
public class Tutorial : MonoBehaviour 
{
	public string LeaveButton = "Cancel";
	public string AltLeaveButton = "Start";

	public AudioClip[] tutorialQueue;

	private AudioSource source;
	private int queueIndex = 0;

	void Start()
	{
		source = GetComponent<AudioSource>();
		source.clip = tutorialQueue[queueIndex];
		source.Play();
	}

	/// <summary>
	///     Updates the menu logic with the functions for this menu
	/// </summary>
	void Update() 
	{
		if (InputPlayers.player0.GetButtonDown(LeaveButton) || InputPlayers.player0.GetButtonDown(AltLeaveButton))
		{
			Application.LoadLevel("MainMenu");			
		}

		if (!source.isPlaying) // Queue up the next clip
		{
			queueIndex++;

			if (queueIndex <= tutorialQueue.Length)
			{
				source.clip = tutorialQueue[queueIndex];
				source.Play();
			}
			else
			{
				Application.LoadLevel("MainMenu");	
			}
		}
	}

}
