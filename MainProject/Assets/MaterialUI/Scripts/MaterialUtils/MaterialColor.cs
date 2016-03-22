//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;

namespace MaterialUI
{
    public class MaterialColor : MonoBehaviour
    {
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex, float alpha = 1f)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, (byte)Mathf.RoundToInt(alpha * 255f));
        }

        public static Color GetHighlightColor(Color baseColor, Color baseHighlightColor, float blendAmount = 0.2f)
        {
            if (baseColor.a == 0)
            {
                baseHighlightColor.a = blendAmount;
            }
            else
            {
                baseHighlightColor.r = Tween.Linear(baseColor.r, baseHighlightColor.r, blendAmount, 1f);
                baseHighlightColor.g = Tween.Linear(baseColor.g, baseHighlightColor.g, blendAmount, 1f);
                baseHighlightColor.b = Tween.Linear(baseColor.b, baseHighlightColor.b, blendAmount, 1f);
                baseHighlightColor.a = Tween.Linear(baseColor.a, baseHighlightColor.a, blendAmount, 1f);
            }

            return baseHighlightColor;
        }

        public static Color Random500()
        {
            Color[] colors =
            {
                red500,
                pink500,
                purple500,
                deepPurple500,
                indigo500,
                blue500,
                lightBlue500,
                cyan500,
                teal500,
                green500,
                lightGreen500,
                lime500,
                yellow500,
                amber500,
                orange500,
                deepOrange500,
                brown500,
                blueGrey500
            };

            return colors[Random.Range(0, colors.Length)];
        }

        #region colors

        public static Color red50 { get { return HexToColor("FFEBEE"); } }
        public static Color red100 { get { return HexToColor("FFCDD2"); } }
        public static Color red200 { get { return HexToColor("EF9A9A"); } }
        public static Color red300 { get { return HexToColor("E57373"); } }
        public static Color red400 { get { return HexToColor("EF5350"); } }
        public static Color red500 { get { return HexToColor("F44336"); } }
        public static Color red600 { get { return HexToColor("E53935"); } }
        public static Color red700 { get { return HexToColor("D32F2F"); } }
        public static Color red800 { get { return HexToColor("C62828"); } }
        public static Color red900 { get { return HexToColor("B71C1C"); } }
        public static Color redA100 { get { return HexToColor("FF8A80"); } }
        public static Color redA200 { get { return HexToColor("FF5252"); } }
        public static Color redA400 { get { return HexToColor("FF1744"); } }
        public static Color redA700 { get { return HexToColor("D50000"); } }

        public static Color pink50 { get { return HexToColor("FCE4EC"); } }
        public static Color pink100 { get { return HexToColor("F8BBD0"); } }
        public static Color pink200 { get { return HexToColor("F48FB1"); } }
        public static Color pink300 { get { return HexToColor("F06292"); } }
        public static Color pink400 { get { return HexToColor("EC407A"); } }
        public static Color pink500 { get { return HexToColor("E91E63"); } }
        public static Color pink600 { get { return HexToColor("D81B60"); } }
        public static Color pink700 { get { return HexToColor("C2185B"); } }
        public static Color pink800 { get { return HexToColor("AD1457"); } }
        public static Color pink900 { get { return HexToColor("880E4F"); } }
        public static Color pinkA100 { get { return HexToColor("FF80AB"); } }
        public static Color pinkA200 { get { return HexToColor("FF4081"); } }
        public static Color pinkA400 { get { return HexToColor("F50057"); } }
        public static Color pinkA700 { get { return HexToColor("C51162"); } }

        public static Color purple50 { get { return HexToColor("F3E5F5"); } }
        public static Color purple100 { get { return HexToColor("E1BEE7"); } }
        public static Color purple200 { get { return HexToColor("CE93D8"); } }
        public static Color purple300 { get { return HexToColor("BA68C8"); } }
        public static Color purple400 { get { return HexToColor("AB47BC"); } }
        public static Color purple500 { get { return HexToColor("9C27B0"); } }
        public static Color purple600 { get { return HexToColor("8E24AA"); } }
        public static Color purple700 { get { return HexToColor("7B1FA2"); } }
        public static Color purple800 { get { return HexToColor("6A1B9A"); } }
        public static Color purple900 { get { return HexToColor("4A148C"); } }
        public static Color purpleA100 { get { return HexToColor("EA80FC"); } }
        public static Color purpleA200 { get { return HexToColor("E040FB"); } }
        public static Color purpleA400 { get { return HexToColor("D500F9"); } }
        public static Color purpleA700 { get { return HexToColor("AA00FF"); } }

        public static Color deepPurple50 { get { return HexToColor("EDE7F6"); } }
        public static Color deepPurple100 { get { return HexToColor("D1C4E9"); } }
        public static Color deepPurple200 { get { return HexToColor("B39DDB"); } }
        public static Color deepPurple300 { get { return HexToColor("9575CD"); } }
        public static Color deepPurple400 { get { return HexToColor("7E57C2"); } }
        public static Color deepPurple500 { get { return HexToColor("673AB7"); } }
        public static Color deepPurple600 { get { return HexToColor("5E35B1"); } }
        public static Color deepPurple700 { get { return HexToColor("512DA8"); } }
        public static Color deepPurple800 { get { return HexToColor("4527A0"); } }
        public static Color deepPurple900 { get { return HexToColor("311B92"); } }
        public static Color deepPurpleA100 { get { return HexToColor("B388FF"); } }
        public static Color deepPurpleA200 { get { return HexToColor("7C4DFF"); } }
        public static Color deepPurpleA400 { get { return HexToColor("651FFF"); } }
        public static Color deepPurpleA700 { get { return HexToColor("6200EA"); } }

        public static Color indigo50 { get { return HexToColor("E8EAF6"); } }
        public static Color indigo100 { get { return HexToColor("C5CAE9"); } }
        public static Color indigo200 { get { return HexToColor("9FA8DA"); } }
        public static Color indigo300 { get { return HexToColor("7986CB"); } }
        public static Color indigo400 { get { return HexToColor("5C6BC0"); } }
        public static Color indigo500 { get { return HexToColor("3F51B5"); } }
        public static Color indigo600 { get { return HexToColor("3949AB"); } }
        public static Color indigo700 { get { return HexToColor("303F9F"); } }
        public static Color indigo800 { get { return HexToColor("283593"); } }
        public static Color indigo900 { get { return HexToColor("1A237E"); } }
        public static Color indigoA100 { get { return HexToColor("8C9EFF"); } }
        public static Color indigoA200 { get { return HexToColor("536DFE"); } }
        public static Color indigoA400 { get { return HexToColor("3D5AFE"); } }
        public static Color indigoA700 { get { return HexToColor("304FFE"); } }

        public static Color blue50 { get { return HexToColor("E3F2FD"); } }
        public static Color blue100 { get { return HexToColor("BBDEFB"); } }
        public static Color blue200 { get { return HexToColor("90CAF9"); } }
        public static Color blue300 { get { return HexToColor("64B5F6"); } }
        public static Color blue400 { get { return HexToColor("42A5F5"); } }
        public static Color blue500 { get { return HexToColor("2196F3"); } }
        public static Color blue600 { get { return HexToColor("1E88E5"); } }
        public static Color blue700 { get { return HexToColor("1976D2"); } }
        public static Color blue800 { get { return HexToColor("1565C0"); } }
        public static Color blue900 { get { return HexToColor("0D47A1"); } }
        public static Color blueA100 { get { return HexToColor("82B1FF"); } }
        public static Color blueA200 { get { return HexToColor("448AFF"); } }
        public static Color blueA400 { get { return HexToColor("2979FF"); } }
        public static Color blueA700 { get { return HexToColor("2962FF"); } }

        public static Color lightBlue50 { get { return HexToColor("E1F5FE"); } }
        public static Color lightBlue100 { get { return HexToColor("B3E5FC"); } }
        public static Color lightBlue200 { get { return HexToColor("81D4FA"); } }
        public static Color lightBlue300 { get { return HexToColor("4FC3F7"); } }
        public static Color lightBlue400 { get { return HexToColor("29B6F6"); } }
        public static Color lightBlue500 { get { return HexToColor("03A9F4"); } }
        public static Color lightBlue600 { get { return HexToColor("039BE5"); } }
        public static Color lightBlue700 { get { return HexToColor("0288D1"); } }
        public static Color lightBlue800 { get { return HexToColor("0277BD"); } }
        public static Color lightBlue900 { get { return HexToColor("01579B"); } }
        public static Color lightBlueA100 { get { return HexToColor("80D8FF"); } }
        public static Color lightBlueA200 { get { return HexToColor("40C4FF"); } }
        public static Color lightBlueA400 { get { return HexToColor("00B0FF"); } }
        public static Color lightBlueA700 { get { return HexToColor("0091EA"); } }

        public static Color cyan50 { get { return HexToColor("E0F7FA"); } }
        public static Color cyan100 { get { return HexToColor("B2EBF2"); } }
        public static Color cyan200 { get { return HexToColor("80DEEA"); } }
        public static Color cyan300 { get { return HexToColor("4DD0E1"); } }
        public static Color cyan400 { get { return HexToColor("26C6DA"); } }
        public static Color cyan500 { get { return HexToColor("00BCD4"); } }
        public static Color cyan600 { get { return HexToColor("00ACC1"); } }
        public static Color cyan700 { get { return HexToColor("0097A7"); } }
        public static Color cyan800 { get { return HexToColor("00838F"); } }
        public static Color cyan900 { get { return HexToColor("006064"); } }
        public static Color cyanA100 { get { return HexToColor("84FFFF"); } }
        public static Color cyanA200 { get { return HexToColor("18FFFF"); } }
        public static Color cyanA400 { get { return HexToColor("00E5FF"); } }
        public static Color cyanA700 { get { return HexToColor("00B8D4"); } }

        public static Color teal50 { get { return HexToColor("E0F2F1"); } }
        public static Color teal100 { get { return HexToColor("B2DFDB"); } }
        public static Color teal200 { get { return HexToColor("80CBC4"); } }
        public static Color teal300 { get { return HexToColor("4DB6AC"); } }
        public static Color teal400 { get { return HexToColor("26A69A"); } }
        public static Color teal500 { get { return HexToColor("009688"); } }
        public static Color teal600 { get { return HexToColor("00897B"); } }
        public static Color teal700 { get { return HexToColor("00796B"); } }
        public static Color teal800 { get { return HexToColor("00695C"); } }
        public static Color teal900 { get { return HexToColor("004D40"); } }
        public static Color tealA100 { get { return HexToColor("A7FFEB"); } }
        public static Color tealA200 { get { return HexToColor("64FFDA"); } }
        public static Color tealA400 { get { return HexToColor("1DE9B6"); } }
        public static Color tealA700 { get { return HexToColor("00BFA5"); } }

        public static Color green50 { get { return HexToColor("E8F5E9"); } }
        public static Color green100 { get { return HexToColor("C8E6C9"); } }
        public static Color green200 { get { return HexToColor("A5D6A7"); } }
        public static Color green300 { get { return HexToColor("81C784"); } }
        public static Color green400 { get { return HexToColor("66BB6A"); } }
        public static Color green500 { get { return HexToColor("4CAF50"); } }
        public static Color green600 { get { return HexToColor("43A047"); } }
        public static Color green700 { get { return HexToColor("388E3C"); } }
        public static Color green800 { get { return HexToColor("2E7D32"); } }
        public static Color green900 { get { return HexToColor("1B5E20"); } }
        public static Color greenA100 { get { return HexToColor("B9F6CA"); } }
        public static Color greenA200 { get { return HexToColor("69F0AE"); } }
        public static Color greenA400 { get { return HexToColor("00E676"); } }
        public static Color greenA700 { get { return HexToColor("00C853"); } }

        public static Color lightGreen50 { get { return HexToColor("F1F8E9"); } }
        public static Color lightGreen100 { get { return HexToColor("DCEDC8"); } }
        public static Color lightGreen200 { get { return HexToColor("C5E1A5"); } }
        public static Color lightGreen300 { get { return HexToColor("AED581"); } }
        public static Color lightGreen400 { get { return HexToColor("9CCC65"); } }
        public static Color lightGreen500 { get { return HexToColor("8BC34A"); } }
        public static Color lightGreen600 { get { return HexToColor("7CB342"); } }
        public static Color lightGreen700 { get { return HexToColor("689F38"); } }
        public static Color lightGreen800 { get { return HexToColor("558B2F"); } }
        public static Color lightGreen900 { get { return HexToColor("33691E"); } }
        public static Color lightGreenA100 { get { return HexToColor("CCFF90"); } }
        public static Color lightGreenA200 { get { return HexToColor("B2FF59"); } }
        public static Color lightGreenA400 { get { return HexToColor("76FF03"); } }
        public static Color lightGreenA700 { get { return HexToColor("64DD17"); } }

        public static Color lime50 { get { return HexToColor("F9FBE7"); } }
        public static Color lime100 { get { return HexToColor("F0F4C3"); } }
        public static Color lime200 { get { return HexToColor("E6EE9C"); } }
        public static Color lime300 { get { return HexToColor("DCE775"); } }
        public static Color lime400 { get { return HexToColor("D4E157"); } }
        public static Color lime500 { get { return HexToColor("CDDC39"); } }
        public static Color lime600 { get { return HexToColor("C0CA33"); } }
        public static Color lime700 { get { return HexToColor("AFB42B"); } }
        public static Color lime800 { get { return HexToColor("9E9D24"); } }
        public static Color lime900 { get { return HexToColor("827717"); } }
        public static Color limeA100 { get { return HexToColor("F4FF81"); } }
        public static Color limeA200 { get { return HexToColor("EEFF41"); } }
        public static Color limeA400 { get { return HexToColor("C6FF00"); } }
        public static Color limeA700 { get { return HexToColor("AEEA00"); } }

        public static Color yellow50 { get { return HexToColor("FFFDE7"); } }
        public static Color yellow100 { get { return HexToColor("FFF9C4"); } }
        public static Color yellow200 { get { return HexToColor("FFF59D"); } }
        public static Color yellow300 { get { return HexToColor("FFF176"); } }
        public static Color yellow400 { get { return HexToColor("FFEE58"); } }
        public static Color yellow500 { get { return HexToColor("FFEB3B"); } }
        public static Color yellow600 { get { return HexToColor("FDD835"); } }
        public static Color yellow700 { get { return HexToColor("FBC02D"); } }
        public static Color yellow800 { get { return HexToColor("F9A825"); } }
        public static Color yellow900 { get { return HexToColor("F57F17"); } }
        public static Color yellowA100 { get { return HexToColor("FFFF8D"); } }
        public static Color yellowA200 { get { return HexToColor("FFFF00"); } }
        public static Color yellowA400 { get { return HexToColor("FFEA00"); } }
        public static Color yellowA700 { get { return HexToColor("FFD600"); } }

        public static Color amber50 { get { return HexToColor("FFF8E1"); } }
        public static Color amber100 { get { return HexToColor("FFECB3"); } }
        public static Color amber200 { get { return HexToColor("FFE082"); } }
        public static Color amber300 { get { return HexToColor("FFD54F"); } }
        public static Color amber400 { get { return HexToColor("FFCA28"); } }
        public static Color amber500 { get { return HexToColor("FFC107"); } }
        public static Color amber600 { get { return HexToColor("FFB300"); } }
        public static Color amber700 { get { return HexToColor("FFA000"); } }
        public static Color amber800 { get { return HexToColor("FF8F00"); } }
        public static Color amber900 { get { return HexToColor("FF6F00"); } }
        public static Color amberA100 { get { return HexToColor("FFE57F"); } }
        public static Color amberA200 { get { return HexToColor("FFD740"); } }
        public static Color amberA400 { get { return HexToColor("FFC400"); } }
        public static Color amberA700 { get { return HexToColor("FFAB00"); } }

        public static Color orange50 { get { return HexToColor("FFF3E0"); } }
        public static Color orange100 { get { return HexToColor("FFE0B2"); } }
        public static Color orange200 { get { return HexToColor("FFCC80"); } }
        public static Color orange300 { get { return HexToColor("FFB74D"); } }
        public static Color orange400 { get { return HexToColor("FFA726"); } }
        public static Color orange500 { get { return HexToColor("FF9800"); } }
        public static Color orange600 { get { return HexToColor("FB8C00"); } }
        public static Color orange700 { get { return HexToColor("F57C00"); } }
        public static Color orange800 { get { return HexToColor("EF6C00"); } }
        public static Color orange900 { get { return HexToColor("E65100"); } }
        public static Color orangeA100 { get { return HexToColor("FFD180"); } }
        public static Color orangeA200 { get { return HexToColor("FFAB40"); } }
        public static Color orangeA400 { get { return HexToColor("FF9100"); } }
        public static Color orangeA700 { get { return HexToColor("FF6D00"); } }

        public static Color deepOrange50 { get { return HexToColor("FBE9E7"); } }
        public static Color deepOrange100 { get { return HexToColor("FFCCBC"); } }
        public static Color deepOrange200 { get { return HexToColor("FFAB91"); } }
        public static Color deepOrange300 { get { return HexToColor("FF8A65"); } }
        public static Color deepOrange400 { get { return HexToColor("FF7043"); } }
        public static Color deepOrange500 { get { return HexToColor("FF5722"); } }
        public static Color deepOrange600 { get { return HexToColor("F4511E"); } }
        public static Color deepOrange700 { get { return HexToColor("E64A19"); } }
        public static Color deepOrange800 { get { return HexToColor("D84315"); } }
        public static Color deepOrange900 { get { return HexToColor("BF360C"); } }
        public static Color deepOrangeA100 { get { return HexToColor("FF9E80"); } }
        public static Color deepOrangeA200 { get { return HexToColor("FF6E40"); } }
        public static Color deepOrangeA400 { get { return HexToColor("FF3D00"); } }
        public static Color deepOrangeA700 { get { return HexToColor("DD2C00"); } }

        public static Color brown50 { get { return HexToColor("EFEBE9"); } }
        public static Color brown100 { get { return HexToColor("D7CCC8"); } }
        public static Color brown200 { get { return HexToColor("BCAAA4"); } }
        public static Color brown300 { get { return HexToColor("A1887F"); } }
        public static Color brown400 { get { return HexToColor("8D6E63"); } }
        public static Color brown500 { get { return HexToColor("795548"); } }
        public static Color brown600 { get { return HexToColor("6D4C41"); } }
        public static Color brown700 { get { return HexToColor("5D4037"); } }
        public static Color brown800 { get { return HexToColor("4E342E"); } }
        public static Color brown900 { get { return HexToColor("3E2723"); } }

        public static Color blueGrey50 { get { return HexToColor("ECEFF1"); } }
        public static Color blueGrey100 { get { return HexToColor("CFD8DC"); } }
        public static Color blueGrey200 { get { return HexToColor("B0BEC5"); } }
        public static Color blueGrey300 { get { return HexToColor("90A4AE"); } }
        public static Color blueGrey400 { get { return HexToColor("78909C"); } }
        public static Color blueGrey500 { get { return HexToColor("607D8B"); } }
        public static Color blueGrey600 { get { return HexToColor("546E7A"); } }
        public static Color blueGrey700 { get { return HexToColor("455A64"); } }
        public static Color blueGrey800 { get { return HexToColor("37474F"); } }
        public static Color blueGrey900 { get { return HexToColor("263238"); } }

        public static Color grey50 { get { return HexToColor("FAFAFA"); } }
        public static Color grey100 { get { return HexToColor("F5F5F5"); } }
        public static Color grey200 { get { return HexToColor("EEEEEE"); } }
        public static Color grey300 { get { return HexToColor("E0E0E0"); } }
        public static Color grey400 { get { return HexToColor("BDBDBD"); } }
        public static Color grey500 { get { return HexToColor("9E9E9E"); } }
        public static Color grey600 { get { return HexToColor("757575"); } }
        public static Color grey700 { get { return HexToColor("616161"); } }
        public static Color grey800 { get { return HexToColor("424242"); } }
        public static Color grey900 { get { return HexToColor("212121"); } }

        #endregion

        public static Color textDark { get { return HexToColor("000000", 0.87f); } }
        public static Color textLight { get { return HexToColor("FFFFFF"); } }

        public static Color textSecondaryDark { get { return HexToColor("000000", 0.54f); } }
        public static Color textSecondaryLight { get { return HexToColor("FFFFFF", 0.7f); } }

        public static Color textHintDark { get { return HexToColor("000000", 0.26f); } }
        public static Color textHintLight { get { return HexToColor("FFFFFF", 0.3f); } }

        public static Color iconDark { get { return HexToColor("000000", 0.54f); } }
        public static Color iconLight { get { return HexToColor("FFFFFF"); } }

        public static Color disabledDark { get { return HexToColor("000000", 0.26f); } }
        public static Color disabledLight { get { return HexToColor("FFFFFF", 0.3f); } }

        public static Color dividerDark { get { return HexToColor("000000", 0.12f); } }
        public static Color dividerLight { get { return HexToColor("FFFFFF", 0.12f); } }
    }
}