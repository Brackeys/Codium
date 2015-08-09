//-----------------------------------------------------------------
// Central Audio Class - Present in all scenes.
//-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.Audio;
using System;

namespace AudioSystem
{
	public class ASAudioManager : MonoBehaviour
	{

		#region Singleton pattern (Awake)

		private static ASAudioManager _ins;
		public static ASAudioManager ins
		{
			get
			{
				if (_ins == null)
				{
					_ins = GameObject.FindObjectOfType<ASAudioManager>();

					DontDestroyOnLoad(_ins.gameObject);
				}

				return _ins;
			}
			set
			{
				_ins = value;
			}
		}

		void Awake()
		{
			if (_ins == null)
			{
				// Populate with first instance
				_ins = this;
				DontDestroyOnLoad(this);
			}
			else
			{
				// Another instance exists, destroy
				if (this != _ins)
					Destroy(this.gameObject);
			}
		}

		#endregion

		[SerializeField]
		private GameObject audioPool;	// Gameobject that hosts the AudioSources for pool sounds

		[SerializeField]
		private AudioMixerGroup masterMixer;

		[SerializeField]
		private ASSound[] pool;			// A pool with a bunch of sounds and settings for said sounds

		void Start()
		{
			if (audioPool == null)
			{
				Debug.LogError("ASAudioManager: No audioPool object referenced.");
				return;
			}

			// Cycle through sounds and add sources
			for (int i = 0; i < pool.Length; i++)
			{
				pool[i].source = audioPool.AddComponent<AudioSource>();
			}
		}

		// Play a sound from the pool
		public void Play(string _soundName)
		{
			ASSound _sound = Array.Find(pool,
				element => element.name == _soundName);

			if (_sound == null)
			{
				Debug.LogWarning("Sound: '" + _soundName + "' couldn't be played because it wasn't found.");
				return;
			}

			AudioSource _source = _sound.source;
			_source.clip = _sound.sound;
			_source.volume = UnityEngine.Random.Range(_sound.baseVolume - _sound.volumeVariance, _sound.baseVolume + _sound.volumeVariance);
			_source.pitch = UnityEngine.Random.Range(_sound.basePitch - _sound.pitchVariance, _sound.basePitch + _sound.pitchVariance);

			if (_sound.mixer == null)
			{
				_source.outputAudioMixerGroup = masterMixer;
			}
			else
			{
				_source.outputAudioMixerGroup = _sound.mixer;
			}

			_source.Play();
		}

	}

}
