using UnityEngine;
using System.Collections;

/// <summary>
/// Debugging script that does a coinflip on a key press.
/// </summary>
public class DoOnKey : MonoBehaviour
{

    

    CoinFlip flip = new CoinFlip();
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(flip.Flip());
        }

	}
}
