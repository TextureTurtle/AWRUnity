using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Lowpass Filter")]
public class VirtualLowPassFilter : VirtualAudioFilter<AudioLowPassFilter>
{
	[SerializeField]
	private float _cutoffFrequency = 5000f;

	[SerializeField]
	private float _resonanceQ = 1f;

	public float CutoffFrequency
	{
		get
		{
			return _cutoffFrequency;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 22000f);
			_cutoffFrequency = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioLowPassFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.cutoffFrequency = value;
			}
		}
	}

	public float ResonanceQ
	{
		get
		{
			return _resonanceQ;
		}
		set
		{
			value = Mathf.Clamp(value, 1f, 10f);
			_resonanceQ = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioLowPassFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.lowpassResonanceQ = value;
			}
		}
	}

	protected override void SynchronizeComponentProperties(AudioLowPassFilter realFilter)
	{
		realFilter.cutoffFrequency = _cutoffFrequency;
		realFilter.lowpassResonanceQ = _resonanceQ;
	}

	public override void InitializeWithRealComponentProperties(AudioLowPassFilter component)
	{
		_cutoffFrequency = component.cutoffFrequency;
		_resonanceQ = component.lowpassResonanceQ;
	}
}
