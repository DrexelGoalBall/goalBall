using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using WiimoteApi;

public class GoalBallPlayerMovementWIIV1 : MonoBehaviour
{

    private Quaternion initial_rotation;

    private Wiimote wiimote;

    private Vector2 scrollPosition;

    private Vector3 wmpOffset = Vector3.zero;

      //Control names
    public string Horizonal = "Horizontal";
    public string Vertical = "Vertical";

    //Required Components
    private Rigidbody RB;

    //Movement Speed
    public float speed = 10f;
    

    void Start() 
    {
                RB = gameObject.GetComponent<Rigidbody>();

    }

	// Update is called once per frame
	void Update ()
    {

         wiimote = WiimoteManager.Wiimotes[0];

        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();

            if (ret > 0 && wiimote.current_ext == ExtensionController.MOTIONPLUS) {
                Vector3 offset = new Vector3(  -wiimote.MotionPlus.PitchSpeed,
                                                wiimote.MotionPlus.YawSpeed,
                                                wiimote.MotionPlus.RollSpeed) / 95f; // Divide by 95Hz (average updates per second from wiimote)
                wmpOffset += offset;

            }
        } while (ret > 0);

        //NunchuckData data = wiimote.Nunchuck;
        MotionPlusData data = wiimote.MotionPlus;


        float hinput = data.PitchSpeed;
        float vinput = data.YawSpeed;

        RB.velocity = (gameObject.transform.right * hinput * speed)  +  (gameObject.transform.forward * vinput * speed);
	}
}
