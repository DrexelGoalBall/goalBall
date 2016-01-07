using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour 
{
	[SerializeField] Camera playerCamera;
	[SerializeField] AudioListener audioListener;

	// Use this for initialization
	public override void OnStartLocalPlayer ()
	{
        //GameObject sc = GameObject.FindGameObjectWithTag("MainCamera");
        //sc.SetActive(false);
        GoalBallPlayerMovementV1 gbpm = GetComponent<GoalBallPlayerMovementV1>();
        gbpm.enabled = true;
        CameraBox cameraBox = GameObject.Find("CameraBox").GetComponent<CameraBox>();
        //cameraBox.cams.Add(playerCamera);
        playerCamera.enabled = true;
		audioListener.enabled = true;
	}
}
