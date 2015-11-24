using UnityEngine;
using System.Collections;

// Static class with tools used for menu navigation and use
namespace MenuTools
{
	public class MenuLogic : MonoBehaviour {

		// Keypress trackers
		private static int horiz = 0;
		private static int vert = 0;

		private static int HOLDLIMIT = 30; // How many frames button must be held to run function

		private static string HorizontalButton = "Horizontal";
		private static string VerticalButton = "Vertical";

		private static AudioSource source;

		// Key Delegates
		public delegate void LeftFunc();
		public delegate void RightFunc();
		public delegate void UpFunc();
		public delegate void DownFunc();

// Public Static Functions
		public static void directionalMenuLogic(LeftFunc left, RightFunc right, UpFunc up, DownFunc down, AudioClip rightSound, AudioClip leftSound, AudioClip upSound, AudioClip downSound)
		{
			if (Input.GetButtonUp(HorizontalButton))
			{
				horiz = 0;
			}
			if (Input.GetButtonUp(VerticalButton))
			{
				vert = 0;
			}

			// Audio for each option if user holds down key for long enough
			if (!source.isPlaying)
			{
				if (horiz >= HOLDLIMIT) // Right
				{
					playDirectionalSound(rightSound);
					horiz = 0;
				}	
				else if (horiz <= -(HOLDLIMIT) ) // Left
				{
					playDirectionalSound(leftSound);
					horiz = 0;
				}	

				if (vert >= HOLDLIMIT) // Up
				{	
					playDirectionalSound(upSound);
					vert = 0;
				}
				else if (vert <= -(HOLDLIMIT) ) // Down
				{
					playDirectionalSound(downSound);
					vert = 0;
				}
			}

			float horizonDir = Input.GetAxis(HorizontalButton);
			float verticDir = Input.GetAxis(VerticalButton);

			if (Input.GetButtonDown("Submit"))
			{
				if (horizonDir > 0) // Right
				{
					right();
				}	
				else if (horizonDir < 0 ) // Left
				{
					left();
				}	

				if (verticDir > 0) // Up
				{	
					up();
				}
				else if (verticDir < 0) // Down
				{
					down(); 
				}
			}

			if (horizonDir > 0) // Right
				horiz++;
			else if (horizonDir < 0) // Left
				horiz--;
			else if (verticDir > 0) // Up
				vert++;
			else if (verticDir < 0) // Down
				vert--;

		}

		public static void initialAudio(AudioClip initial)
		{
			source.clip = initial;
			source.Play();
		}

// Getters

// Setters
		public static void setAudioSource(AudioSource audSource)
		{
			source = audSource;
		}


// Private Functions

		private static void playDirectionalSound(AudioClip clip)
		{
			source.Stop();
			source.clip = clip;
			source.Play();
		}


	}
}

