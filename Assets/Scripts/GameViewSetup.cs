//-----------------------------------------------------------------
// Loads levels and sets up console.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraViewport))]
[RequireComponent(typeof(ConsoleWindow))]
public class GameViewSetup : MonoBehaviour {

	public enum GameType {
		level,
		console
	};

	public GameType gameType;
	public string scene = "";

	// Use this for initialization
	void Awake () {
		if (gameType == GameType.level) {
			Debug.Log ("TODO: Check if scene exists.");
			Application.LoadLevelAdditive (scene);
			GetComponent<CameraViewport>().enabled = true;
		}
		else if (gameType == GameType.console) {
			GetComponent<ConsoleWindow>().enabled = true;
		}
	}
}
