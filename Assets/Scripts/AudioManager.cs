using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

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

		if (one.value < 0.5f) {
			audioOne.volume = one.value * 2;
			audioTwo.volume = 1.0f;
		} else {
			audioOne.volume = 1.0f;
			audioTwo.volume = 1.0f - (one.value-0.5f) * 2;
		}
		// Pitch 0.5f - 2.0f x
		//masterMixer.SetFloat ("masterPitch", one.value);
	}

	public void SetTwo () {
		//two.value = (float) UDPReceive.ints[1] / 255;
		
		float freqMax = 22000f;
		float freq = freqMax * two.value;

		// Low Pass Freq 0 - 22000 Hz
		masterMixer.SetFloat ("masterLowPassFreq", freq);
        masterMixer.SetFloat ("masterLowPassRes", (6f - (two.value * 6f)));
	}

	public void SetThree () {
		//three.value = (float) UDPReceive.ints[2] / 255;

		// Flange 0f - 20f Hz
		masterMixer.SetFloat ("masterFlange", 20f * three.value);
	}
}
