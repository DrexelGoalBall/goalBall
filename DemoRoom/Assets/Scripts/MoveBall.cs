using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {
	private WiimoteReceiver receiver;
	private WiimoteTransmitter transmitter;
	private Quaternion initPosition;
	public Texture2D cursorOne;
	public Rigidbody rb; 


	// Use this for initialization
	void Start () {
		receiver = WiimoteReceiver.Instance;
		receiver.connect();
		initPosition = transform.rotation;
		rb = GetComponent<Rigidbody> ();

		transmitter = WiimoteTransmitter.Instance;
		transmitter.setConnectionInfo("localhost", 8000);
		transmitter.connect();
	
	}
	
	// Update is called once per frame
	void Update () {
		Wiimote mymote = receiver.wiimotes[1];

		float acc = mymote.PRY_ACCEL;
		//float pitch = mymote.PRY_PITCH; 

		if (mymote.BUTTON_A == 1) {
			onButton_press (acc);
		} 
		else if (mymote.BUTTON_B == 1)
		{
			float pitch_old = mymote.PRY_PITCH;
			float pitch_new = mymote.PRY_PITCH; 
		}
	
	}

	void onButton_press(float acc)
	{
		rb.AddForce(0, (acc*20), 0);
		//gameObject.transform.TransformVector (0, 0, pitch);
	}

	void toss()
	{
		
	}
}
