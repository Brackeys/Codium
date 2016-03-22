//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Globalization;
using System.Text.RegularExpressions;

namespace MaterialUI
{
    public static class IconDecoder
    {
        private static Regex m_RegExpression = new Regex(@"\\u(?<Value>[a-zA-Z0-9]{4})");

        public static string Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            return m_RegExpression.Replace(value, m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
        }
    }
}