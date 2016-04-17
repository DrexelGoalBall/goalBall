using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTest;

[IntegrationTest.DynamicTestAttribute("UIIntegrationTests")]
public class NavigationTest : MonoBehaviour {

	public MenuTools.MenuLogic menuLogicRef;


	private bool lC = false, rC = false, uC = false, dC = false;

	void Awake () {
		IAssertionComponentConfigurator configurator;
	
	}

	void Update()
	{
		if (!menuLogicRef.isSourcePlaying())
		{
			if (!lC)
				testLeft();
			else if (!rC)
				testRight();
			else if (!uC)
				testUp();
			else if (!dC)
				testDown();
		}

		IntegrationTest.Pass();
	}

	void testLeft()
	{
		menuLogicRef.PressLeft();	

		StartCoroutine(WaitForReset("horiz"));

		lC = true;
	}

	void testRight()
	{
		menuLogicRef.PressRight();

		StartCoroutine(WaitForReset("horiz"));

		rC = true;
	}

	void testUp()
	{
		menuLogicRef.PressUp();

		StartCoroutine(WaitForReset("vert"));

		uC = true;
	}

	void testDown()
	{
		menuLogicRef.PressDown();

		StartCoroutine(WaitForReset("vert"));

		dC = true;
	}


	IEnumerator WaitForReset(string val)
	{
		yield return new WaitForSeconds(5);

		int checkValue;
		if (val.Contains("horiz"))
		{
			checkValue = menuLogicRef.GetHoriz();
		}
		else
		{
			checkValue = menuLogicRef.GetVert();
		}

		if (checkValue != 0)
		{
			Debug.Log (checkValue);
			throw new ValueException();
		}

	}

// Exceptions

	private class ValueException : Exception
	{

	}
}
