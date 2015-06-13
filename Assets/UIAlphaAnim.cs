using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAlphaAnim : MonoBehaviour {

	[SerializeField]
	private AnimationCurve curve;
	public float animSpeed = 1f;
	[SerializeField]
	private bool playOnEnable = false;
	[SerializeField]
	private float onEnableDelay = 0f;

	private bool backwards = false;
	private bool disableOnEnd = false;

	[Header("Requires CanvasGroup instead of Image Component:")]
	[SerializeField]
	private bool affectChildren = false;

	[Header("Testing only:")]
	[SerializeField]
	private bool animate = false;
	private float time = 0;

	private CanvasGroup cg;
	private Image img;

	// Cache
	void Awake()
	{
		if (affectChildren)
		{
			cg = GetComponent<CanvasGroup>();
			if (cg == null)
			{
				Debug.LogError("Affect children requires a CanvasGroup.");
				return;
			}
		}
		else
		{
			img = GetComponent<Image>();
			if (img == null)
			{
				Debug.LogError("No Image component found.");
				return;
			}
		}

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
	void Update()
	{
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
		if (affectChildren)
			cg.alpha = curve.Evaluate(_time);
		else
			img.color = new Color(img.color.r, img.color.g, img.color.b, curve.Evaluate(_time));
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

	IEnumerator _Animate(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		animate = true;
	}
}
