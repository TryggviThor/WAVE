using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[RequireComponent (typeof (AudioSource))]
public class AudioVisualizer: MonoBehaviour {

	public float tileScale = 0.5f;
	public float desired = 1.0f;
	public float scale = 32;
	public float radius = 32;

	float[] spectrum = new float[512]; // Power of two

	AudioSource audioSpec; // Audio for spectrum

	GameObject[] freqRings;
	float[] desiredScales;
	int freqRes = 42; // Resolution cut

	Vector3 updateScale;
	Collider[] updateColls;
	List<Transform> oldParents = new List<Transform> ();
	List<Transform> childColls = new List<Transform>();
	int distIndex;

	bool hasInit = false;


	void OnEnable () {
		if (hasInit) {
			audioSpec.UnPause ();
		}
	}

	void OnDisable () {
		if (hasInit) {
			audioSpec.Pause ();
		}
	}

	void OnDestroy () {
		
	}

	void Start () {
		InitAudioStuff ();

		freqRings = new GameObject[freqRes];
		desiredScales = new float[freqRes];
		GameObject go = new GameObject();
		for (int i = 0; i < freqRes; i++) {
            freqRings[i] = Instantiate (go, transform.position, transform.rotation) as GameObject;
			freqRings[i].name = "Freq" + i;
			freqRings[i].transform.parent = transform;
			desiredScales[i] = desired;
		}
		Destroy (go);


		// Find scaling cubes in a radius and set their parent
		updateColls = Physics.OverlapSphere (transform.position, radius - 1);
		for (int i = 0; i < updateColls.Length; i++) {
			if (updateColls[i].gameObject.tag == "ScalingCube" &&
				updateColls[i].transform.up == transform.up) {
				updateColls[i].enabled = false; // Updating colliders is expensive
				distIndex = (int) ((Vector3.Distance (updateColls[i].transform.position, transform.position)) * 1/tileScale);
				if (distIndex < freqRes) {
					childColls.Add (updateColls[i].transform);
					oldParents.Add (updateColls[i].transform.parent);
					updateColls[i].transform.parent = freqRings[distIndex].transform;
				}
			}
		}
	}

	void FixedUpdate () {
		// Update data
		AudioListener.GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);
	}

	void Update () {
		for (int i = 0; i < freqRes; i++) {
			// Update the desired scale for the ring
			desiredScales[i] = desired + spectrum[i] * scale;

			// Lerp to desired scale
			updateScale = freqRings[i].transform.localScale;
			updateScale.y = Mathf.Lerp (updateScale.y, desiredScales[i], Time.deltaTime * 10); // Magic 10
			//if (updateScale.y <= 1.02f) updateScale.y = 1.0f;
			freqRings[i].transform.localScale = updateScale;
		}

		if (!audioSpec.isPlaying) {
			// Reset to old parents
			for (int i = 0; i < updateColls.Length; i++) {
				if (updateColls[i] &&
                    updateColls[i].gameObject.tag == "ScalingCube") {
					updateColls[i].enabled = true;
				}
			}
			for (int i = 0; i < childColls.Count; i++) {
				childColls[i].parent = oldParents[i];
			}
			Destroy (gameObject);
		}
	}

	// Create a new child audiosource for the audio spectrum
	void InitAudioStuff () {
		audioSpec = GetComponent<AudioSource> ();

		hasInit = true;
	}
}