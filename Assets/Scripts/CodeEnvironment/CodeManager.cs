//-----------------------------------------------------------------
// Sets up the scripting environment
// Implements the UCCE
// Implements the SyntaxHighlighter
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class CodeManager : MonoBehaviour {

	public bool testGUI = false;
	private string testCode = "";	// Testing variable

	public bool setupSyntaxHighlighter = true;	// Setup a SyntaxHighlighter at startup?
	// An array of keywords (TextAsset).
	// The first line in the txt file is the color.
	public TextAsset[] keywordTxtFiles;

	public bool setupUCCE = true;	// Setup UCCE at startup?

	public Text codeText;
	public Text formattedCodeText;

	private SyntaxHighlighter syntaxHighlighter;
	private UCCE ucce;
	private int namespaceCounter = 0;


	#region Singleton pattern (Awake)

	private static CodeManager _ins;
	public static CodeManager ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<CodeManager>();

				DontDestroyOnLoad(_ins.gameObject);
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
			DontDestroyOnLoad(this);
		}
		else
		{
			// Another instance exists, destroy
			if (this != _ins)
				Destroy(this.gameObject);
		}

		if (setupSyntaxHighlighter)
			SetupSyntaxHighlighter(keywordTxtFiles);

		if (setupUCCE)
			SetupUCCE();
	}

	#endregion

	// Testing GUI
	void OnGUI()
	{
		if (!testGUI)
			return;

		testCode = GUI.TextArea(new Rect(400, 0, 500, 500), testCode);
		if (codeText.transform.parent.GetComponent<InputField>().text != testCode)
			FormatCodeByReference(testCode);
		codeText.transform.parent.GetComponent<InputField>().text = testCode;
	}

	#region Setup methods

	// Method for creating a SyntaxHighlighter class
	private void SetupSyntaxHighlighter(TextAsset[] keywordTxtFiles)
	{
		// Init an instance of the SyntaxHighlighter class
		syntaxHighlighter = new SyntaxHighlighter();

		// Loop through the keywords array and add them to the SyntaxHighlighter
		for (int i = 0; i < keywordTxtFiles.Length; i++)
		{
			syntaxHighlighter.AddKeywords(keywordTxtFiles[i]);
		}
	}

	// Method for creating a SyntaxHighlighter class
	private void SetupSyntaxHighlighter(TextAsset keywordTxtFile)
	{
		// Init an instance of the SyntaxHighlighter class
		syntaxHighlighter = new SyntaxHighlighter();

		//Add the keywords in keywordTxtFile to the SyntaxHighlighter
		syntaxHighlighter.AddKeywords(keywordTxtFile);
	}

	// Method for creating an instance of UCCE
	private void SetupUCCE()
	{
		// Init an instance of the UCCE
		ucce = new UCCE();
	}

	#endregion

	#region Usage methods

	//----------- CODE COMPILATION + ERROR HANDLING ------------

	// Static method for evaluating C# code
	public void RunCode(string _code)
	{
		if (ucce == null)
		{
			Debug.LogError("Couldn't run code because no UCCE was set up in CodeManager.");
			return;
		}

		if (ucce.Run(WrapInNamespace(_code))) {
			namespaceCounter++;
			CallMethods();
		}
		PrintLastError();
	}

	// Public method for evaluating C# code if a Text object is referenced
	public void RunCodeByReference()
	{
		if (codeText == null)
		{
			Debug.LogError("Could not run code: No codeText object referenced.");
			return;
		}

		RunCode(codeText.text);
	}

	// Print last known error
	public void PrintLastError()
	{
		string _error = ucce.GetLastError();
		if (_error != "")
		{
			Debug.Log(ucce.GetLastError());
		}
	}

	private string WrapInNamespace(string _code)
	{
		_code = "namespace Codium" + namespaceCounter + " {\n" + _code + "\n}";
		//Debug.Log(_code);
		return _code;
	}

	public void CallMethods()
	{
		if (ucce.GetLastError() != "")
			return;

		var type = typeof(ICodiumBase);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => type.IsAssignableFrom(p));

		int highestNamespaceNumber = -1;
		Type mostRecentType = default(Type);

		for (int i = 0; i < types.ToArray().Length; i++)
		{
			Type _type = types.ToArray()[i];
			if (_type.IsClass)
			{
				string _namespace = _type.Namespace;
				//Debug.Log("Type: " + _type + " in namespace: " + _namespace);
				string _numberSubStr = _namespace.Substring(_namespace.IndexOfAny("0123456789".ToCharArray()));
				int _namespaceNumber = Convert.ToInt32(new string (_numberSubStr.TakeWhile(char.IsDigit).ToArray()));

				if (_namespaceNumber > highestNamespaceNumber)
				{
					highestNamespaceNumber = _namespaceNumber;
					mostRecentType = _type;
				}
			}
		}

		object instance = Activator.CreateInstance(mostRecentType);
		MethodInfo method = mostRecentType.GetMethod("Entry", BindingFlags.Instance | BindingFlags.Public);

		method.Invoke(instance, null);
	}


	//----------- SYNTAX HIGHLIGHTING ------------

	// Static method for formatting code by applying syntax highlighting
	public string FormatCode(string _code)
	{
		if (syntaxHighlighter == null)
		{
			Debug.LogError("Couldn't format code because no SyntaxHighlighter was set up in CodeManager.");
			return null;
		}

		return syntaxHighlighter.Highlight(_code);
	}

	// Public method for formatting code by reference
	public void FormatCodeByReference(string _code)
	{
		if (formattedCodeText == null)
		{
			Debug.LogError("Could not run code: No formattedCodeText object referenced.");
			return;
		}

		if (syntaxHighlighter == null)
		{
			Debug.LogError("Couldn't format code because no SyntaxHighlighter was set up in CodeManager.");
			return;
		}

		formattedCodeText.text = syntaxHighlighter.Highlight(_code);
	}
	#endregion

}
