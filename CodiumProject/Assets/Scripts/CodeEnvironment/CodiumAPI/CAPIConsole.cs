using UnityEngine;
using GameView;

namespace CodiumAPI
{
	public static class Console
	{
		private static GVConsole gvConsole;
		private static void InitGVConsole()
		{
			gvConsole = GVConsole.ins;
			if (gvConsole == null)
			{
				Debug.LogError("No GVConsole found!");
			}
		}

		public static void Print(object _msg)
		{
			if (gvConsole == null)
			{
				InitGVConsole();
			}

			gvConsole.HandleLog(_msg.ToString(), "", LogType.Log);
		}

	}

}
