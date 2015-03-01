using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UICodeText : MonoBehaviour {

	private Text text;

	void Awake () {
		text = GetComponent<Text>();
	}

	void Start () {
		text.text = "";
	}

	public void SetCode (string code) {
		text.text = CodeManager.FormatCode (code);
	}

}
