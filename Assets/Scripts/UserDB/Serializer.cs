//-----------------------------------------------------------------
// The central Codium serialization (data save/load) component.
//-----------------------------------------------------------------

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer : MonoBehaviour
{
	#region Singleton pattern (Awake)
	private static Serializer _ins;
	public static Serializer ins
	{
		get
		{
			if (_ins == null)
			{
				_ins = GameObject.FindObjectOfType<Serializer>();

				DontDestroyOnLoad(_ins.gameObject);
			}

			return _ins;
		}
		set
		{
			_ins = value;
		}
	}

	void Awake()
	{
		if (_ins == null)
		{
			// Populate with first instance
			_ins = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			// Another instance exists, destroy
			if (this != _ins)
				Destroy(this.gameObject);
		}

		path = Application.persistentDataPath;
	}

	#endregion

	static string path;	// Set in awake

	#region Generic Save/Load methods
	public static void Save<T>(T _data, string _name)
	{
		string _fullPath = path + "/" + _name + ".dat";

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(_fullPath);

		bf.Serialize(file, _data);
		file.Close();

		Debug.Log("Data of type: " + _data.GetType().ToString() + " was saved to path: " + _fullPath);
	}

	public static T Load<T>(string _name)
	{
		string _fullPath = path + "/" + _name + ".dat";

		if (!File.Exists(_fullPath))
		{
			Debug.LogError("No save file found. Returns data passed as argument. Search Path: " + _fullPath);
			return default(T);
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(_fullPath, FileMode.Open);
		T data = (T)bf.Deserialize(file);
		file.Close();

		return data;
	}
	#endregion
}
