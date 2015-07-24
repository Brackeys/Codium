//-----------------------------------------------------------------
// A console to display Unity's debug logs in-game.
//-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace GameView
{
	public class GVConsole : MonoBehaviour
	{
		struct Log
		{
			public string message;
			public string stackTrace;
			public LogType type;
		}

		List<Log> logs = new List<Log>();
		Vector2 scrollPosition;
		bool collapse;

		// Visual elements:

		static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
	{
		{ LogType.Assert, Color.white },
		{ LogType.Error, Color.red },
		{ LogType.Exception, Color.red },
		{ LogType.Log, Color.white },
		{ LogType.Warning, Color.yellow },
	};

		private bool drawConsole = true;
		public void EnableConsole()
		{
			drawConsole = true;
		}
		public void DisableConsole()
		{
			drawConsole = false;
		}

		public GUISkin skin;
		public GUIStyle style;
		public Texture2D flatTex;

		public Color bgColor, logColor01, logColor02;

		GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
		GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

		void Start()
		{
			if (flatTex == null)
			{
				Debug.LogError("No flat texture referenced.");
				return;
			}
		}

		void OnGUI()
		{
			if (!drawConsole)
				return;

			if (GVScreen.isMinimized)
				return;

			GUI.skin = skin;	// Apply the GUISkin to use for styling

			Rect displayRect = GVScreen.DisplayRect;

			// Draw a color on a certain part of the screen.
			DrawFillTexture(displayRect, bgColor);

			// Constrain GUILayout content to be inside the actual display of the screen texture
			GUILayout.BeginArea(new Rect(displayRect.x, displayRect.y, displayRect.width, displayRect.height - 30));

			// Create a scrollview
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);

			// Store the label color in a temp var to switch between the two different colors
			Color labelBGColor = logColor01;

			// Iterate through the recorded logs.
			for (int i = 0; i < logs.Count; i++)
			{
				var log = logs[i];

				// Combine identical messages if collapse option is chosen.
				if (collapse)
				{
					var messageSameAsPrevious = i > 0 && log.message == logs[i - 1].message;

					if (messageSameAsPrevious)
					{
						continue;
					}
				}

				// Set the color of the label and store the previous color in a temp var
				Color prevContentColor = GUI.contentColor;
				Color prevBGColor = GUI.backgroundColor;
				GUI.contentColor = logTypeColors[log.type];
				GUI.backgroundColor = labelBGColor;

				// Create a label with a color that corresponds to the log type
				GUILayout.Label("> " + log.message);

				// Set the color back
				GUI.contentColor = prevContentColor;
				GUI.backgroundColor = prevBGColor;

				// Switch between the two colors
				if (labelBGColor == logColor01)
				{
					labelBGColor = logColor02;
				}
				else
				{
					labelBGColor = logColor01;
				}
			}

			GUILayout.EndScrollView();
			GUILayout.EndArea();

			// Make the clear button and a toggle for collapsing logs

			GUI.backgroundColor = Color.clear;

			Rect clearButtonRect = new Rect(displayRect.x + displayRect.width / 2f - 30, displayRect.y + displayRect.height - 31, 60, 30);
			if (GUI.Button(clearButtonRect, clearLabel))
			{
				logs.Clear();
			}

			//collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));
		}

		// Draws a color on the screen using a tinted white texture.
		void DrawFillTexture(Rect displayRect, Color color)
		{
			Color prevColor = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(displayRect, flatTex);
			GUI.color = prevColor;
		}

		// Records a log from the log callback.
		void HandleLog(string message, string stackTrace, LogType type)
		{
			// Force the scrollbar to the bottom position.
			scrollPosition.y = Mathf.Infinity;

			logs.Add(new Log()
			{
				message = message,
				stackTrace = stackTrace,
				type = type,
			});
		}

		// Used for registering logs:

		void OnEnable()
		{
			Application.RegisterLogCallback(HandleLog);
		}

		void OnDisable()
		{
			Application.RegisterLogCallback(null);
		}
	}

}
