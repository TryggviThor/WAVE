using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManagerSecond : MonoBehaviour {

	public Slider one;
	public Slider two;
	public Slider three;

	public AudioMixer masterMixer;

	public AudioSource audioOne;
	public AudioSource audioTwo;
	public AudioSource audioThree;


	void Start () {
		float nice;
		//masterMixer.GetFloat ("masterPitch", out nice);
		//one.value = nice;
		masterMixer.GetFloat ("masterLowPassFreq", out nice);
		two.value = nice;
		masterMixer.GetFloat ("masterFlange", out nice);
		three.value = nice;
	}

	void Update () {
		SetOne ();
		SetTwo ();
		SetThree ();
	}

	public void SetOne () {
		//one.value = (float) UDPReceive.ints[0] / 255;

		audioOne.volume = one.value < 0.2f ? one.value/4 : one.value;
	}

	public void SetTwo () {
		//two.value = (float) UDPReceive.ints[1] / 255;

		audioTwo.volume = two.value < 0.2f ? two.value/4 : two.value;

	}

	public void SetThree () {
		//three.value = (float) UDPReceive.ints[2] / 255;

		audioThree.volume = three.value < 0.2f ? three.value/4 : three.value;
	}
}