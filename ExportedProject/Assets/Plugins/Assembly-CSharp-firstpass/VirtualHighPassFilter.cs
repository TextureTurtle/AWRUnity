using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Highpass Filter")]
public class VirtualHighPassFilter : VirtualAudioFilter<AudioHighPassFilter>
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
			value = Mathf.Clamp(value, 10f, 22000f);
			_cutoffFrequency = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioHighPassFilter listenedComponent in base.ListenedComponents)
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
			foreach (AudioHighPassFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.highpassResonanceQ = value;
			}
		}
	}

	protected override void SynchronizeComponentProperties(AudioHighPassFilter realFilter)
	{
		realFilter.cutoffFrequency = _cutoffFrequency;
		realFilter.highpassResonanceQ = _resonanceQ;
	}

	public override void InitializeWithRealComponentProperties(AudioHighPassFilter component)
	{
		_cutoffFrequency = component.cutoffFrequency;
		_resonanceQ = component.highpassResonanceQ;
	}
}
