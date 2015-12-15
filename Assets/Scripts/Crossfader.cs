using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Crossfader : MonoBehaviour {

	Slider slider;
	AudioSource one;
	AudioSource two;

	void Start () {
		slider = GetComponent<Slider> ();
		one = transform.parent.GetComponents<AudioSource> ()[0];
		two = transform.parent.GetComponents<AudioSource> ()[1];
	}
	
	void Update () {
		SetValues ();
	}

	void SetValues () {
		if (slider.value < 0.5f) {
			one.volume = slider.value * 2;
			two.volume = 1.0f;
		}
		else {
			one.volume = 1.0f;
			two.volume = 1.0f - (slider.value - 0.5f) * 2;
		}
	}
}
