//-----------------------------------------------------------------
// Sets up the GameView controls located under the GVScreen.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace GameView
{
	public class GVControls : MonoBehaviour
	{
		[SerializeField]
		private Camera UICam;
		[SerializeField]
		private float xMin;

		[SerializeField]
		private GameObject gameViewToggle;

		[Tooltip("Convenience when testing:")]
		public bool alwaysUpdate;

		private RectTransform rt;

		private Rect lastRect;

		void Start()
		{
			rt = GetComponent<RectTransform>();
			if (UICam == null)
			{
				Debug.LogError("GVControls: No UICam reference.");
				this.enabled = false;
			}
			if (gameViewToggle == null)
			{
				Debug.LogError("GVControls: No gameViewToggle referenced.");
				this.enabled = false;
			}

			SetXPosToScreenCenter();
			lastRect = GVScreen.ScreenRect;
		}

		void Update()
		{
			if (GVScreen.ScreenRect != lastRect || alwaysUpdate)
			{
				SetXPosToScreenCenter();
				lastRect = GVScreen.ScreenRect;
			}
		}

		void SetXPosToScreenCenter()
		{
			float _screenPos = GVScreen.ScreenRect.x + GVScreen.ScreenRect.width / 2f;
			_screenPos = Mathf.Clamp(_screenPos, xMin, Mathf.Infinity);
			Vector3 _worldPos = UICam.ScreenToWorldPoint(new Vector3(_screenPos, 0, 0));
			rt.position = new Vector3(_worldPos.x, rt.position.y, rt.position.z);
		}

		public void ConfigureConsoleOnly()
		{
			gameViewToggle.SetActive(false);
		}

	}

}
