
using OSC.NET;

public class WiimoteTransmitter {
	
	//Singleton instance of WiimoteReceiver
	static readonly WiimoteTransmitter instance = new WiimoteTransmitter();

	// Returning the instance of this class.
	public static WiimoteTransmitter Instance { get {return instance;}}
	
	private bool isOpen = false;
	
	private string host;
	private int port;
	
	private OSCTransmitter transmitter;
	
	public void setConnectionInfo(string host, int port) {
		this.host = host;
		this.port = port;
	}
	
	public void connect() {
		if(!isOpen) {
			// TODO Open Connection
			this.transmitter = new OSCTransmitter(this.host, this.port);
			this.isOpen = true;
		}
	}
	
	public void close() {
		// TODO Close connection
		this.transmitter.Close();
	}
	
	public void vibrate(int wiimoteID, int length) {
		// TODO Vibrate wiimote.
		string address = "/wii/" + wiimoteID + "/vibrate";
		transmitter.Send(new OSCMessage(address, length));
		//Message /wii/wiimoteID/vibrate
		//Value length		
	}
	
	public void setLED(int wiimoteID, int combination) {
		// TODO update LED.
		//transmitter.Send(OSCPacket...(OSCMessage...))
		string address = "/wii/" + wiimoteID + "/led";
		transmitter.Send(new OSCMessage(address, combination));
	}
	
	public void resetMotionPlus(int wiimoteID) {
		string address = "/wii/" + wiimoteID + "motion/reset";
		transmitter.Send(new OSCMessage(address, 1));
	}
}