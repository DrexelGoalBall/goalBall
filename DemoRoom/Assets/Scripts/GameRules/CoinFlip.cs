using UnityEngine;
using System.Collections;

public class CoinFlip : ScriptableObject {
    /// <summary>
    /// Script that simulates a coinflip.
    /// </summary>

    public float odds = .5f;
    public int expected = 1;
    public int actual = 0;
	
   
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
