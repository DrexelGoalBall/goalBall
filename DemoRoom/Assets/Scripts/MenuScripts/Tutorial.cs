using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the tutorial menu
public class Tutorial : MonoBehaviour {

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
		// Undefined
	}

	void Right ()
	{
		Application.LoadLevel("MainMenu");
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
