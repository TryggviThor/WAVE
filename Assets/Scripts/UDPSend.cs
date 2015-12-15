using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour {
	// prefs
	public string IP = "127.0.0.1"; // 255.255.255.255
	public int port = 52525;

	public bool broadcast = true;

	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;

	Thread broadcastThread;
	UdpClient broadcastClient;

	// gui
	string strOne = "";
	string strTwo = "";
	string strThree = "";



	public void Start () {
		initSender ();

		if (broadcast) {
			initBroadcast ();
		}
	}

	void OnDisable () {
		if (client != null) client.Close ();
		if (broadcastThread != null) broadcastThread.Abort ();
		if (broadcastClient != null) broadcastClient.Close ();
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

	// Initialize the sender 
	public void initSender () {
		remoteEndPoint = new IPEndPoint (IPAddress.Parse (IP), port);
		client = new UdpClient ();
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

	// Initialize the broadcasting thread
	private void initBroadcast () {
		broadcastThread = new Thread (
				new ThreadStart (sendBroadcast));
		broadcastThread.IsBackground = true;
		broadcastThread.Start ();
	}

	// broadcast every few seconds
	private void sendBroadcast () {
		int broadcastPort = 52526;
		IPEndPoint broadcastEndpoint = new IPEndPoint (IPAddress.Broadcast, broadcastPort);
		broadcastClient = new UdpClient ();

		byte[] tinyPacket = { Convert.ToByte (0) };

		while (true) {
			broadcastClient.Send (tinyPacket, tinyPacket.Length, broadcastEndpoint);

			Thread.Sleep (2000);
		}
    }
}