using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTest;

[IntegrationTest.DynamicTestAttribute("GameSceneIntegrationTests")]
[IntegrationTest.SucceedWithAssertions]
public class BallResetTest : MonoBehaviour {

	public BallReset bs;

	private GameObject ball;

	void Awake()
	{
		IAssertionComponentConfigurator configurator;
	}

	void Start()
	{
	// Does ball exist?
		ball = bs.GetBall();

//		var gc = AssertionComponent.Create<GeneralComparer>(CheckMethod.Update, ball, "BallResetTest.ball", null);
//		gc.compareType = GeneralComparer.CompareType.ANotEqualsB;

	// Does ball spawn correctly?
		bs.placeBallRSC(); // Red Side Center
		WaitForReset(Possession.Team.red, "RSC");

		bs.placeBallBSC(); // Blue Side Center
		WaitForReset(Possession.Team.blue, "BSC");

		bs.placeBallRSL(); // Red Side Left
		WaitForReset(Possession.Team.red, "RSL");

		bs.placeBallBSL(); // Blue Side Left
		WaitForReset(Possession.Team.blue, "BSL");

		bs.placeBallRSR(); // Red Side Right
		WaitForReset(Possession.Team.red, "RSR");

		bs.placeBallBSR(); // Blue Side Right
		WaitForReset(Possession.Team.blue, "BSR");

	}

	private void checkTeam(Possession.Team t, string pos)
	{
/*
	// Having problems getting this assertion to work. In the meantime, going to just throw exceptions otherwise.
		var tc = AssertionComponent.Create<IntComparer>(CheckMethod.Update, ball,"BallResetTest.ball.GetComponent<Possession>().HasPossessionOfBall()", t);
		tc.compareType = IntComparer.CompareType.Equal;
*/
		if (ball.GetComponent<Possession>().HasPossessionOfBall() != t)
		{
			Debug.Log (pos);
			throw new WrongTeamException();
		}		
	}

	IEnumerator WaitForReset(Possession.Team t, string pos)
	{
		yield return new WaitForSeconds(2);

		checkTeam(t, pos);
	}

// Exceptions

	private class WrongTeamException : Exception
	{

	}
}
