using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour {

	public AudioClip loopOne;
	public AudioClip loopTwo;
	public AudioMixerGroup mixerGroup;

	AudioSource sourceOne;
	AudioSource sourceTwo;

	void OnEnable () {
		sourceOne = GetComponents<AudioSource> ()[0];
		sourceTwo = GetComponents<AudioSource> ()[1];

		sourceOne.clip = loopOne;
		sourceTwo.clip = loopTwo;

		sourceOne.outputAudioMixerGroup = mixerGroup;
		sourceTwo.outputAudioMixerGroup = mixerGroup;

		sourceOne.loop = true;
		sourceTwo.loop = true;

		sourceOne.Play ();
		sourceTwo.Play ();
	}
}
