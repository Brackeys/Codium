using UnityEngine;
using System.Collections;

public class UIBringToFront : MonoBehaviour {

	void OnEnable()
	{
		transform.SetAsLastSibling();
	}
	
}
