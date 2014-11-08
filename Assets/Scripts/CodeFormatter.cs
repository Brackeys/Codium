//-----------------------------------------------------------------
// Formats the code by applying syntax highlighting to certain keywords.
//-----------------------------------------------------------------

using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class CodeFormatter : MonoBehaviour {

	Regex keywords = new Regex ("|Empty|");
	public string[] keywordsArray;

	[System.Serializable]
	public struct KeywordMatch {
		public string Value;
		public int Index;
		public int Length;
	}
	
	[System.Serializable]
	public class CodeChange
	{
		public int index;
		public string change;
		public int keywordLength;

		public ChangeType changeType;

		public int startIndex;
		public int endIndex;
		public string color;
	}

	public enum ChangeType {
		word,
		comment,
		quote,
		empty
	}

	//string variables = "<color=#8de043>", functions = "<color=#ac80a6>";
	
	string exeText = "";
	
	public Text code;
	public Text formattedCode;

	public string quoteColor = "<color=#e6db74>";
	public string commentColor = "<color=#75715e>";
	public string keywordColor = "<color=#a6e22d>";
	
	string lastExeText;
	
	public List<int> changedLineIndexes;

	//public string[] lastCodeLines = new string[30];
	public List<string> lastCodeLines = new List <string>();
	public string[] lastFormattedCodeLines;

	public List<KeywordMatch> testList;

	void Start () {
		Debug.Log ("TODO: Optimize code formatting.");
		Debug.Log("TODO: Custom match search that returns words within words.");
		Debug.Log ("TODO: Can the array search be optimized?");
		Debug.Log ("TODO: Initialize lastCodeLines with an empty value.");

		testList = GetKeywordMatches ("This is a test in");
	}



	void Update () {
		
		// Disable original code from view
		code.color = new Color (0, 0, 0, 0);
		
		exeText = code.text;
		
		if (exeText != lastExeText) {

			string[] codeLines = exeText.Split( '\n' );
			changedLineIndexes = GetChangedLines(codeLines);

			for (int i = 0; i < codeLines.Length; i++) {
				if (changedLineIndexes.Contains(i) == true || codeLines[i] == "" || lastFormattedCodeLines.Length <= i){
					codeLines[i] = FormatLine (codeLines[i]);
					//Debug.Log (i);
				} else {
					codeLines[i] = lastFormattedCodeLines [i];
				}
				//Debug.Log ("Formatted: " + codeLines[lineIndex]);
			}

			lastFormattedCodeLines = codeLines;

			string formattedcode;
			formattedcode = String.Join ("\n", codeLines);
			
			formattedCode.text = formattedcode;
			lastExeText = exeText;
		}
	}

	// Format the line by applying html to certain keyword matches
	string FormatLine (string codeLine) {

		// Check if the keywords are available. If not, return a simple message.
		if (KeywordAPI.APIKeywords == null) {
			return "Gathering Data";
		}
		// Set up references to keywords
		keywords = KeywordAPI.APIKeywords;

		// Create an array of changes to be made.
		CodeChange[] codeChanges = MakeCodeChangeArray(codeLine);
		
		// Apply the changes from the codeChanges array to the code.
		int offset = 0;	// Used to handle the offset of the index caused by inserting html.
		bool stopNext = false; // Used for alternating between coloring quotes
		foreach (CodeChange codechange in codeChanges) {
			
			// If the change is empty, skip it.
			if (codechange.changeType == ChangeType.empty) {
				continue;
			}

			// If the change is inside a comment, skip it
			bool stop = false;
			foreach (CodeChange cc in codeChanges) {
				if (codechange.startIndex > cc.startIndex && codechange.startIndex < cc.endIndex-1) {
					if (cc.changeType == ChangeType.comment) {
						stop = true;
					}
					if (cc.changeType == ChangeType.quote) {
						if (stopNext) {
							stop = true;
						}
					}
				}
				if (codechange.index > cc.startIndex && codechange.index < cc.endIndex) {
					//If the change is inside a comment
					if (cc.changeType == ChangeType.comment)
						stop = true;

					//If the change is a word and is inside a quote
					if (cc.changeType == ChangeType.quote && codechange.changeType == ChangeType.word)
						stop = true;
				}
				if (codechange.changeType == ChangeType.quote && codechange.startIndex > cc.startIndex && cc.changeType == ChangeType.comment) {
					stop = true;
				}
			}
			if (stop) {
				continue;
			}

			// If it's a comment, apply the correct html
			if (codechange.changeType == ChangeType.comment) {
				codeLine = codeLine.Insert(codechange.startIndex+offset, codechange.color);
				offset += codechange.color.Length;
				codeLine = codeLine.Insert(codechange.endIndex+offset, "</color>");
				offset += "</color>".Length;
				continue;
			}

			// If it's a quote, apply the correct html
			if (codechange.changeType == ChangeType.quote) {

				// ALternate between coloring quotes
				if (stopNext) {
					stopNext = false;
					continue;
				}	
				else {
					stopNext = true;
				}

				codeLine = codeLine.Insert(codechange.startIndex+offset, codechange.color);
				offset += codechange.color.Length;
				codeLine = codeLine.Insert(codechange.endIndex+offset, "</color>");
				offset += "</color>".Length;
				continue;
			}
			
			// If we've made it here we can safely replace the keyword to apply the html
			codeLine = codeLine.Remove(codechange.index+offset, codechange.keywordLength);
			codeLine = codeLine.Insert(codechange.index+offset, codechange.change);
			offset += codechange.change.Length - codechange.keywordLength;
		}

		return codeLine;
	}

	List<int> GetChangedLines (string[] codeLines) {

		// Create a list to store the indexes of the lines that have changed
		List <int> changedLineIndexes = new List<int>();

		// Loop through all the lines of code
		for (int i = 0; i < codeLines.Length; i++) {

			// If the line doesn't match the previous line
			if (codeLines[i] != lastCodeLines[i]) {
				// if new lines have been added, update the rest of the string
				if (codeLines.Length > lastCodeLines.Count) {
					while (i < codeLines.Length) {
						changedLineIndexes.Add (i);
						i++;
					}
				}
				// Add the line index to the changedLineIndexes list
				changedLineIndexes.Add (i);
			}

			// If new lines have been added, add them to the lastCodeLines list
			while (codeLines.Length > lastCodeLines.Count) {
				lastCodeLines.Add (codeLines[codeLines.Length-1]);
			}

		}

		lastCodeLines = codeLines.ToList();
		return changedLineIndexes;
	}

	// Returns an Array of changes to be made to the text.
	// Each element contains information about the change.
	CodeChange[] MakeCodeChangeArray (string codeFormatted) {
		// Create an empty Array of ChodeChanges with a length of the number of matches
		CodeChange[] codeChanges = new CodeChange[keywords.Matches(codeFormatted).Count];
		int matchNumber = 0;	// index used while looping through the matches

		// Fill in the array of changes to be made by looping through matches between the regex and the code.
		foreach (Match keywordMatch in keywords.Matches(codeFormatted))
		{
			// If the match is nothing, simply skip it
			if (keywordMatch.Value == "") {
				CodeChange emptychange = new CodeChange();
				emptychange.changeType = ChangeType.empty;
				codeChanges[matchNumber] = emptychange;
				
				matchNumber += 1; // increase the matchNumber
				continue;	// continue to next match
				//continue;
			}

			//Debug.Log (keywordMatch);

			// If the match is a comment sign, add a speciel kind of change to the array
			if (keywordMatch.Value == "//") {

				// create a CodeChange and apply it to the codeChanges array.
				CodeChange commentChange = new CodeChange();

				commentChange.startIndex = keywordMatch.Index;
				commentChange.endIndex = codeFormatted.Length;
				commentChange.color = commentColor;

				commentChange.changeType = ChangeType.comment;
				codeChanges[matchNumber] = commentChange;
				matchNumber += 1; // increase the matchNumber
				continue;	// continue to next match
			}

			// If the match is a quote sign, add a speciel kind of change to the array
			if (keywordMatch.Value == "\"") {

				// create a CodeChange and apply it to the codeChanges array.
				CodeChange quoteChange = new CodeChange();

				quoteChange.startIndex = keywordMatch.Index;
				quoteChange.endIndex = codeFormatted.Length;
				quoteChange.color = quoteColor;

				if (codeFormatted.Length > keywordMatch.Index+1) {
					string quote = codeFormatted.Substring (keywordMatch.Index+1, codeFormatted.Length-keywordMatch.Index-1);
					int closeSignIndex = quote.IndexOf("\"");
					if (closeSignIndex != -1) {
						quoteChange.endIndex = closeSignIndex + (codeFormatted.Length-quote.Length) + 1;
					}
					//Debug.Log(codeFormatted.Length-keywordMatch.Index-1);
				}

				quoteChange.changeType = ChangeType.quote;
				codeChanges[matchNumber] = quoteChange;
				matchNumber += 1; // increase the matchNumber
				continue;	// continue to next match
			}

			//CACHE THIS
			string allowedCharsBeforeKeyword = " .()\t=<>+-/*";

			// if the keyword is not first in the string.
			if (keywordMatch.Index >= 1) {
				char charBeforeKeyword = Convert.ToChar (codeFormatted.Substring (keywordMatch.Index-1, 1));
				// check if the character before the keyword is one of certain signs
				if (allowedCharsBeforeKeyword.Contains (charBeforeKeyword) == false) {
					// create an empty CodeChange and apply it to the codeChanges array.
					CodeChange emptychange = new CodeChange();
					emptychange.changeType = ChangeType.empty;
					codeChanges[matchNumber] = emptychange;
					
					matchNumber += 1; // increase the matchNumber
					continue;	// continue to next match
				}
			}

			//CACHE THIS
			string allowedCharsAfterKeyword = " ;.()\t[=<>+-/*";

			// if the keyword is not last in the string
			if (keywordMatch.Index + keywordMatch.Length < codeFormatted.Length) {
				char charAfterKeyword = Convert.ToChar (codeFormatted.Substring (keywordMatch.Index+keywordMatch.Length, 1));
				// check if the character after the keyword is one of cartain signs
				if (allowedCharsAfterKeyword.Contains (charAfterKeyword) == false) {
					// create an empty CodeChange and apply it to the codeChanges array.
					CodeChange emptychange = new CodeChange();
					emptychange.changeType = ChangeType.empty;
					codeChanges[matchNumber] = emptychange;
					
					matchNumber += 1; // increase the matchNumber
					continue;	// continue to next match
				}
			}
			
			// if we've made it here we can safely add a change to the array and fill in the information.
			CodeChange codechange = new CodeChange();

			string changecolor = keywordColor;

			// TODO: OPTIMIZE ARRAY SEARCH

			CustomKeyword[] customKeywords = KeywordAPI.CustomKeywords;
			if (customKeywords != null) {
				for (int i = 0; i < customKeywords.Length; i++) {

					string[] kws = customKeywords[i].keywords;

					for (int j = 0; j < kws.Length; j++) {
						if (kws[j] == keywordMatch.Value)
							changecolor = customKeywords[i].color;
					}
				}
			}

			codechange.change =  changecolor + keywordMatch.Value + "</color>";
			codechange.index = keywordMatch.Index;
			codechange.keywordLength = keywordMatch.Length;
			
			codeChanges[matchNumber] = codechange; // apply the change to the array
			matchNumber += 1; // increment the matchNumber
		}

		return codeChanges;
	}

	List<KeywordMatch> GetKeywordMatches (string line) {
		List<KeywordMatch> matches = new List<KeywordMatch>();
		string[] kws = KeywordAPI.APIKeywordsArray;

		for (int i = 0; i < kws.Length; i++) {
			string lineInstance = line;
			int lineOffset = 0;

			// int secure = 0;
			// while (lineInstance.IndexOf (kws[i]) != -1) {

			// 	KeywordMatch kwm = new KeywordMatch();
			// 	kwm.Value = kws[i];
			// 	kwm.Index = FIND + lineOffset;
			// 	kwm.Length = kws[i].Length;

			// 	lineOffset += FIND + kws[i].Length;
			// 	lineInstance = lineInstance.Substring (0, FIND + kws[i].Length);

			// 	matches.Add (kwm);

			// 	Debug.Log (lineInstance);

			// 	secure += 1;
			// 	if (secure > 100) {
			// 		Debug.LogError ("BROKE");
			// 		break;
			// 	}
			// }

			//Return indexes of matches
		}

		return matches;
	}
}