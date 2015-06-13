using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UIScaleAnim : MonoBehaviour {

	[SerializeField] private AnimationCurve curve;
	public float animSpeed = 1f;
	[SerializeField]
	private bool playOnEnable = false;
	[SerializeField]
	private float onEnableDelay = 0f;

	[Header("Testing only:")]
	[SerializeField]
	private bool animate = false;
	private float time = 0;

	private RectTransform rt;

	// Cache
	void Awake()
	{
		rt = GetComponent<RectTransform>();
		rt.localScale = Vector3.one * curve.Evaluate(0);
	}

	void OnEnable()
	{
		if (playOnEnable)
		{
			Animate(onEnableDelay);
		}
	}

	// Handle animation loop
	void Update () {
		if (animate)
		{
			time += Time.deltaTime * animSpeed;
			rt.localScale = Vector3.one * curve.Evaluate(time);

			if (time > 1f)
			{
				animate = false;
				time = 0;
			}
		}
	}

	public void Animate(float delay)
	{
		rt.localScale = Vector3.one * curve.Evaluate(0);
		StartCoroutine(_Animate(delay));
	}

	IEnumerator _Animate(float delay)
	{
		yield return new WaitForSeconds(delay);
		animate = true;
	}
}
