using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTest;

[IntegrationTest.DynamicTestAttribute("GameSceneIntegrationTests")]
[IntegrationTest.SucceedWithAssertions]
public class ScoreCounterTest : MonoBehaviour {

	public ScoreKeeper sk;

	// Use this for initialization
	void Awake () {
		IAssertionComponentConfigurator configurator;

		sk.ScoreBluePoint();

		checkScore("blue", 1);

		sk.ScoreRedPoint();

		checkScore("red", 1);

		sk.RemoveBluePoint();

		checkScore("blue", 0);

		sk.RemoveRedPoint();

		checkScore("red", 0);

		IntegrationTest.Pass();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void checkScore(string team, int value)
	{
		int score = 0;
		if (team == "red")
		{
			score = sk.RedScore();
		}
		else if (team == "blue")
		{
			score = sk.BlueScore();
		}

		if (score != value)
		{
			Debug.Log ("Team: " + team + " Expected Score: " + value + " Received Score: " + score);
			throw new WrongScoreException();
		}

		Debug.Log("Pass");
	}

	IEnumerator WaitForReset(string team, int value)
	{
		yield return new WaitForSeconds(1);

		checkScore(team, value);
	}

// Exceptions

	private class WrongScoreException : Exception
	{

	}
}
