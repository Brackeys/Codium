//-----------------------------------------------------------------
// Uses a sine wave to lerp between a max and a min value.
// The value is used for image alpha.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

public class UIPulseAnim : MonoBehaviour {

	[SerializeField]
	private Image image;

	[Range(0f, 10f)]
	[SerializeField]
	private float speed = 2f;
	[Range(0f, 1f)]
	[SerializeField]
	private float max = 0.5f;
	[Range(0f, 1f)]
	[SerializeField]
	private float min = 0;

	private float x = -1;
	bool update = true;

	void Update()
	{
		if (!update)
			return;

		Color _col = image.color;
		float _lerp = (Mathf.Sin(x) + 1f)/2f;
		_col.a = Mathf.Lerp (min, max, _lerp);
		image.color = _col;

		x += Time.deltaTime * speed;
	}

	public void Stop()
	{
		update = false;
		x = 0;
		Color _col = image.color;
		_col.a = 0;
		image.color = _col; 
	}

	public void Start()
	{
		update = true;
	}

}
