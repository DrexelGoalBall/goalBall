using UnityEngine;
using System.Collections;

/// <summary>
/// Script that simulates a coinflip.
/// </summary>
public class CoinFlip : ScriptableObject {
    

    public float odds = .5f;
    private int expected = 1;
    private int actual = 0;
	
   
    /// <summary>
    /// Activates the flip and returns true or false depending on if the expected choice was chosen.
    /// </summary>
    /// <returns></returns>
    public bool Flip()
    {
        actual = Random.Range(0, 2);
        if (actual == expected)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Getter for expected.
    /// </summary>
    /// <returns>The value of expected.</returns>
    public int getExpected()
    {
        return expected;
    }

    /// <summary>
    /// Sets the expected value to the choice argument.  Should be 1 or 0.
    /// </summary>
    /// <param name="choice"></param>
    public void setExpected(int choice)
    {
        if (choice == 0 || choice == 1)
        {
            expected = choice;
        }
    }

}
