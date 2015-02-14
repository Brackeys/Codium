//-----------------------------------------------------------------
// A general script to handle Scrollbar functionality such as
// turning it on & off when needed and forcing it to the bottom.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]
public class ScrollbarHandler : MonoBehaviour {

	private Scrollbar scrollbar;
	private bool isNeeded = false;
	private float hideCountdown = 0f;

	void Awake () {
		scrollbar = GetComponent<Scrollbar>();
		scrollbar.interactable = false;
	}

	void Update () {
		HandleInteractability();
		if (hideCountdown > 0f)
			HideScrollbar ();
	}

	void HideScrollbar () {
		hideCountdown -= Time.deltaTime;
		if (hideCountdown <= 0) {
			scrollbar.interactable = false;
		}
	}

	// Turns the scrollbar on/off if needed
	void HandleInteractability () {
		if (scrollbar.size == 0) {
			scrollbar.size = 1;
		}

		if (scrollbar.size < 0.99f) {
			isNeeded = true;
		} else {
			isNeeded = false;
		}
	}

	// Call this from the scrollrect on value changed
	public void ShowScrollbar () {
		if (!isNeeded)
			return;

		scrollbar.interactable = true;
		hideCountdown = 2f;
	}
}
