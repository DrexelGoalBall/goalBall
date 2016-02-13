using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
///     Displays the winning team in the endgame scene
/// </summary>
public class RedOrBlue : MonoBehaviour 
{
    // 
	public Text blue;
	public Text red;

    // 
	GameObject determinate;
	
    /// <summary>
    ///     Destroys the object corresponding to the losing team
    /// </summary>
	void Start () 
    {
		determinate = GameObject.Find("WinnerCarryOver");

		if(determinate == null)
			Destroy(blue);
		else
        {
			if(determinate.GetComponent<ScoreCarry>().teamWinning == 0)
            {
				Destroy(blue);
			}
			else if(determinate.GetComponent<ScoreCarry>().teamWinning == 1)
            {
				Destroy(red);
			}
		}
	}
}
