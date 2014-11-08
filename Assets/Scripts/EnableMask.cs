using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Mask))]
public class EnableMask : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Mask>().enabled = true;
	}

}