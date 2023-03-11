using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Chorus Filter")]
public class VirtualChorusFilter : VirtualAudioFilter<AudioChorusFilter>
{
	[SerializeField]
	private float _delay = 40f;

	[SerializeField]
	private float _depth = 0.03f;

	[SerializeField]
	private float _dryMix = 0.5f;

	[SerializeField]
	private float _feedback;

	[SerializeField]
	private float _rate = 0.8f;

	[SerializeField]
	private float _wetMix1 = 0.5f;

	[SerializeField]
	private float _wetMix2 = 0.5f;

	[SerializeField]
	private float _wetMix3 = 0.5f;

	public float Delay
	{
		get
		{
			return _delay;
		}
		set
		{
			value = Mathf.Clamp(value, 0.1f, 100f);
			_delay = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.delay = value;
			}
		}
	}

	public float Depth
	{
		get
		{
			return _depth;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_depth = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.depth = value;
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
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.dryMix = value;
			}
		}
	}

	public float Feedback
	{
		get
		{
			return _feedback;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_feedback = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.feedback = value;
			}
		}
	}

	public float Rate
	{
		get
		{
			return _rate;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 20f);
			_rate = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.rate = value;
			}
		}
	}

	public float WetMix1
	{
		get
		{
			return _wetMix1;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_wetMix1 = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.wetMix1 = value;
			}
		}
	}

	public float WetMix2
	{
		get
		{
			return _wetMix2;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_wetMix2 = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.wetMix2 = value;
			}
		}
	}

	public float WetMix3
	{
		get
		{
			return _wetMix3;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_wetMix3 = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioChorusFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.wetMix3 = value;
			}
		}
	}

	protected override void SynchronizeComponentProperties(AudioChorusFilter realFilter)
	{
		realFilter.delay = _delay;
		realFilter.depth = _depth;
		realFilter.dryMix = _dryMix;
		realFilter.feedback = _feedback;
		float rate = (realFilter.wetMix1 = _wetMix1);
		realFilter.rate = rate;
		realFilter.wetMix2 = _wetMix2;
		realFilter.wetMix3 = _wetMix3;
	}

	public override void InitializeWithRealComponentProperties(AudioChorusFilter component)
	{
		_delay = component.delay;
		_depth = component.depth;
		_dryMix = component.dryMix;
		_feedback = component.feedback;
		_rate = component.rate;
		_wetMix1 = component.wetMix1;
		_wetMix2 = component.wetMix2;
		_wetMix3 = component.wetMix3;
	}
}
