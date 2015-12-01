using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	public void PlayAgain(){
		if(GameObject.Find("WinnerCarryOver") != null)
			Destroy(GameObject.Find("WinnerCarryOver"));
		Application.LoadLevel("DetailedGame");
	}

	public void Menu(){
		if(GameObject.Find("WinnerCarryOver") != null)
			Destroy(GameObject.Find("WinnerCarryOver"));
		Application.LoadLevel("MainMenu");
	}
}
