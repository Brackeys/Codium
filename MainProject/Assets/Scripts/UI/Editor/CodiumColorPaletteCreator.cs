using UnityEditor;

namespace Codium
{
	public class CodiumColorPaletteCreator
	{

		[MenuItem("Assets/Create/Codium/ColorPalette")]
		public static void CreateColorPalette()
		{
			ScriptableObjectUtility.CreateAsset<CodiumColorPalette>();
		}

	}
}
