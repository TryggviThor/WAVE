using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {

	// receiving Thread
	Thread receiveThread;

	// udpclient object
	UdpClient client;

	// public
	public int port; // define > init

	// infos
	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = ""; // clean up this from time to time!

	public static bool networkGUI = false;
	public bool drawNetworkGUI = false;

	public static int[] ints = new int[3] { 0, 0, 0 };


	
	void Start () {
		initThread ();
	}

	void Update () {
		networkGUI = drawNetworkGUI;
	}

	void OnDisable () {
		if (receiveThread != null) receiveThread.Abort ();
		if (client != null) client.Close ();
	}

	void OnGUI () {
		// Debug GUI
		if (networkGUI) {
			Rect rectObj = new Rect (40, 10, 200, 400);
			GUIStyle style = new GUIStyle ();
			style.alignment = TextAnchor.UpperLeft;
			GUI.Box (rectObj, "# UDPReceive\nlocalhost " + port + " #\n"
						+ "shell> nc -u localhost : " + port + " \n"
						+ "\nLast Packet: \n" + lastReceivedUDPPacket
						+ "\n\nAll Messages: \n" + allReceivedUDPPackets
					, style);

			if (GUI.Button (new Rect (190, 110, 40, 20), "clear")) {
				getLatestUDPPacket ();
			}
		}
	}

	// init
	private void initThread () {
		receiveThread = new Thread (
			new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}

	// receive thread
	private void ReceiveData () {
		client = new UdpClient (port);
		while (true) {
			try {
				IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
				byte[] data = client.Receive (ref anyIP);

				if (data.Length > 0) ints[0] = data[0] & 0xff;
				if (data.Length > 1) ints[1] = data[1] & 0xff;
				if (data.Length > 2) ints[2] = data[2] & 0xff;

				string text = ints[0] + ", " + ints[1] + ", " + ints[2];
				//Debug.Log (">> " + text);

				lastReceivedUDPPacket = text;

				allReceivedUDPPackets = allReceivedUDPPackets + text + "\n";
				allReceivedUDPPackets = "";
            }
			catch (Exception err) {
				Debug.Log (err.ToString ());
			}
		}
	}

	// getLatestUDPPacket
	// cleans up the rest
	public string getLatestUDPPacket () {
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}
}