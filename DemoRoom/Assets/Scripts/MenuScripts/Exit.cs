using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the networking menu
public class Exit : MonoBehaviour {

	public AudioClip LeftSound;
	public AudioClip RightSound;
	public AudioClip UpSound;
	public AudioClip DownSound;
	public AudioClip MenuSound;

	private AudioSource source;

	void Start () {
		source = GetComponent<AudioSource>();
		MenuLogic.setAudioSource(source);

		MenuLogic.initialAudio(MenuSound);
	}
	
	// Update is called once per frame
	void Update () {
		MenuLogic.directionalMenuLogic(Left, Right, Up, Down, RightSound, LeftSound, UpSound, DownSound);
	}

	void Left ()
	{
		Application.LoadLevel("MainMenu");
	}

	void Right ()
	{
		Application.Quit();

	}

	void Up ()
	{
		// Undefined
	}

	void Down ()
	{
		// Undefined
	}
}
