//-----------------------------------------------------------------
// This component currently doesn't do too much but will probably be
// expanded on in the future.
// The central component for loading the GameView into the CourseView scene.
//-----------------------------------------------------------------

using UnityEngine;

namespace GameView
{
	[RequireComponent(typeof(GVScreen))]
	[RequireComponent(typeof(GVViewport))]
	[RequireComponent(typeof(GVConsole))]
	public class GVManager : MonoBehaviour
	{

		#region Singleton pattern (Awake)
		private static GVManager _ins;
		public static GVManager ins
		{
			get
			{
				if (_ins == null)
				{
					_ins = GameObject.FindObjectOfType<GVManager>();
				}

				return _ins;
			}
			set
			{
				_ins = value;
			}
		}

		void Awake()
		{
			if (_ins == null)
			{
				// Populate with first instance
				_ins = this;
			}
			else
			{
				// Another instance exists, destroy
				if (this != _ins)
					Destroy(this.gameObject);
			}
		}

		#endregion

		public void SetupGameScene(string _sceneName)
		{
			Application.LoadLevelAdditive(_sceneName);
		}
	}
}
