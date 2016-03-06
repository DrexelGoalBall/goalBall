using UnityEngine;
using System.Collections;

/// <summary>
/// This script is a debugging script that changes the layer of the player when the player presses the v key
/// </summary>
public class DebugSwapLayer : MonoBehaviour {

    public string PlayerLayer = "Player";
    public string DefaultLayer = "Default";

    void Start()
    {
        PlayerLayer = LayerMask.LayerToName(gameObject.layer);
    }


	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (LayerMask.LayerToName(gameObject.layer) == PlayerLayer)
                gameObject.layer = LayerMask.NameToLayer(DefaultLayer);
            else
                gameObject.layer = LayerMask.NameToLayer(PlayerLayer);
        }

	}
}
