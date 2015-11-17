using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the main menu
public class MainMenu : MonoBehaviour {

	// Audio Clips for options
	// Still figuring out how we should do this - one way is it announcing the option you are currently on, the other is announcing the option you've selected before you actually go to it. I'm using the former one right now.
	public AudioClip LeftSound;
	public AudioClip RightSound;
	public AudioClip UpSound;
	public AudioClip DownSound;
	public AudioClip MenuSound;

	private AudioSource source;

	void Start () {
		source = GetComponent<AudioSource>();
		source.clip = MenuSound;
		source.Play();
	}
	
	// Update is called once per frame
	void Update () {
		MenuLogic.directionalMenuLogic(Left, Right, Up, Down);
	}


	void Left ()
	{
//		source.clip = LeftSound;
//		source.Play();
		Application.LoadLevel("Tutorial");
	}

	void Right ()
	{
//		source.clip = RightSound;
//		source.Play();
		// Ask User if they wish to exit application (currently just quits without asking)
		Application.Quit();
	}

	void Up ()
	{
//		source.clip = UpSound;
//		source.Play();
		Application.LoadLevel("Settings");
	}

	void Down ()
	{
//		source.clip = DownSound;
//		source.Play();
		Application.LoadLevel("Networking");
	}
}
