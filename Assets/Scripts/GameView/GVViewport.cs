//-----------------------------------------------------------------
// When a scene is loaded into the GameView this script adjusts the
// camera to fit the right part of the screen.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameView
{
	public class GVViewport : MonoBehaviour
	{
		public bool drawViewport = true;

		private Camera cam;

		//Used for checking if the screen size has changed
		float prevHeight = 0;
		float prevWidth = 0;

		public void Init(Camera _cam)
		{
			cam = _cam;

			Rect pr = GVScreen.DisplayRect;
			cam.pixelRect = new Rect(pr.x, Screen.height - pr.yMax, pr.width, pr.height);
		}

		void Update()
		{
			if (cam == null)
				return;

			if (drawViewport)
			{
				if (GVScreen.isMinimized)
				{
					_DisableCamera();
				}
				else
				{
					_EnableCamera();
				}
			}
			else
			{
				_DisableCamera();
				return;
			}

			//if the screen has changed
			if (Screen.height != prevHeight || Screen.width != prevWidth)
			{
				Rect pr = GVScreen.DisplayRect;
				cam.pixelRect = new Rect(pr.x, Screen.height - pr.yMax, pr.width, pr.height);

				prevHeight = Screen.height;
				prevWidth = Screen.width;
			}
		}

		private void _EnableCamera () {
			if (cam.enabled == false)
				cam.enabled = true;
		}
		public void EnableViewport()
		{
			drawViewport = true;
		}
		private void _DisableCamera()
		{
			if (cam.enabled == true)
				cam.enabled = false;
		}
		public void DisableViewport()
		{
			drawViewport = false;
		}
	}

}
