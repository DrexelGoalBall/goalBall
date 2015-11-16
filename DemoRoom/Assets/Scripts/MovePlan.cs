using UnityEngine;
using System.Collections;

public class MovePlan : MonoBehaviour {

	private WiimoteReceiver receiver;
	private WiimoteTransmitter transmitter;
	private Quaternion initPosition;
	public Texture2D cursorOne;


	// Use this for initialization
	void Start () {
		receiver = WiimoteReceiver.Instance;
		receiver.connect();
		initPosition = transform.rotation;
		
		transmitter = WiimoteTransmitter.Instance;
		transmitter.setConnectionInfo("localhost", 8000);
		transmitter.connect();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Wiimote mymote = receiver.wiimotes[1];

		float pitch = mymote.PRY_PITCH; 
		float roll = mymote.PRY_ROLL;
		float yaw = mymote.PRY_YAW;
		Debug.Log (pitch + " " + roll + " " + yaw);
		gameObject.transform.Rotate (new Vector3 ( roll, pitch, yaw));
		//GUI.Label(new Rect(5,215,110,20), "Accel" + mymote.PRY_ACCEL.ToString());

	}
}
