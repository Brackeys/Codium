//-----------------------------------------------------------------
// Loads levels and sets up console.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace GameView
{
	[RequireComponent(typeof(GVViewport))]
	[RequireComponent(typeof(GVConsole))]
	public class GVManager : MonoBehaviour
	{

		public enum GameType
		{
			level,
			console
		};

		public GameType gameType;
		public string scene = "";

		// Use this for initialization
		void Awake()
		{
			if (gameType == GameType.level)
			{
				Debug.Log("TODO: Check if scene exists.");
				Application.LoadLevelAdditive(scene);
				//GetComponent<GVViewport>().enabled = true;
			}
			else if (gameType == GameType.console)
			{
				//GetComponent<GVConsole>().enabled = true;
			}
		}
	}
}
