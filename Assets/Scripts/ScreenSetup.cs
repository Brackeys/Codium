//-----------------------------------------------------------------
// A convenience script for setting up and updating the screen rect &
// the display rect.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ScreenSetup : MonoBehaviour {

	public static Rect ScreenRect;	//Used for the screen texture
	public static Rect DisplayRect;	//Used for displaying content

	//Settings for the screen texture
	public Texture2D screenTex;
	public float screenSize = 5f;
	public float xOffset = 38.89f, yOffset = 50.3f;

	//Settings for the display rect inside the screen
	public Rect contentRect = new Rect(-43f, -39f, 0.92f, 0.67f);

	//Used for checking if the screen size has changed
	float prevHeight = 0;
	float prevWidth = 0;
	
	void Awake () {
		if (screenTex == null) {
			Debug.LogError("No screen texture referenced.");
			return;
		}

		GenScreenRect ();
		GenDisplayRect ();
	}

	void OnGUI () {
		//if the screen has changed
		if (Screen.height != prevHeight || Screen.width != prevWidth) {
			GenScreenRect ();
			GenDisplayRect ();

			prevHeight = Screen.height;
			prevWidth = Screen.width;
		}

		GUI.depth = -100;	// Draw it on top
		// Draw the screen to display the content on.
		GUI.DrawTexture (ScreenRect, screenTex);
	}

	//Generates a rect for the displaying of the screen texture
	public void GenScreenRect () {
		// Calculate the size based on the height and width of the camera.
		float size = (Screen.height+Screen.width)/(10000-screenSize*1000);
		// The desired width and height of the screen in pixels.
		float width = screenTex.width * size;
		float height = screenTex.height * size;

		ScreenRect = new Rect (Screen.width - xOffset - width, Screen.height - yOffset - height, width, height);
	}

	//Generates a rect for content inside the screen texture: the display
	public void GenDisplayRect () {
		// Calculate the size based on the height and width of the camera.
		float size = (Screen.height+Screen.width)/(10000-screenSize*1000);

		DisplayRect = new Rect (ScreenRect.x - size * contentRect.x, ScreenRect.y - size * contentRect.y, ScreenRect.width * contentRect.width, ScreenRect.height * contentRect.height );
	}
}
