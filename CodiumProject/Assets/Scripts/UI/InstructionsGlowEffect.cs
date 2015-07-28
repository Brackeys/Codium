using UnityEngine;

public class InstructionsGlowEffect : MonoBehaviour {

	[SerializeField]
	private ParticleSystem[] particles;
	private bool active = true;

	public void Disable()
	{
		if (!active)
			return;

		active = false;

		foreach (ParticleSystem _particle in particles)
		{
			_particle.enableEmission = false;
		}
	}

}
