using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UIScaleAnim : MonoBehaviour {

	[SerializeField]
	private AnimationCurve curve;
	public float animSpeed = 1f;
	[SerializeField]
	private bool playOnEnable = false;
	[SerializeField]
	private float onEnableDelay = 0f;

	private bool backwards = false;
	private bool disableOnEnd = false;

	[Header("Testing only:")]
	[SerializeField]
	private bool animate = false;
	private float time = 0;

	private RectTransform rt;

	// Cache
	void Awake()
	{

		rt = GetComponent<RectTransform>();
		SetAnimationState(time);
	}

	void OnEnable()
	{
		if (playOnEnable)
		{
			Animate(onEnableDelay, false, false);
		}
	}

	// Handle animation loop
	void Update () {
		if (animate)
		{
			if (!backwards)
				time += Time.deltaTime * animSpeed;
			else
				time -= Time.deltaTime * animSpeed;
			SetAnimationState(time);

			if (time > 1f || time < 0f)
			{
				if (disableOnEnd)
					gameObject.SetActive(false);

				animate = false;
				time = 0;
			}
		}
	}

	private void SetAnimationState(float _time)
	{
		rt.localScale = Vector3.one * curve.Evaluate(_time);
	}

	public void Animate(float _delay, bool _backwards, bool _disableOnEnd)
	{
		if (_backwards)
			time = 1f;
		backwards = _backwards;
		disableOnEnd = _disableOnEnd;
		SetAnimationState(time);
		StartCoroutine(_Animate(_delay));
	}

	public void AnimateSimple(bool _backwards)
	{
		Animate (0, _backwards, false);
	}

	IEnumerator _Animate(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		animate = true;
	}
}
