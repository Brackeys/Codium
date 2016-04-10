using UnityEditor;

namespace Codium.UI
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
