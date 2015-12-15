using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddressManager: MonoBehaviour {

	public AudioControl[] audioControls;
	Slider[][] sliders;

	void Start () {
		audioControls = GetComponentsInChildren<AudioControl> ();

		sliders = new Slider[audioControls.Length][];
		for (int i = 0; i < audioControls.Length; i++) {
			sliders[i] = audioControls[i].GetComponentsInChildren<Slider> ();
		}
	}
	
	void Update () {
		if (UDPReceive.vecList.Count > 0) {
			for (int i = 0; i < UDPReceive.vecList.Count; i++) {
				for (int j = 0; j < 3; j++) {
					sliders[i][j].value = UDPReceive.vecList[i][j];
                }
			}
		}
	}
}
