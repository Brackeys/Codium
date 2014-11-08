using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

public class Intellisense {

	public void GetFieldsInClass (string className, string namespaceName) {
		Type[] types = GetTypeByName ("Debug", "UnityEngine");
		for (int i = 0; i < types.Length; i++) {
			Debug.Log(types[i].Name);

			// Get the fields of the specified class.
			FieldInfo[] myField = types[i].GetFields();

			Debug.Log(myField.Length);
			for(int j = 0; j < myField.Length; j++)
			{
				Debug.Log(myField[j].Name);
			}
		}


		string typeName = namespaceName + "." + className;

		Debug.Log (typeName);

		Type myType = Type.GetType(typeName);

		if (myType == null) {
			Debug.LogError ("Class doesn't exist in namespace.");
			return;
		}

	}

	//Gets a all Type instances matching the specified class name with just non-namespace qualified class name.
	public static Type[] GetTypeByName(string className, string namespaceName)
	{
	    List<Type> returnVal = new List<Type>();

	    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
	    {
	        Type[] assemblyTypes = a.GetTypes();
	        for (int j = 0; j < assemblyTypes.Length; j++)
	        {
	            if (assemblyTypes[j].Name == className)
	            {
	            	if (assemblyTypes[j].Namespace == namespaceName)
	                	returnVal.Add(assemblyTypes[j]);
	            }
	        }
	    }

	    return returnVal.ToArray();
	}
}
