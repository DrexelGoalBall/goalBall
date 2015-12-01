using UnityEngine;
using System.Collections;

public class CoinFlip : ScriptableObject {

    public float odds = .5f;
    public int expected = 1;
    public int actual = 0;
	
    // 0 = heads
    // 1 = tails

    public bool Flip()
    {
        actual = Random.Range(0, 2);
        if (actual == expected)
        {
            return true;
        }
        return false;
    }

    public void setExpected(int choice)
    {
        if (choice == 0 || choice == 1)
        {
            expected = choice;
        }
    }

}
