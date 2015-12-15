using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

[ExecuteInEditMode]
public class MixerChanger: MonoBehaviour {

	// Drums: LowPass, Reverb
	//		LowPass: Frequency range: 60Hz - 20000Hz
	//				 Resonance range: 1.0 - 10.0
	//		Reverb: All values maxed except Room and Decay time
	//				Room: -5000.0Hz - -1500.0Hz
	//				Decay time: 1.0s - 5.0s

	public int size = 0;
	public string[] parameters;
	public Vector2[] range;
	public bool[] negative;

	int origSize = 0;
	Slider slider;
	AudioMixer mixer;

	void Start () {
		slider = GetComponent<Slider> ();
		mixer = transform.parent.GetComponent<AudioControl> ().mixerGroup.audioMixer;
    }

	void Update () {
		// Resize every array to be matching
		if (size != origSize) {
			Array.Resize (ref parameters, size);
			Array.Resize (ref range, size);
			Array.Resize (ref negative, size);
			origSize = size;
		}

		SetValues ();
	}

	public void SetValues () {
		for (int i = 0; i < parameters.Length; i++) {
			if (negative[i]) {
				mixer.SetFloat (parameters[i], range[i].x + range[i].y - (range[i].x + slider.value * (range[i].y - range[i].x)));
			}
			else {
				mixer.SetFloat (parameters[i], range[i].x + slider.value * (range[i].y - range[i].x));
			}
		}
	}
}
