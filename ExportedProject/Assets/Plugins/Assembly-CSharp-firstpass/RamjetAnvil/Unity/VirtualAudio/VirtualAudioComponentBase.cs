using UnityEngine;

namespace RamjetAnvil.Unity.VirtualAudio
{
	public abstract class VirtualAudioComponentBase : MonoBehaviour, IVirtualAudioComponent
	{
		public abstract void OnListenerAdded(VirtualAudioListener listener, GameObject realObject);

		public abstract void OnListenerRemoved(VirtualAudioListener listener);

		public abstract void DestroyRealComponents();
	}
}
