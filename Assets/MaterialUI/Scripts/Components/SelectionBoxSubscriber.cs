using UnityEngine;
using System.Collections;

public class SelectionBoxSubscriber : MonoBehaviour
{
	SelectionBoxConfig config;

	void OnEnable ()
	{
		config = gameObject.GetComponent<SelectionBoxConfig> ();
		config.ItemPicked += DoThing;
	}

	void OnDisable ()
	{
		config.ItemPicked -= DoThing;
	}

	void DoThing (int id)
	{
		Debug.Log ("'" + config.listItems[id] + "' picked, ID: " + id);
	}
}
