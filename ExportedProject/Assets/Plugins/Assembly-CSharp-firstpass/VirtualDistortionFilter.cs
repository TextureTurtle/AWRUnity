using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Distortion Filter")]
public class VirtualDistortionFilter : VirtualAudioFilter<AudioDistortionFilter>
{
	[SerializeField]
	private float _distortionLevel = 0.5f;

	public float DistortionLevel
	{
		get
		{
			return _distortionLevel;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_distortionLevel = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioDistortionFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.distortionLevel = value;
			}
		}
	}

	protected override void SynchronizeComponentProperties(AudioDistortionFilter realFilter)
	{
		realFilter.distortionLevel = _distortionLevel;
	}

	public override void InitializeWithRealComponentProperties(AudioDistortionFilter component)
	{
		_distortionLevel = component.distortionLevel;
	}
}
