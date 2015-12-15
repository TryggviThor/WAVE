using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {

	public int port = 52525;

	// infos
	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = ""; // clean up this from time to time!

	public static bool networkGUI = false;
	public bool drawNetworkGUI = false;

	public static List<Vector3> vecList = new List<Vector3> ();
	public List<string> addresses = new List<string> ();
	public List<float> timers = new List<float> ();


	float timeoutSeconds = 5.0f;
	Thread receiveThread;
	UdpClient client;

	void Start () {
		initThread ();
	}

	void Update () {
		networkGUI = drawNetworkGUI;

		// Count down all timers
		for (int i = 0; i < timers.Count; i++) {
			timers[i] -= 1.0f * Time.deltaTime;
			if (timers[i] < 0.0f) timers[i] = 0.0f;
        }

		// When there's only one address left that's not getting removed, remove it
		if (addresses.Count == 1 && timers[0] == 0.0f) {
			addresses.RemoveAt (0);
			vecList.RemoveAt (0);
			timers.RemoveAt (0);
		}
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

	// initialize the thread
	private void initThread () {
		receiveThread = new Thread (
			new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}

	// receive thread
	private void ReceiveData () {
		client = new UdpClient (port);
		IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);

		bool IPexists;
        while (true) {
			try {
				byte[] data = client.Receive (ref anyIP);

				// Unregister IP if it has timed out
				int i;
				for (i = timers.Count - 1; i >= 0; i--) {
					if (timers[i] == 0.0f) {
						addresses.RemoveAt (i);
						vecList.RemoveAt (i);
						timers.RemoveAt (i);
					}
				}

				// Register the IP if it hasn't been seen before
				IPexists = false;
				for (i = 0; i < addresses.Count; i++) {
					// Check if the IP is already registered
					if (addresses[i] == anyIP.Address.ToString ()) {
						timers[i] = timeoutSeconds;
						IPexists = true;
						break;
					} else {
						IPexists = false;
					}
				}
				if (!IPexists) {
					// Couldn't find the IP
					addresses.Add (anyIP.Address.ToString ());
					vecList.Add (Vector3.zero);
					timers.Add (timeoutSeconds);
				}

				// Convert data bytes to int, then to a float, inside a vector3
				vecList[i] = new Vector3 ((float) (data[0] & 0xff) / 255, 
										  (float) (data[1] & 0xff) / 255, 
										  (float) (data[2] & 0xff) / 255);

				string text = vecList[i].x + ", " + vecList[i].y + ", " + vecList[i].z;

				lastReceivedUDPPacket = text;
				//allReceivedUDPPackets = allReceivedUDPPackets + text + "\n";
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