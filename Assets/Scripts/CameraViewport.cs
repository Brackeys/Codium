//-----------------------------------------------------------------
// When a scene is loaded into the GameView this script adjusts the
// camera to fit the right part of the screen.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraViewport : MonoBehaviour {
	
	Camera cam;

	//Used for checking if the screen size has changed
	float prevHeight = 0;
	float prevWidth = 0;

	void Start () {
		cam = Camera.main;
		if (cam == null) {
			Debug.LogError ("No camera found in Camera Vieport!");
			return;
		}

		Rect pr = ScreenSetup.DisplayRect;
		cam.pixelRect = new Rect (pr.x, Screen.height - pr.yMax, pr.width, pr.height);
	}

	void Update () {
		//if the screen has changed
		if (Screen.height != prevHeight || Screen.width != prevWidth) {
			Rect pr = ScreenSetup.DisplayRect;
			cam.pixelRect = new Rect (pr.x, Screen.height - pr.yMax, pr.width, pr.height);

			prevHeight = Screen.height;
			prevWidth = Screen.width;
		}
	}
}
