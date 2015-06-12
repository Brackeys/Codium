//-----------------------------------------------------------------
// Find keywords in the loaded assemblies and add them to a Regex.
// Used by CodeFormatter to do syntax highlighting.
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Reflection;

[System.Serializable]
public class CustomKeyword {
	[HideInInspector]
	public string[] keywords;
	public string color;
	public TextAsset keywordsAsset;
}

public class KeywordAPI : MonoBehaviour {
	
	// This static Regex is used by the CodeFormatter to do code highlighting.
	public static Regex APIKeywords;
	public static string[] APIKeywordsArray;

	// This static is also used by the CodeFormatter to color certain keywords differently.
	public static CustomKeyword[] CustomKeywords;

	// This adds the possibility to include extra keywords.
	public CustomKeyword[] customKeywords;

	public TextAsset extraKeywords;

	void Awake () {

		Debug.Log ("TODO: Complete switch to arrays.");

		// Create a string called regex to fill with keywords
		string regex = "";
		// Find the assembly of the Color property in Unity
		Assembly assembly = typeof(Color).Assembly;
		// Loop through the types in that assembly & add them to the regex string
		foreach (Type type in assembly.GetTypes())
		{
			string formattedLine = type.Name;
			regex += formattedLine + "|";
		}
		
		// If custom keywords are referenced, loop through them and add them to a static array
		if (customKeywords != null) {
			int arrayLength = customKeywords.Length;
			CustomKeywords = new CustomKeyword[arrayLength];
			for (int i = 0; i < arrayLength; i++) {
				CustomKeywords[i] = new CustomKeyword();
				CustomKeywords[i].keywords = ExtractKeywordArrayFromTxt (customKeywords[i].keywordsAsset);
				CustomKeywords[i].color = customKeywords[i].color;
				// Also add them to the final regex.
				// The array is used to compare the regex with the array to create different colors.
				regex += ExtractKeywordsFromTxt (customKeywords[i].keywordsAsset);
				//Debug.Log(ExtractKeywordsFromTxt (customKeywords[i].keywordsAsset));
			}
		}

		// Add the extra keywords
		regex += ExtractKeywordsFromTxt(extraKeywords);
		
		// Assign the regex to a static Regex called APIKeywords.
		APIKeywords = new Regex(regex);
		APIKeywordsArray = ExtractKeywordArrayFromString (regex);
	}

	string[] ExtractKeywordArrayFromString (String regex) {
		return regex.Split( '|' );
	}

	string[] ExtractKeywordArrayFromTxt (TextAsset txt) {
		return txt.text.Split( '\n' );
	}
	
	// This method extracts keywords from a TextAsset. It returns a string.
	string ExtractKeywordsFromTxt (TextAsset txt) {

		string[] keywordLines;
		keywordLines = txt.text.Split( '\n' );
		
		string previousLine = "Default";
		string regex = "";
		foreach (string keywordLine in keywordLines) {
			//string keywordLine = keywordLines[i];
			string formattedLine = keywordLine.Split (' ')[0];
			if (formattedLine != previousLine && formattedLine.Contains('.') != true) {
				regex += formattedLine + "|";
				previousLine = formattedLine;
			}
		}

		return regex;
	}
}
