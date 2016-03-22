//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;

namespace MaterialUI
{
	public class VectorImageParserOcticons : VectorImageFontParser
	{
		protected override string GetIconFontUrl()
		{
			return "https://github.com/github/octicons/blob/master/octicons/octicons.ttf?raw=true";
		}

		protected override string GetIconFontLicenseUrl()
	    {
			return "https://github.com/github/octicons/blob/master/LICENSE.txt?raw=true";
	    }
		
		protected override string GetIconFontDataUrl()
		{
			return "https://github.com/github/octicons/raw/master/octicons/octicons.css?raw=true";
		}
		
		public override string GetWebsite()
		{
			return "https://octicons.github.com/";
		}
		
		public override string GetFontName()
		{
			return "Octicons";
		}
		
		protected override VectorImageSet GenerateIconSet(string fontDataContent)
		{
			VectorImageSet vectorImageSet = new VectorImageSet();
			
			foreach (string line in fontDataContent.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
			{
				if (line.StartsWith(".octicon-") && line.Contains("content:"))
				{
					string name = line.Replace(".octicon-", string.Empty).Trim();
					name = name.Substring(0, name.IndexOf(":before")).Trim();

					string unicode = line.Substring(line.IndexOf("content: '") + 11);
					unicode = unicode.Substring(0, unicode.IndexOf("'}"));

					Glyph glyph = new Glyph(name, unicode, false);
					vectorImageSet.iconGlyphList.Add(glyph);
				}
			}

            return vectorImageSet;
		}

		protected override string ExtractLicense(string fontDataLicenseContent)
		{
			return fontDataLicenseContent;
		}
	}
}
