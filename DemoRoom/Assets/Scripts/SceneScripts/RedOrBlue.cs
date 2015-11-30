using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RedOrBlue : MonoBehaviour {

	public Text blue;
	public Text red;
	GameObject determinate;
	// Use this for initialization
	void Start () {
		determinate = GameObject.Find("WinnerCarryOver");
		if(determinate == null)
			Destroy(blue);
		else{
			if(determinate.GetComponent<ScoreCarry>().teamWinning == 0){
				Destroy(blue);
			}
			else if(determinate.GetComponent<ScoreCarry>().teamWinning == 1){
				Destroy(red);
			}
		}
	}
	
}
