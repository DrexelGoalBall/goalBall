using UnityEngine;
using System.Collections;

public class DoOnKey : MonoBehaviour
{

    /// <summary>
    /// Debugging script that does a coinflip on a key press.
    /// </summary>

    CoinFlip flip = new CoinFlip();
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(flip.Flip());
        }

	}
}
