using UnityEngine;
using System.Collections;

// Static class with tools used for menu navigation and use
namespace MenuTools
{
	public class MenuLogic : MonoBehaviour {

		// Keypress trackers
		private static int horiz = 0;
		private static int vert = 0;

		private static int HOLDLIMIT = 40; // How many frames button must be held to run function

		// Key Delegates
		public delegate void LeftFunc();
		public delegate void RightFunc();
		public delegate void UpFunc();
		public delegate void DownFunc();

		public static void directionalMenuLogic(LeftFunc left, RightFunc right, UpFunc up, DownFunc down)
		{

			if (horiz >= HOLDLIMIT) // Right
			{
				right();
				horiz = 0;
			}	
			else if (horiz <= -(HOLDLIMIT) ) // Left
			{
				left();
				horiz = 0;
			}	

			if (vert >= HOLDLIMIT) // Up
			{	
				up();
				vert = 0;
			}
			else if (vert <= -(HOLDLIMIT) ) // Down
			{
				down(); 
				vert = 0;
			}

			if (Input.GetAxis("Horizontal") > 0) // Right
				horiz++;
			else if (Input.GetAxis("Horizontal") < 0) // Left
				horiz--;
			else if (Input.GetAxis("Vertical") > 0) // Up
				vert++;
			else if (Input.GetAxis("Vertical") < 0) // Down
				vert--;
			else // Clear out buttons if let go
			{
				horiz = 0;
				vert = 0;
			}

		}


	}
}

