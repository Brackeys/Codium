//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;

namespace MaterialUI
{
	public class VectorImageParserCommunityMD : VectorImageFontParser
	{
		protected override string GetIconFontUrl()
		{
			return "https://github.com/Templarian/MaterialDesign-Webfont/blob/master/fonts/materialdesignicons-webfont.ttf?raw=true";
		}

		protected override string GetIconFontLicenseUrl()
	    {
			return "https://raw.githubusercontent.com/Templarian/MaterialDesign-Webfont/master/license.txt";
	    }
		
		protected override string GetIconFontDataUrl()
		{
			return "https://raw.githubusercontent.com/Templarian/MaterialDesign-Webfont/master/scss/_icons.scss";
		}
		
		public override string GetWebsite()
		{
			return "https://materialdesignicons.com/";
		}
		
		public override string GetFontName()
		{
			return "CommunityMD";
		}
		
		protected override VectorImageSet GenerateIconSet(string fontDataContent)
		{
			VectorImageSet vectorImageSet = new VectorImageSet();

		    string[] splitContent = fontDataContent.Split(new [] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

		    string[] lineOne = splitContent[0].Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries);
		    string[] lineTwo = splitContent[1].Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries);

		    for (int i = 1; i < lineOne.Length; i++)
		    {
		        vectorImageSet.iconGlyphList.Add(new Glyph(lineTwo[i].Replace("'", ""), lineOne[i].Replace("'", ""), false));
            }

            return vectorImageSet;
		}

		protected override string ExtractLicense(string fontDataLicenseContent)
		{
			fontDataLicenseContent = fontDataLicenseContent.Substring(fontDataLicenseContent.IndexOf("License"));
			return fontDataLicenseContent;
		}
	}
}
