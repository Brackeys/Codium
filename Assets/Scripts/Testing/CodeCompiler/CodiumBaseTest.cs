using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class CodiumBaseTest : MonoBehaviour{

	public void CallMethods()
	{
		var type = typeof(ICodiumBase);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => type.IsAssignableFrom(p));

		for (int i = 0; i < types.ToArray().Length; i++)
		{
			Type _type = types.ToArray()[i];
			if (_type.IsClass)
			{
				object instance = Activator.CreateInstance(_type);
				MethodInfo method = _type.GetMethod("Entry", BindingFlags.Instance | BindingFlags.Public);
				Debug.Log(types.ToArray()[i]);
				Debug.Log("Method: " + _type);
				method.Invoke(instance, null);
			}
		}
	}
	
}
