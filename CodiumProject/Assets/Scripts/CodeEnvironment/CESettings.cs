using UnityEngine;

namespace CodeEnvironment
{
	public enum ExecutionMode { full, expression, runInMain };

	[System.Serializable]
	public class CESettings
	{
		public ExecutionMode executionMode;
		public string usingNamespaces;

		public CESettings()
		{
			executionMode = ExecutionMode.expression;
			usingNamespaces = "using CodiumAPI";
		}
	}

}
