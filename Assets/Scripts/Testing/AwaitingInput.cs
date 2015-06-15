//-----------------------------------------------------------------
// Small text animation when console has nothing to display.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class AwaitingInput : MonoBehaviour {
	
	public float speed = 10f;
	const int interval = 8;

	// Private variables to handle time and index
	float nextTime = 0;
	int index = 0;

	// used to check for logs
	public RectTransform logParrent;

	// Cache
	Text text;

	void Awake () {
		// Cache components
		text = GetComponent<Text>();

		if (logParrent == null) {
			Debug.LogError("No log parrent referenced.");
			return;
		}

		Debug.Log("To fix: Check performance here.");
	}

	// Update is called once per frame
	void Update () {
		// If the console is empty
		if (logParrent.childCount == 0) {
			// If it is time to animate
			if (nextTime < Time.time) {
				// Set string based on index
				switch (index)
				{
					case 0:
					text.text = "";
					break;

					case 1:
					text.text = ".";
					break;

					case 2:
					text.text = "..";
					break;

					case 3:
					text.text = "...";
					break;

					case interval:
					index = -1;
					break;
				}

				index++;

				// Set next animation time
				nextTime = Time.time + 1/speed;
			}
		} else {
			text.text = "";
		}
	}
}
