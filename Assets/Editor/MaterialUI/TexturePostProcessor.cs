
/*	This script is used when a new texture is created by the
 * 	shadow generator. It turns the texture into a sprite and
 * 	applies the right settings to apply to an image.
*/


using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{
	
	void OnPreprocessTexture()
	{
		
		if(assetPath.Contains("GeneratedShadows"))
		{
			TextureImporter importer = assetImporter as TextureImporter;
			importer.textureType  = TextureImporterType.Advanced;
			importer.npotScale = TextureImporterNPOTScale.None;
			importer.generateCubemap = TextureImporterGenerateCubemap.None;
			importer.spriteImportMode = SpriteImportMode.Single;
			importer.wrapMode = TextureWrapMode.Clamp;
			importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
			importer.filterMode = FilterMode.Trilinear;
//			Debug.Log(GlobalVars.shadowSpriteBorder);
			importer.spriteBorder = MaterialGlobals.shadowSpriteBorder;
			importer.mipmapEnabled = false;
			Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
			if (asset)
			{
				EditorUtility.SetDirty(asset);
			}
			else
			{
				importer.textureType  = TextureImporterType.Advanced;
			}
		}
		
	}
}