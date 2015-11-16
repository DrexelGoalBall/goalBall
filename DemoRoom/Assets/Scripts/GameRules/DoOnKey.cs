using UnityEngine;
using System.Collections;

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
