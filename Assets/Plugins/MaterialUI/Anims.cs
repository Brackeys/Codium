using UnityEngine;
using System.Collections;

public class Anims : MonoBehaviour
{
	public static float EaseOutQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		time--;
		return differenceValue * (time * time * time * time * time + 1) + startValue;
	}

	public static float EaseInQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		return differenceValue * time * time * time * time * time + startValue;
	}

	public static float EaseInOutQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);

		time /= duration / 2f;
		if (time < 1f)
		{
			return differenceValue / 2 * time * time * time * time * time + startValue;
		}
		time -= 2f;
		return differenceValue / 2 * (time * time * time * time * time + 2) + startValue;
	}
}
