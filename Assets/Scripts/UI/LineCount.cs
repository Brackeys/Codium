using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Text))]

public class LineCount : MonoBehaviour {

	public Text Code;

	Text lines;
	int lineCount = 0;

	void Awake () {
		if (Code == null) {
			Debug.LogError ("No code associated with LineCount!");
			return;
		}

		lines = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		int numLines = Code.text.Split('\n').Length;
		if (numLines > lineCount) {
			string lineToAdd = "";
			if (lineCount >= 1) {
				lineToAdd += "\n";
			}
			lineToAdd += numLines.ToString();
			lines.text += lineToAdd;
			lineCount += 1;
		} else if (numLines < lineCount) {
			lines.text = lines.text.Substring(0, lines.text.Length - 1 - lineCount.ToString().Length);
			lineCount -= 1;
		}
	}
}