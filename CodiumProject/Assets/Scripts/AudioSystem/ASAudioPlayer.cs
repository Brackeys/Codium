//-----------------------------------------------------------------
// MonoBehaviour that can be used locally to link up with the Audio Manager
// to play sounds.
//-----------------------------------------------------------------

using UnityEngine;

namespace AudioSystem 
{
	public class ASAudioPlayer : MonoBehaviour {

		private ASAudioManager audioManager;

		void Start()
		{
			audioManager = ASAudioManager.ins;
			if (audioManager == null)
			{
				Debug.LogError("ASAudioPlayer: No audio manager found!");
			}
		}

		public void PlaySound(string _soundName)
		{
			audioManager.Play(_soundName);
		}

	}
}
