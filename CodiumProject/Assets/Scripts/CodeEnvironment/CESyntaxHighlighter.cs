//-----------------------------------------------------------------
// Formats the code by applying syntax highlighting to keywords, quotes and comments.
// Keywords are applied by using an array of keyword collections: regexes and colors
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;

namespace CodeEnvironment
{
	// A collection stores a regex with a bunch of keywords
	// and a color for those keywords in hex format
	public class KeywordCollection
	{
		public Regex regex;
		public string color;

		public KeywordCollection(string _color, Regex _regex)
		{
			color = _color;
			regex = _regex;
		}
	}

	public class CESyntaxHighlighter
	{

		private string _commentCssClass;
		private string _keywordCssClass;
		private string _quotesCssClass;
		private string _typeCssClass;
		private string _numberCssClass;
		private string _operatorCssClass;

		// HUGE master regex containing all keywords
		private KeywordCollection[] _collections;
		public KeywordCollection[] Collections
		{
			get { return _collections; }
		}

		// Gets or sets the CSS class used for comments.
		public string CommentCssClass
		{
			get { return _commentCssClass; }
			set { _commentCssClass = value; }
		}

		// Gets or sets the CSS class used for string quotes.
		public string QuotesCssClass
		{
			get { return _quotesCssClass; }
			set { _quotesCssClass = value; }
		}

		// Gets or sets the CSS class used for types.
		public string TypeCssClass
		{
			get { return _typeCssClass; }
			set { _typeCssClass = value; }
		}

		// Gets or sets the CSS class used for numbers.
		public string NumberCssClass
		{
			get { return _numberCssClass; }
			set { _numberCssClass = value; }
		}

		// Gets or sets the CSS class used for operators.
		public string OperatorCssClass
		{
			get { return _operatorCssClass; }
			set { _operatorCssClass = value; }
		}

		// Initializes a new instance of the SyntaxHighlighter class.
		public CESyntaxHighlighter()
		{
			_commentCssClass = "grey";
			_quotesCssClass = "#e6db74";
			_typeCssClass = "#a6e22d";
			_numberCssClass = "#ae81ff";
			_operatorCssClass = "#f92772";
		}

		// Adds keywords to the color dictionary and the keyword regex by supplying a txt file array
		public void AddKeywords(TextAsset[] txtFiles)
		{
			_collections = new KeywordCollection[txtFiles.Length];

			int i = 0;
			foreach (TextAsset _txt in txtFiles)
			{
				string[] kws = _txt.text.Split('\n');	//Create a string array of lines

				//Extract a color from the first line in the txt file
				string _color = kws[0].Trim();

				string _keywordRegexPattern = "";
				bool first = true;
				//Loop through the keywords and add them (skip the color)
				foreach (string _kw in kws)
				{
					if (_kw.Trim() == "")
					{
						continue;
					}
					string _keyword = _kw.Trim();

					if (first)
					{
						_keywordRegexPattern += "(";
						first = false;
					}

					_keywordRegexPattern += @"(?<=[(|\s])" + _keyword + "|^" + _keyword + "|";
				}
				_keywordRegexPattern = _keywordRegexPattern.Remove(_keywordRegexPattern.Length - 1, 1);
				_keywordRegexPattern += @")(\W>|\W&gt;|\W\s|\W\n|\W;|\W<|\W)";

				Regex _regex = new Regex(_keywordRegexPattern, RegexOptions.Singleline);

				KeywordCollection _collection = new KeywordCollection(_color, _regex);
				_collections[i] = _collection;

				i++;
			}
		}

		// Highlights the specified source code and returns it as stylised HTML.
		public string Highlight(string source)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(HighlightSource(source + " "));

			return builder.ToString();
		}

		private string pContent = "";
		// Formats the code (content)
		protected virtual string HighlightSource(string content)
		{
			if (string.IsNullOrEmpty(CommentCssClass))
				Debug.LogError("The CommentCssClass should not be null or empty");
			if (string.IsNullOrEmpty(QuotesCssClass))
				Debug.LogError("The CommentCssClass should not be null or empty");
			if (string.IsNullOrEmpty(TypeCssClass))
				Debug.LogError("The TypeCssClass should not be null or empty");

			// Some fairly secure token placeholders
			const string COMMENTS_TOKEN = "`````";
			const string MULTILINECOMMENTS_TOKEN = "~~~~~";
			const string QUOTES_TOKEN = "Â¬Â¬Â¬Â¬Â¬";

			// Remove /* */ quotes, taken from ostermiller.org
			Regex regex = new Regex(@"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/", RegexOptions.Singleline);
			List<string> multiLineComments = new List<string>();
			if (regex.IsMatch(content))
			{
				foreach (Match item in regex.Matches(content))
				{
					if (!multiLineComments.Contains(item.Value))
						multiLineComments.Add(item.Value);
				}
			}

			for (int i = 0; i < multiLineComments.Count; i++)
			{
				content = content.ReplaceToken(multiLineComments[i], MULTILINECOMMENTS_TOKEN, i);
			}

			// Remove the quotes first, so they don't get highlighted
			List<string> quotes = new List<string>();
			bool onEscape = false;
			bool onComment1 = false;
			bool onComment2 = false;
			bool inQuotes = false;
			int start = -1;
			for (int i = 0; i < content.Length; i++)
			{
				if (content[i] == '/' && !inQuotes && !onComment1)
					onComment1 = true;
				else if (content[i] == '/' && !inQuotes && onComment1)
					onComment2 = true;
				else if (content[i] == '"' && !onEscape && !onComment2)
				{
					inQuotes = true; // stops cases of: var s = "// I'm a comment";
					if (start > -1)
					{
						string quote = content.Substring(start, i - start + 1);
						if (!quotes.Contains(quote))
							quotes.Add(quote);
						start = -1;
						inQuotes = false;
					}
					else
					{
						start = i;
					}
				}
				else if (content[i] == '\\' || content[i] == '\'')
				{
					onEscape = true;
				}
				else if (content[i] == '\n')
				{
					onComment1 = false;
					onComment2 = false;
				}
				else
				{
					onEscape = false;
				}
			}

			for (int i = 0; i < quotes.Count; i++)
			{
				content = content.ReplaceToken(quotes[i], QUOTES_TOKEN, i);
			}

			// Remove the comments next, so they don't get highlighted
			regex = new Regex("(/{2,3}.+)", RegexOptions.Multiline);
			List<string> comments = new List<string>();
			if (regex.IsMatch(content))
			{
				foreach (Match item in regex.Matches(content))
				{
					if (!comments.Contains(item.Value + "\n"))
						comments.Add(item.Value);
				}
			}

			for (int i = 0; i < comments.Count; i++)
			{
				content = content.ReplaceToken(comments[i], COMMENTS_TOKEN, i);
			}

			// Highlight single quotes
			content = Regex.Replace(content, "('.{1,2}')", CssExtensions.GetCssReplacement(QuotesCssClass), RegexOptions.Singleline);

			// Highlight class names based on the logic: {space OR start of line OR >}{1 capital){alphanumeric}{space}
			regex = new Regex(@"((?:\s|^)[A-Z]\w+(?:\s))", RegexOptions.Singleline);
			List<string> highlightedClasses = new List<string>();
			if (regex.IsMatch(content))
			{
				foreach (Match item in regex.Matches(content))
				{
					string val = item.Groups[1].Value;
					if (!highlightedClasses.Contains(val))
						highlightedClasses.Add(val);
				}
			}

			for (int i = 0; i < highlightedClasses.Count; i++)
			{
				content = content.ReplaceWithCss(highlightedClasses[i].Trim(), TypeCssClass);
			}

			// Pass 2. Doing it in N passes due to my inferior regex knowledge of back/forwardtracking.
			// This does {space or [}{1 capital){alphanumeric}{]}
			regex = new Regex(@"(?:\s|\[)([A-Z]\w+)(?:\])", RegexOptions.Singleline);
			highlightedClasses = new List<string>();
			if (regex.IsMatch(content))
			{
				foreach (Match item in regex.Matches(content))
				{
					string val = item.Groups[1].Value;
					if (!highlightedClasses.Contains(val))
						highlightedClasses.Add(val);
				}
			}

			for (int i = 0; i < highlightedClasses.Count; i++)
			{
				content = content.ReplaceWithCss(highlightedClasses[i], TypeCssClass);
			}

			// Pass 3. Generics
			regex = new Regex(@"(?:\s|\[|\()([A-Z]\w+(?:<|&lt;))", RegexOptions.Singleline);
			highlightedClasses = new List<string>();
			if (regex.IsMatch(content))
			{
				foreach (Match item in regex.Matches(content))
				{
					string val = item.Groups[1].Value;
					if (!highlightedClasses.Contains(val))
						highlightedClasses.Add(val);
				}
			}

			for (int i = 0; i < highlightedClasses.Count; i++)
			{
				string val = highlightedClasses[i];
				val = val.Replace("<", "").Replace("&lt;", "");
				content = content.ReplaceWithCss(highlightedClasses[i], val, "&lt;", TypeCssClass);
			}

			// Pass 4. new keyword with a type
			regex = new Regex(@"new\s+([A-Z]\w+)(?:\()", RegexOptions.Singleline);
			highlightedClasses = new List<string>();
			if (regex.IsMatch(content))
			{
				foreach (Match item in regex.Matches(content))
				{
					string val = item.Groups[1].Value;
					if (!highlightedClasses.Contains(val))
						highlightedClasses.Add(val);
				}
			}

			// Replace the highlighted classes
			for (int i = 0; i < highlightedClasses.Count; i++)
			{
				content = content.ReplaceWithCss(highlightedClasses[i], TypeCssClass);
			}

			// Highlight only changed lines with keywords
			string[] lines = content.Split('\n');
			string[] pLines = pContent.Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i] == string.Empty)
					continue;

				if (i > pLines.Length - 1)
				{
					content = content.Replace(lines[i], HighlightKeywords(lines[i]));
					continue;
				}

				if (lines[i] != pLines[i])
				{
					//Debug.Log(lines[i]);
					content = content.Replace(lines[i], HighlightKeywords(lines[i]));
				}

			}

			pContent = content;

			// Shove the multiline comments back in
			for (int i = 0; i < multiLineComments.Count; i++)
			{
				content = content.ReplaceTokenWithCss(multiLineComments[i], MULTILINECOMMENTS_TOKEN, i, CommentCssClass);
			}

			// Shove the quotes back in
			for (int i = 0; i < quotes.Count; i++)
			{
				content = content.ReplaceTokenWithCss(quotes[i], QUOTES_TOKEN, i, QuotesCssClass);
			}

			// Shove the single line comments back in
			for (int i = 0; i < comments.Count; i++)
			{
				string comment = comments[i];
				// Add quotes back in
				for (int n = 0; n < quotes.Count; n++)
				{
					comment = comment.Replace(string.Format("{0}{1}{0}", QUOTES_TOKEN, n), quotes[n]);
				}
				content = content.ReplaceTokenWithCss(comment, COMMENTS_TOKEN, i, CommentCssClass);
			}

			return content;
		}

		private string HighlightKeywords(string _line)
		{
			//string _operators = "+|-|?|/|*|=|!";

			// Highlight numbers
			//Regex numberRegex = new Regex(@"((?<=[" + _operators + @"|\s|(|0-9|.])([\d|f|.])|^([\d|f|.]))(?=[" + _operators + @"|\s|;|)|0-9|.|f])", RegexOptions.Singleline);
			//_line = numberRegex.Replace(_line, "<color=" + NumberCssClass + ">$1</color>");

			// Highlight operators
			//Regex operatorRegex = new Regex(@"(?<=[\w|\s|" + _operators + @"])([" + _operators + @"])(?=[\s|" + _operators + @"|)|;])", RegexOptions.Singleline);
			//content = operatorRegex.Replace(content, "<color=" + OperatorCssClass + ">$1</color>");

			// Loop through the keyword collections
			foreach (KeywordCollection _collection in Collections)
			{
				_line = _collection.regex.Replace(_line, CssExtensions.GetCssReplacement(_collection.color) + "$2");
			}

			return _line;

		}

	}

	// This class hosts some convenience methods for replacing keywords with CSS
	public static class CssExtensions
	{
		public static string GetCssReplacement(string cssCLass)
		{
			return "<color=" + cssCLass + ">" + "$1</color>";
		}


		public static string ReplaceWithCss(this string content, string source, string cssClass)
		{
			return content.Replace(source, string.Format("<color=\"{0}\">{1}</color>", cssClass, source));
		}

		public static string ReplaceWithCss(this string content, string source, string replacement, string cssClass)
		{
			return content.Replace(source, string.Format("<color=\"{0}\">{1}</color>", cssClass, replacement));
		}

		public static string ReplaceWithCss(this string content, string source, string replacement, string suffix, string cssClass)
		{
			return content.Replace(source, string.Format("<color=\"{0}\">{1}</color>{2}", cssClass, replacement, suffix));
		}

		public static string ReplaceTokenWithCss(this string content, string source, string token, int counter, string cssClass)
		{
			string formattedToken = String.Format("{0}{1}{0}", token, counter);
			return content.Replace(formattedToken, string.Format("<color=\"{0}\">{1}</color>", cssClass, source));
		}

		public static string ReplaceToken(this string content, string source, string token, int counter)
		{
			string formattedToken = String.Format("{0}{1}{0}", token, counter);
			return content.Replace(source, formattedToken);
		}
	}

}
