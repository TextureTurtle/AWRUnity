using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Reverb Filter")]
public class VirtualReverbFilter : VirtualAudioFilter<AudioReverbFilter>
{
	[SerializeField]
	private float _decayHFRatio = 0.5f;

	[SerializeField]
	private float _decayTime = 1f;

	[SerializeField]
	private float _density = 100f;

	[SerializeField]
	private float _diffusion = 100f;

	[SerializeField]
	private float _dryLevel;

	[SerializeField]
	private float _hfReference = 5000f;

	[SerializeField]
	private float _lfReference = 250f;

	[SerializeField]
	private float _reflectionsDelay;

	[SerializeField]
	private float _reflectionsLevel;

	[SerializeField]
	private float _reverbDelay = 0.04f;

	[SerializeField]
	private float _reverbLevel;

	[SerializeField]
	private AudioReverbPreset _reverbPreset = AudioReverbPreset.User;

	[SerializeField]
	private float _room;

	[SerializeField]
	private float _roomHF;

	[SerializeField]
	private float _roomLF;

	[SerializeField]
	private float _roomRolloff = 10f;

	public float DecayHFRatio
	{
		get
		{
			return _decayHFRatio;
		}
		set
		{
			value = Mathf.Clamp(value, 0.1f, 2f);
			_decayHFRatio = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.decayHFRatio = value;
			}
		}
	}

	public float DecayTime
	{
		get
		{
			return _decayTime;
		}
		set
		{
			_decayTime = Mathf.Clamp(value, 0.1f, 20f);
			_decayTime = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.decayTime = value;
			}
		}
	}

	public float Density
	{
		get
		{
			return _density;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 100f);
			_density = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.density = value;
			}
		}
	}

	public float Diffusion
	{
		get
		{
			return _density;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 100f);
			_diffusion = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.diffusion = value;
			}
		}
	}

	public float DryLevel
	{
		get
		{
			return _dryLevel;
		}
		set
		{
			value = Mathf.Clamp(value, -10000f, 0f);
			_dryLevel = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.dryLevel = value;
			}
		}
	}

	public float HFReference
	{
		get
		{
			return _hfReference;
		}
		set
		{
			value = Mathf.Clamp(value, 1000f, 20000f);
			_hfReference = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.hfReference = value;
			}
		}
	}

	public float LFReference
	{
		get
		{
			return _lfReference;
		}
		set
		{
			value = Mathf.Clamp(value, 20f, 1000f);
			_lfReference = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.lfReference = value;
			}
		}
	}

	public float ReflectionsDelay
	{
		get
		{
			return _reflectionsDelay;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 0.3f);
			_reflectionsDelay = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.reflectionsDelay = value;
			}
		}
	}

	public float ReflectionsLevel
	{
		get
		{
			return _reflectionsLevel;
		}
		set
		{
			value = Mathf.Clamp(value, -10000f, 1000f);
			_reflectionsLevel = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.reflectionsLevel = value;
			}
		}
	}

	public float ReverbDelay
	{
		get
		{
			return _reverbDelay;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 0.1f);
			_reverbDelay = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.reverbDelay = value;
			}
		}
	}

	public float ReverbLevel
	{
		get
		{
			return _reverbLevel;
		}
		set
		{
			value = Mathf.Clamp(value, -10000f, 2000f);
			_reverbLevel = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.reverbLevel = value;
			}
		}
	}

	public AudioReverbPreset ReverbPreset
	{
		get
		{
			return _reverbPreset;
		}
		set
		{
			_reverbPreset = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.reverbPreset = value;
			}
		}
	}

	public float Room
	{
		get
		{
			return _room;
		}
		set
		{
			value = Mathf.Clamp(value, -10000f, 0f);
			_room = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.room = value;
			}
		}
	}

	public float RoomHF
	{
		get
		{
			return _roomHF;
		}
		set
		{
			value = Mathf.Clamp(value, -10000f, 0f);
			_roomHF = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.roomHF = value;
			}
		}
	}

	public float RoomLF
	{
		get
		{
			return _roomLF;
		}
		set
		{
			value = Mathf.Clamp(value, -10000f, 0f);
			_roomLF = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.roomLF = value;
			}
		}
	}

	public float RoomRolloff
	{
		get
		{
			return _roomRolloff;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 10f);
			_roomRolloff = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			/*foreach (AudioReverbFilter listenedComponent in base.ListenedComponents)
			{
				listenedComponent.roomRolloff = value;
			}*/
		}
	}

	protected override void SynchronizeComponentProperties(AudioReverbFilter realFilter)
	{
		realFilter.decayHFRatio = _decayHFRatio;
		realFilter.decayTime = _decayTime;
		realFilter.density = _density;
		realFilter.diffusion = _diffusion;
		realFilter.dryLevel = _dryLevel;
		realFilter.hfReference = _hfReference;
		realFilter.lfReference = _lfReference;
		realFilter.reflectionsDelay = _reflectionsDelay;
		realFilter.reflectionsLevel = _reflectionsLevel;
		realFilter.reverbDelay = _reverbDelay;
		realFilter.reverbLevel = _reverbLevel;
		realFilter.reverbPreset = _reverbPreset;
		realFilter.room = _room;
		realFilter.roomHF = _roomHF;
		realFilter.roomLF = _roomLF;
		//realFilter.roomRolloff = _roomRolloff;
	}

	public override void InitializeWithRealComponentProperties(AudioReverbFilter component)
	{
		_decayHFRatio = component.decayHFRatio;
		_decayTime = component.decayTime;
		_density = component.density;
		_diffusion = component.diffusion;
		_dryLevel = component.dryLevel;
		_hfReference = component.hfReference;
		_lfReference = component.lfReference;
		_reflectionsDelay = component.reflectionsDelay;
		_reflectionsLevel = component.reflectionsLevel;
		_reverbDelay = component.reverbDelay;
		_reverbLevel = component.reverbLevel;
		_reverbPreset = component.reverbPreset;
		_room = component.room;
		_roomHF = component.roomHF;
		_roomLF = component.roomLF;
		//_roomRolloff = component.roomRolloff;
	}
}
