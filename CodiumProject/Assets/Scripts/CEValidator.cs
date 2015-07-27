using UnityEngine;
using GameView;

namespace CodeEnvironment
{
	// Logic for all validators
	public class CEValidator : MonoBehaviour
	{
		protected GVConsole console;

		void Start()
		{
			console = GVConsole.ins;
			if (console == null)
			{
				Debug.LogError("No GVConsole found!");
			}
		}

		public virtual bool Validate()
		{
			// This method must be overwritten
			// with proper validation checks
			return true;
		}

	}
}
