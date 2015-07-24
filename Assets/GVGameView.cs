//-----------------------------------------------------------------
// The actual GameView component. Sits on the parent object of all game scenes.
// Interfaces with the CourseView scene where necessary.
//-----------------------------------------------------------------

using UnityEngine;
using GameView;

public class GVGameView : MonoBehaviour {

	public Camera gameCam;

	private GVManager gvManager;

	void Start()
	{
		if (gameCam == null)
		{
			Debug.LogError("GVGameView: No game camera referenced. Defaulting to main.");
			gameCam = Camera.main;
		}

		gvManager = GVManager.ins;
		if (gvManager == null)
		{
			Debug.LogError("No GVManager?!");
		}

		GVViewport _vp = gvManager.GetComponent<GVViewport>();
		if (_vp == null)
		{
			Debug.LogError("No GVViewport on GVManager.");
		}
		else
		{
			_vp.Init(gameCam);
		}
	}

}
