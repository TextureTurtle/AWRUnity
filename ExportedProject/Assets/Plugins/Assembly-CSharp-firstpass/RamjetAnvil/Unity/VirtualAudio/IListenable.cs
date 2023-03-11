using UnityEngine;

namespace RamjetAnvil.Unity.VirtualAudio
{
	public interface IListenable
	{
		void AddListener(VirtualAudioListener virtualListener);

		void RemoveListener(VirtualAudioListener virtualListener);

		void OnListenerEnabled(VirtualAudioListener virtualListener);

		void OnListenerDisabled(VirtualAudioListener virtualListener);

		void SetHideflags(HideFlags hideFlags);
	}
}
