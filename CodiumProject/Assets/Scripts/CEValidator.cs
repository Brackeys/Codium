using UnityEngine;

namespace CodeEnvironment
{
	// Logic for all validators
	public class CEValidator : MonoBehaviour
	{
		public virtual bool Validate()
		{
			// This method must be overwritten
			// with proper validation checks
			return true;
		}

	}
}
