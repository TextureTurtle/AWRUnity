using UnityEngine;

namespace RamjetAnvil.Unity.VirtualAudio
{
	public interface IVirtualAudioComponent
	{
		void OnListenerAdded(VirtualAudioListener listener, GameObject realObject);

		void OnListenerRemoved(VirtualAudioListener listener);

		void DestroyRealComponents();
	}
}
