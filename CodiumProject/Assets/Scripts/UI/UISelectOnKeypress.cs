using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectOnKeypress : MonoBehaviour {
	void Update () {

		if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) &&
			!Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
		{
			EventSystem e = EventSystem.current;

			e.SetSelectedGameObject(this.gameObject);
		}
	}
}
