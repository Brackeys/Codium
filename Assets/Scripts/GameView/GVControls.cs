//-----------------------------------------------------------------
// Sets up the GameView controls located under the GVScreen.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace GameView
{
	public class GVControls : MonoBehaviour
	{

		public Camera UICam;
		private RectTransform rt;

		private Rect lastRect;

		void Start()
		{
			rt = GetComponent<RectTransform>();
			if (UICam == null)
			{
				Debug.LogError("GVBControls: No UICam reference.");
				this.enabled = false;
			}

			SetXPosToScreenCenter();
			lastRect = GVScreen.ScreenRect;
		}

		void Update()
		{
			if (GVScreen.ScreenRect != lastRect)
			{
				SetXPosToScreenCenter();
				lastRect = GVScreen.ScreenRect;
			}
		}

		void SetXPosToScreenCenter()
		{
			float _screenPos = GVScreen.ScreenRect.x + GVScreen.ScreenRect.width / 2f;
			Vector3 _worldPos = UICam.ScreenToWorldPoint(new Vector3(_screenPos, 0, 0));
			rt.position = new Vector3(_worldPos.x, rt.position.y, rt.position.z);
		}

	}

}
