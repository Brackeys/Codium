//-----------------------------------------------------------------
// Changes the text of a UI Text component to the current system time.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class UIClock : MonoBehaviour {

	private Text clockText;
	
	void Awake () {
		clockText = GetComponent<Text> ();

		if (clockText == null) {
			Debug.LogError ("No Text component found on " + transform.name);
			return;
		}

		InvokeRepeating ("UpdateClock", 0f, 5f);
	}

	public void UpdateClock () {
		System.DateTime time = System.DateTime.Now;
		clockText.text = time.ToString ("hh:mm");

		Debug.Log ("Looping coroutine");
	}

}
