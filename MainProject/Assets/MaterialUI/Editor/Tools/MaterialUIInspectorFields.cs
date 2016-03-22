//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEditor;
using UnityEngine;

namespace MaterialUI
{
    public static class MaterialUIInspectorFields
    {
        public static void VectorImageDataField(string label, VectorImageData targetData, Object targetObject)
        {
            string code = targetData.glyph.unicode;
            string name = targetData.glyph.name;
            Font font = targetData.font;
            GUIStyle iconStyle = new GUIStyle { font = font, fontSize = 16 };

            float offsetH = 0;

            offsetH += 40;
			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel(label);
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(-12f));
				
				if (!string.IsNullOrEmpty(name))
				{
					GUIContent widthLabel = new GUIContent(IconDecoder.Decode(code));
					float widthLabelWidth = iconStyle.CalcSize(widthLabel).x;
					EditorGUILayout.LabelField(widthLabel, iconStyle, GUILayout.MaxWidth(widthLabelWidth));
					widthLabel = new GUIContent(name);
					widthLabelWidth = GUIStyle.none.CalcSize(widthLabel).x;
					EditorGUILayout.LabelField(name, GUILayout.MaxWidth(widthLabelWidth + 4));
					GUILayout.FlexibleSpace();
				}
				else
				{
					EditorGUILayout.LabelField("No icon selected", GUILayout.MaxWidth(96f));
					GUILayout.FlexibleSpace();
				}
				
				if (GUILayout.Button("Pick Icon"))
				{
					VectorImagePickerWindow.Show(targetData, targetObject);
				}
				
				if (!string.IsNullOrEmpty(name))
				{
					if (GUILayout.Button(IconDecoder.Decode(@"\ue14c"), new GUIStyle { font = VectorImageManager.GetIconFont(VectorImageManager.materialDesignIconsFontName), fontSize = 20 }))
					{
						targetData.font = null;
						targetData.glyph = new Glyph();
						EditorUtility.SetDirty(targetObject);
					}
				}
			}
        }
    }
}