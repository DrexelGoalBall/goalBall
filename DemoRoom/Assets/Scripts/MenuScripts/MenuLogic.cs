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
		private float horiz = 0;
		private float vert = 0;

		private bool hasSounded = false; // Prevents repeating sound on button press

		private float LIMIT = 0.9f; // How many frames button must be held to run function

		public string HorizontalButton = "Move Horizontal";
		public string VerticalButton = "Move Vertical";

		public string SubmitButton = "Submit";

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

		// Highlight bumpers when a direction is selected
		private Transform bumpers;
		private Transform leftBumper;
		private Transform rightBumper;
		private Transform upBumper;
		private Transform downBumper;

        //Timer until we explain the Menu;
        private float TimeLimit = 10;
        private float timer = 0;
        public bool shouldPlay = true;
        public AudioClip Instructions;

		/// <summary>
		///     Get the AudioSource for the menu when the scene starts
		/// </summary>
		void Start() 
		{
			source = GetComponent<AudioSource>();
			Transform canvas = GameObject.Find("MenuCanvas").transform;
			bumpers = canvas.Find("Bumpers");

			if (bumpers)
			{
				leftBumper = bumpers.Find("LeftBumper");
				rightBumper = bumpers.Find("RightBumper");	
				upBumper = bumpers.Find("UpBumper");
				downBumper = bumpers.Find("DownBumper");				
			}

		}
	
		/// <summary>
		///     Every frame, check for user input on menu
		/// </summary>
		void Update() 
		{
            directionalMenuLogic(Left, Right, Up, Down);
		}

        protected void PlayInstructionsAfterDelay()
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            if (timer > TimeLimit && Instructions && shouldPlay)
            {
                source.clip = Instructions;
                source.Play();
                TimeLimit = 20;
                shouldPlay = false;
            }
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
            horiz = InputPlayers.player0.GetAxis(HorizontalButton);
            vert = InputPlayers.player0.GetAxis(VerticalButton);

            if (horiz < LIMIT && vert < LIMIT)
            {
                changeBumpers("off");
            }

			// Audio for each option if user holds down key for long enough
		//	if (!hasSounded) // To be used if the welcome instructions should be skippable if the user presses a button
			 if (horiz >= LIMIT) // Right
			 {
                if (!source.isPlaying) playDirectionalSound(rightSound);
                changeBumpers("Right");
			 }	
			 else if (horiz <= -(LIMIT) ) // Left
			 {
                if (!source.isPlaying) playDirectionalSound(leftSound);
                changeBumpers("Left");
            }	
           
			 if (vert >= LIMIT) // Up
			 {
                if (!source.isPlaying) playDirectionalSound(upSound);
                changeBumpers("Up");
            }
			 else if (vert <= -(LIMIT) ) // Down
			 {
                if (!source.isPlaying) playDirectionalSound(downSound);
                changeBumpers("Down");
            }

            if (InputPlayers.player0.GetButtonDown(SubmitButton))
            {
                if (horiz > LIMIT) // Right
                {
                    right();
                }
                else if (horiz < -1*LIMIT) // Left
                {
                    left();
                }

                if (vert > LIMIT) // Up
                {
                    up();
                }
                else if (vert < -1*LIMIT) // Down
                {
                    down();
                }
            }

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

		private void changeBumpers(string dir)
		{
			if (bumpers)
			{
				foreach (Transform child in bumpers)
				{
					if (child.name.Contains(dir))
						child.gameObject.SetActive(true);
					else
						child.gameObject.SetActive(false);
				}			
			}

		}

	}
}

