using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Text))]

public class LineCount : MonoBehaviour {

	public Text Code;

	Text lines;
	public int lineCount = 0;

	void Awake () {
		if (Code == null) {
			Debug.LogError ("No code associated with LineCount!");
			return;
		}

		lines = GetComponent<Text>();
	}

	void UpdateLineCount () {
		string lc = "";

		for (int i = 1; i <= lineCount; i++) {
			lc += i + "\n";
		}

		lines.text = lc;
	}
	
	void Update () {
		int count = Code.text.Split('\n').Length;

		if (lineCount != count) {
			lineCount = count;
			UpdateLineCount ();
		}
	}
}