﻿using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the networking menu
public class Networking : MonoBehaviour {

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
		// Navigate to Quick Match Menu
	}

	void Right ()
	{
		// Navigate to Joining Menu
	}

	void Up ()
	{
		Application.LoadLevel("MainMenu");
	}

	void Down ()
	{
		// Navigate to Host Menu
	}
}
