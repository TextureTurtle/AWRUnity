using UnityEngine;

namespace RamjetAnvil.Unity.VirtualAudio
{
	[RequireComponent(typeof(VirtualAudioSource))]
	public abstract class VirtualAudioFilter<T> : VirtualAudioComponent<T> where T : Behaviour
	{
		public override void InitializeInPlayMode()
		{
			VirtualAudioSource component = GetComponent<VirtualAudioSource>();
			if ((bool)component)
			{
				component.InitializeInPlayMode();
			}
			base.InitializeInPlayMode();
		}
	}
}
