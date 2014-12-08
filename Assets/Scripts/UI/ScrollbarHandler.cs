//-----------------------------------------------------------------
// A general script to handle Scrollbar functionality such as
// turning it on & off when needed and forcing it to the bottom.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Scrollbar))]
public class ScrollbarHandler : MonoBehaviour {

	public bool forceToBottomOnChange = false;
	float lastSize = 0;

	Scrollbar scrollbar;

	void Awake () {
		scrollbar = GetComponent<Scrollbar>();
	}

	void OnGUI () {
		HandleInteractability();

		if (forceToBottomOnChange) {
			ForceToBottom();
		}
	}

	// Turns the scrollbar on/off if needed
	void HandleInteractability () {
		if (scrollbar.size == 0) {
			scrollbar.size = 1;
		}

		if (scrollbar.size < 0.99f) {
			scrollbar.interactable = true;
		} else {
			scrollbar.interactable = false;
		}
	}

	// Forces the scrollbar to the bottom if it has changed
	public void ForceToBottom () {
		float sizeDif = scrollbar.size-lastSize;
		if (sizeDif < 0) {
			sizeDif *= -1;
		}
		if (sizeDif > 0.0001) {
			scrollbar.value = 0;
			lastSize = scrollbar.size;
		}
	}
}
