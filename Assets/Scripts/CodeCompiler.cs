//-----------------------------------------------------------------
// Evaluates the C# code typed by the user.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Mono.CSharp;

public class CodeCompiler
{
	private bool _isInitialized;	// Has the evaluater been initialized?

	// Gets the initialization state
	public bool IsInitialized
	{
		get { return _isInitialized; }
	}

	// Class constructor
	public CodeCompiler () {
		_isInitialized = false;
	}

	public void InitEvaluator (string namespaces) {
		Evaluator.Interrupt();

		Evaluator.Init(new string[] { });
		foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			//Dbg.Log("refer: {0}", assembly.FullName);
			if( assembly.FullName.Contains("Cecil") || assembly.FullName.Contains("UnityEditor") )
				continue;
			Evaluator.ReferenceAssembly(assembly);
		}

		Evaluator.Run (namespaces);

		_isInitialized = true;
	}

	public void RunCode (string code) {
		bool isSuccess = Evaluator.Run(code);
		string result = "Code Evaluated.";
		if (!isSuccess) {
			result = "Error in Code.";
		}
		Debug.Log (result);
	}
}