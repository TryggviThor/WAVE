using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddressManager: MonoBehaviour {

	public AudioControl[] audioControls;

	Slider[][] sliders;
	int lastCount = 0;


	void Start () {
		audioControls = GetComponentsInChildren<AudioControl> ();

		sliders = new Slider[audioControls.Length][];
		for (int i = 0; i < audioControls.Length; i++) {
			sliders[i] = audioControls[i].GetComponentsInChildren<Slider> ();
			if (UDPReceive.vecList[i] != null) {
				sliders[i][0].value = 0f;
				sliders[i][1].value = 1f;
				sliders[i][2].value = 0f;
			}
		}
	}
	
	void Update () {
		if (lastCount != UDPReceive.vecList.Count) {
			for (int i = 0; i < audioControls.Length; i++) {
				if (UDPReceive.vecList[i] != null) {
					sliders[i][0].value = 0f;
					sliders[i][1].value = 1f;
					sliders[i][2].value = 0f;
				}
			}
			lastCount = UDPReceive.vecList.Count;
        }
		if (UDPReceive.vecList.Count > 0) {
			for (int i = 0; i < UDPReceive.vecList.Count; i++) {
				for (int j = 0; j < 3; j++) {
					sliders[i][j].value = UDPReceive.vecList[i][j];
				}
			}
		}
	}
}
