//-----------------------------------------------------------------
// The Sound class of Audio System. It is used by the Audio Manager to
// easily pool together a bunch of sounds with each their settings.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem
{
	[System.Serializable]
	public class ASSound
	{
		public string name;

		public AudioClip sound;

		[Range(0f, 1f)]
		public float baseVolume;
		[Range(0f, 0.5f)]
		public float volumeVariance;

		public float basePitch;
		[Range(0f, 0.5f)]
		public float pitchVariance;

		public AudioMixerGroup mixer;

		[HideInInspector]
		public AudioSource source;	// Populate the source on start

		public ASSound ()
		{
			baseVolume = 0.7f;
			volumeVariance = 0.1f;

			basePitch = 1f;
			pitchVariance = 0.1f;
		}
	}

}
