using UnityEngine;
using System.Collections;

// Static class with tools used for menu navigation and use
namespace MenuTools
{
    /// <summary>
    ///     Receives user input on menus, plays audio clips and calls appropriate functions based on selections
    /// </summary>
    public class MenuLogic : MonoBehaviour 
    {
		// Keypress trackers
		private int horiz = 0;
		private int vert = 0;

		private bool hasSounded = false; // Prevents repeating sound on button press

		private int HOLDLIMIT = 30; // How many frames button must be held to run function

		public string HorizontalButton = "Horizontal";
		public string VerticalButton = "Vertical";

		public string SubmitButton = "Submit"; // Keyboard
		public string SubmitButtonAlt = "CatchP1"; // Controller

		public string CancelButton; // For Later Use?

		// Audio clips for directions
		public AudioClip leftSound;
		public AudioClip rightSound;
		public AudioClip upSound;
		public AudioClip downSound;

		// Allows derived classes to call directionalMenuLogic with their own functions
		public delegate void LeftFunc();
		public delegate void RightFunc();
		public delegate void UpFunc();
		public delegate void DownFunc();

		private AudioSource source;

        /// <summary>
        ///     Get the AudioSource for the menu when the scene starts
        /// </summary>
		void Start() 
        {
			source = GetComponent<AudioSource>();
		}
	
		/// <summary>
		///     Every frame, check for user input on menu
		/// </summary>
		void Update() 
        {
			directionalMenuLogic(Left, Right, Up, Down);
		}

// Protected Functions

        /// <summary>
        ///     Checks for user button presses and calls respective function
        /// </summary>
        /// <param name="left">Defined Left function</param>
        /// <param name="right">Defined Right function</param>
        /// <param name="up">Defined Up function</param>
        /// <param name="down">Defined Down function</param>
		protected void directionalMenuLogic(LeftFunc left, RightFunc right, UpFunc up, DownFunc down)
		{
			if (Input.GetButtonUp(HorizontalButton))
			{
				horiz = 0;
				hasSounded = false;
			}
			if (Input.GetButtonUp(VerticalButton))
			{
				vert = 0;
				hasSounded = false;
			}

			// Audio for each option if user holds down key for long enough
		//	if (!hasSounded) // To be used if the welcome instructions should be skippable if the user presses a button
			if (!source.isPlaying && !hasSounded) // To be used if the welcome instructions are supposed to be unskippable
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

			if (Input.GetButtonDown(SubmitButton) || Input.GetButtonDown(SubmitButtonAlt))
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

	// These instances should only ever be called if a menu is missing functions

        /// <summary>
        ///     Left menu option is selected (only called if menu is missing Left function)
        /// </summary>
        protected void Left()
		{
			print ("UNDEFINEDLEFT");
		}

        /// <summary>
        ///     Right menu option is selected (only called if menu is missing Right function)
        /// </summary>
        protected void Right()
		{
			print ("UNDEFINEDRIGHT");
		}

        /// <summary>
        ///     Up menu option is selected (only called if menu is missing Up function)
        /// </summary>
		protected void Up()
		{
			print ("UNDEFINEDUP");
		}

        /// <summary>
        ///     Down menu option is selected (only called if menu is missing Down function)
        /// </summary>
		protected void Down()
		{
			print ("UNDEFINEDDOWN");
		}

// Private Functions

        /// <summary>
        ///     Plays clip when direction is pressed
        /// </summary>
        /// <param name="clip">Clip to play</param>
		private void playDirectionalSound(AudioClip clip)
		{
			source.Stop();
			source.clip = clip;
			source.Play();
			hasSounded = true;
		}

	}
}

