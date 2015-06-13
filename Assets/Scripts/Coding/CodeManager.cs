//-----------------------------------------------------------------
// 1. Implements the SyntaxHighlighter by supplying keywords from txt files
// & the code to format. 
// 2. Implements the CodeCompiler to evaluate C# code at runtime
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CodeManager : MonoBehaviour {

	public MCC mcc;
	public Text code;

	void Start()
	{
		mcc = new MCC();
	}

	public void RunCode()
	{
		mcc.Run(code.text);
	}

	public void EvaluateCode()
	{
		System.Object res;
		mcc.Evaluate<System.Object>(code.text, out res);
		//Debug.Log(res);
	}

	public void GetLastError()
	{
		Debug.Log (mcc.GetLastError());
	}

	//public bool setupSyntaxHighlighter = true;	// Setup a SyntaxHighlighter at startup?
	//// An array of keywords (TextAsset).
	//// The first line in the txt file is the color.
	//public TextAsset[] keywordTxtFiles;

	//public bool setupCompiler = true;	// Setup a CodeCompiler at startup?
	//[Multiline]
	//public string extraNamespaces = "";

	//public Text codeText;
	//public Text formattedCodeText;

	//private static SyntaxHighlighter syntaxHighlighter;
	//private static CodeCompiler codeCompiler;

	//void Awake () {
	//	if (setupSyntaxHighlighter)
	//		SetupSyntaxHighlighter (keywordTxtFiles);

	//	if (setupCompiler)
	//		SetupCompiler (extraNamespaces);
	//}

	////PUBLIC METHODS

	//// Public method for evaluating C# code if a text object is referenced
	//public void RunCodeByReference()
	//{
	//	if (codeText == null)
	//	{
	//		Debug.LogError("Could not run code: No codeText object referenced.");
	//		return;
	//	}

	//	CodeManager.RunCode(codeText.text);
	//}

	//// Public method for formatting code by reference
	//public void FormatCodeByReference(string code)
	//{
	//	if (formattedCodeText == null)
	//	{
	//		Debug.LogError("Could not run code: No formattedCodeText object referenced.");
	//		return;
	//	}

	//	if (syntaxHighlighter == null)
	//	{
	//		Debug.LogError("Couldn't format code because no SyntaxHighlighter was set up in CodeManager.");
	//		return;
	//	}

	//	formattedCodeText.text = syntaxHighlighter.Highlight(code);
	//}

	//// STATIC SETUP METHODS

	//// Static method for creating a SyntaxHighlighter class
	//public static void SetupSyntaxHighlighter (TextAsset[] keywordTxtFiles) {
	//	// Init an instance of the SyntaxHighlighter class
	//	syntaxHighlighter = new SyntaxHighlighter();

	//	// Loop through the keywords array and add them to the SyntaxHighlighter
	//	for (int i = 0; i < keywordTxtFiles.Length; i++) {
	//		syntaxHighlighter.AddKeywords (keywordTxtFiles[i]);
	//	}
	//}

	//// Static method for creating a SyntaxHighlighter class
	//public static void SetupSyntaxHighlighter (TextAsset keywordTxtFile) {
	//	// Init an instance of the SyntaxHighlighter class
	//	syntaxHighlighter = new SyntaxHighlighter();

	//	//Add the keywords in keywordTxtFile to the SyntaxHighlighter
	//	syntaxHighlighter.AddKeywords (keywordTxtFile);
	//}

	//// Static method for creating an instance of CodeCompiler & initializing it
	//public static void SetupCompiler (string namespaces) {
	//	// Init an instance of the CodeCompiler class
	//	codeCompiler = new CodeCompiler();

	//	// Initialize the evaluator to make it ready for compiling
	//	codeCompiler.InitEvaluator ("using UnityEngine;\n" + 
	//								"using System;\n" + namespaces);
	//}

	//// STATIC USAGE METHODS

	//// Static method for evaluating C# code
	//public static void RunCode (string code) {
	//	if (codeCompiler == null) {
	//		Debug.LogError("Couldn't run code because no CodeCompiler was set up in CodeManager.");
	//		return;
	//	}

	//	if (!codeCompiler.IsInitialized) {
	//		Debug.LogError ("Code couldn't be run. Evaluator not initialized.");
	//		return;
	//	}

	//	codeCompiler.RunCode (code);
	//}

	//// Static method for formatting code by applying syntax highlighting
	//public static string FormatCode (string code) {
	//	if (syntaxHighlighter == null) {
	//		Debug.LogError("Couldn't format code because no SyntaxHighlighter was set up in CodeManager.");
	//		return null;
	//	}

	//	return syntaxHighlighter.Highlight(code);
	//}
}
