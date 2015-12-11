using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour {
	// prefs
	public string IP = IPAddress.Broadcast.ToString (); // 255.255.255.255
	public int port = 5001;

	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;

	// gui
	string strOne = "";
	string strTwo = "";
	string strThree = "";



	public void Start () {
		initSender ();
	}

	void OnDisable () {
		if (client != null) client.Close ();
	}

	void OnGUI () {
		// Debug GUI
		if (UDPReceive.networkGUI) {
			Rect rectObj = new Rect (40, 380, 200, 400);
			GUIStyle style = new GUIStyle ();
			style.alignment = TextAnchor.UpperLeft;
			GUI.Box (rectObj, "# UDPSend-Data\n127.0.0.1 " + port + " #\n"
						+ "shell> nc -lu 127.0.0.1  " + port + " \n"
					, style);

			// ------------------------
			// send it
			// ------------------------
			strOne = GUI.TextField (new Rect (40, 430, 60, 20), strOne);
			strTwo = GUI.TextField (new Rect (110, 430, 60, 20), strTwo);
			strThree = GUI.TextField (new Rect (180, 430, 60, 20), strThree);
			if (GUI.Button (new Rect (250, 430, 40, 20), "send")) {
				if (strOne == "") strOne = "0";
				if (strTwo == "") strTwo = "0";
				if (strThree == "") strThree = "0";
				sendData (Int32.Parse (strOne), Int32.Parse (strTwo), Int32.Parse (strThree));
			}
		}
	}

	// init
	public void initSender () {
		// Define the end point, to send the messages from
		print ("UDPSend.init()");

		// Send
		remoteEndPoint = new IPEndPoint (IPAddress.Parse (IP), port); // 255.255.255.255 : 5001
		client = new UdpClient ();

		// status
		print ("Sending to " + IP + " : " + port);
		print ("Testing: nc -lu " + IP + " : " + port);

	}

	// sendData
	private void sendData (int one, int two, int three) {
		try {
			byte done = Convert.ToByte (one);
			byte dtwo = Convert.ToByte (two);
			byte dthree = Convert.ToByte (three);

			byte[] data = { done, dtwo, dthree };

			// Send the text to the remote client
			client.Send (data, data.Length, remoteEndPoint);
		}
		catch (Exception err) {
			print (err.ToString ());
		}
	}


	// endless test
	private void sendEndless (int one, int two, int three) {
		do {
			sendData (one, two, three);
		}
		while (true);

	}

}