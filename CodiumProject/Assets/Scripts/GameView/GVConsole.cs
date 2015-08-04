//-----------------------------------------------------------------
// A console to display Unity's debug logs in-game.
//-----------------------------------------------------------------

using System.Collections.Generic;
using System;
using UnityEngine;

namespace GameView
{
	[System.Serializable]
	public struct Log
	{
		public string message;
		public string stackTrace;
		public LogType type;
		public bool systemLog;
	}

	public class GVConsole : MonoBehaviour
	{
		#region Singleton pattern (Awake)

		private static GVConsole _ins;
		public static GVConsole ins
		{
			get
			{
				if (_ins == null)
				{
					_ins = GameObject.FindObjectOfType<GVConsole>();
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
					this.enabled = false;
			}
		}

		#endregion

		[SerializeField]
		private List<Log> logs = new List<Log>();
		Vector2 scrollPosition;
		public bool collapse;

		#region VISUAL ELEMENTS
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
		//GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
		#endregion

		void Start()
		{
			if (flatTex == null)
			{
				Debug.LogError("No flat texture referenced.");
				return;
			}
		}

		#region DRAWING THE CONSOLE
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
				GUILayout.Label(log.message);

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
		#endregion

		#region RECORDING LOGS
		// Records a log from the log callback.
		public void HandleLog(string message, string stackTrace, LogType type, bool system)
		{
			// Force the scrollbar to the bottom position.
			scrollPosition.y = Mathf.Infinity;

			logs.Add(new Log()
			{
				message = message,
				stackTrace = stackTrace,
				type = type,
				systemLog = system
			});
		}

		//void OnEnable()
		//{
		//	Application.logMessageReceived += HandleLog;
		//}

		//void OnDisable()
		//{
		//	Application.logMessageReceived -= HandleLog;
		//}

		#endregion

		#region ACCESSING LOGS

		// Get the latest log
		public Log GetLatestLog()
		{
			// Filter out the system logs
			List<Log> _logs = new List<Log>();
			for (int i = 0; i < logs.Count; i++)
			{
				if (!logs[i].systemLog)
					_logs.Add(logs[i]);
			}

			if (_logs.Count == 0)
			{
				Log _log = new Log();
				return _log;
			}
			return _logs[_logs.Count - 1];
		}

		// Get logs according to amount (backwards)
		public Log[] GetLogs(int _amount)
		{
			// Filter out the system logs
			List<Log> _logs = new List<Log>();
			for (int i = 0; i < logs.Count; i++)
			{
				if (!logs[i].systemLog)
					_logs.Add(logs[i]);
			}

			if (_logs.Count == 0)
			{
				return new Log[0];
			}
			_amount = Mathf.Clamp(_amount, 1, _logs.Count);
			Log[] _logsArray = new Log[_amount];
			_logsArray = _logs.GetRange(_logs.Count - _amount, _amount).ToArray();
			Array.Reverse(_logsArray);
			return _logsArray;
		}

		// Get all logs reversed (newest first)
		public Log[] GetAllLogs()
		{
			// Filter out the system logs
			List<Log> _logs = new List<Log>();
			for (int i = 0; i < logs.Count; i++)
			{
				if (!logs[i].systemLog)
					_logs.Add(logs[i]);
			}

			Log[] _logsArray = _logs.ToArray();
			Array.Reverse(_logsArray);
			return _logsArray;
		}

		#endregion

	}

}
