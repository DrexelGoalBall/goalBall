﻿using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the settings menu
public class Settings : MonoBehaviour {

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
		// Undefined
	}

	void Up ()
	{
		// Undefined
	}

	void Down ()
	{
		Application.LoadLevel("MainMenu");
	}
}
