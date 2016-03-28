using UnityEngine;

namespace Codium {
	
	public class CodiumColorPalette : ScriptableObject
	{
		[System.Serializable]
		public class CodiumColor
		{
			public string name;
			public Color color;

			public CodiumColor (string name, Color color)
			{
				this.name = name;
				this.color = color;
			}
		}

		[SerializeField]
		private CodiumColor[] colors;
		
		public Color GetColor (string name)
		{
			foreach (CodiumColor _color in colors)
			{
				if (_color.name == name)
					return _color.color;
			}

			Debug.LogError("Color: " + name + " not found.");

			return Color.magenta;
		}

	}
	
}
