using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Echo Filter")]
public class VirtualEchoFilter : VirtualAudioFilter<AudioEchoFilter>
{
	[SerializeField]
	private int _delay = 500;

	[SerializeField]
	private float _decayRatio = 0.5f;

	[SerializeField]
	private float _wetMix = 1f;

	[SerializeField]
	private float _dryMix = 1f;

	public int Delay
	{
		get
		{
			return _delay;
		}
		set
		{
			value = Mathf.Clamp(value, 10, 5000);
			_delay = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioEchoFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.delay = value;
			}
		}
	}

	public float DecayRatio
	{
		get
		{
			return _decayRatio;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_decayRatio = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioEchoFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.decayRatio = value;
			}
		}
	}

	public float WetMix
	{
		get
		{
			return _wetMix;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_wetMix = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioEchoFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.wetMix = value;
			}
		}
	}

	public float DryMix
	{
		get
		{
			return _dryMix;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_dryMix = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioEchoFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.dryMix = value;
			}
		}
	}

	protected override void SynchronizeComponentProperties(AudioEchoFilter realFilter)
	{
		realFilter.delay = _delay;
		realFilter.decayRatio = _decayRatio;
		realFilter.wetMix = _wetMix;
		realFilter.dryMix = _dryMix;
	}

	public override void InitializeWithRealComponentProperties(AudioEchoFilter component)
	{
		_delay = (int)component.delay;
		_decayRatio = component.decayRatio;
		_wetMix = component.wetMix;
		_dryMix = component.dryMix;
	}
}
