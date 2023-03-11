using RamjetAnvil.Unity.Utility;
using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Audio Source")]
public class VirtualAudioSource : VirtualAudioComponent<AudioSource>
{
	private const string iconFileName = "ramjetaudio-speaker_icon.png";

	[SerializeField]
	private float _volume = 1f;

	[SerializeField]
	private float _pitch = 1f;

	[SerializeField]
	private AudioClip _clip;

	[SerializeField]
	private bool _loop;

	[SerializeField]
	private bool _playOnAwake = true;

	[SerializeField]
	private bool _mute;

	[SerializeField]
	private bool _bypassEffects;

	[SerializeField]
	private int _priority = 128;

	[SerializeField]
	private float _dopplerLevel = 1f;

	[SerializeField]
	private float _panLevel = 1f;

	[SerializeField]
	private float _spread;

	[SerializeField]
	private float _pan;

	[SerializeField]
	private float _minDistance = 1f;

	[SerializeField]
	private float _maxDistance = 500f;

	[SerializeField]
	private AudioVelocityUpdateMode _velocityUpdateMode;

	[SerializeField]
	private VirtualRolloffMode _volumeRolloffMode = VirtualRolloffMode.Logarithmic;

	[SerializeField]
	private VirtualRolloffMode _panLevelRolloffMode;

	[SerializeField]
	private VirtualRolloffMode _spreadRolloffMode;

	[SerializeField]
	private AnimationCurve _volumeRolloff = AnimationCurveUtils.Logarithmic(0.002f, 1f, 1f);

	[SerializeField]
	private AnimationCurve _panLevelRolloff = AnimationCurveUtils.Constant(1f);

	[SerializeField]
	private AnimationCurve _spreadRolloff = AnimationCurveUtils.Constant(0f);

	[SerializeField]
	private bool _ignoreListenerVolume;

	[SerializeField]
	private bool _isPlaying;

	public float Volume
	{
		get
		{
			return _volume;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_volume = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.volume = _volume;
			}
		}
	}

	public float Pitch
	{
		get
		{
			return _pitch;
		}
		set
		{
			value = Mathf.Clamp(value, -3f, 3f);
			_pitch = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.pitch = value;
			}
		}
	}

	public AudioClip Clip
	{
		get
		{
			return _clip;
		}
		set
		{
			_clip = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.clip = value;
			}
		}
	}

	public bool IsPlaying
	{
		get
		{
			return _isPlaying;
		}
	}

	public bool Loop
	{
		get
		{
			return _loop;
		}
		set
		{
			_loop = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.loop = value;
			}
		}
	}

	public float Time
	{
		get
		{
			return (!(base.PropertyHelper != null)) ? 0f : base.PropertyHelper.time;
		}
		set
		{
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.time = value;
			}
		}
	}

	public int TimeSamples
	{
		get
		{
			return (base.PropertyHelper != null) ? base.PropertyHelper.timeSamples : 0;
		}
		set
		{
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.timeSamples = value;
			}
		}
	}

	public bool PlayOnAwake
	{
		get
		{
			return _playOnAwake;
		}
		set
		{
			_playOnAwake = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.playOnAwake = value;
			}
		}
	}

	public bool Mute
	{
		get
		{
			return _mute;
		}
		set
		{
			_mute = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.mute = value;
			}
		}
	}

	public bool BypassEffects
	{
		get
		{
			return _bypassEffects;
		}
		set
		{
			_bypassEffects = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.bypassEffects = value;
			}
		}
	}

	public int Priority
	{
		get
		{
			return _priority;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 255);
			_priority = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.priority = value;
			}
		}
	}

	public float DopplerLevel
	{
		get
		{
			return _dopplerLevel;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 5f);
			_dopplerLevel = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.dopplerLevel = value;
			}
		}
	}

	public float PanLevel
	{
		get
		{
			return _panLevel;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			_panLevel = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.spatialBlend = value;
			}
		}
	}

	public float Spread
	{
		get
		{
			return _spread;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 360f);
			_spread = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.spread = value;
			}
		}
	}

	public float Pan
	{
		get
		{
			return _pan;
		}
		set
		{
			value = Mathf.Clamp(value, -1f, 1f);
			_pan = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.panStereo = value;
			}
		}
	}

	public float MinDistance
	{
		get
		{
			return _minDistance;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, float.MaxValue);
			_minDistance = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.minDistance = value;
			}
		}
	}

	public float MaxDistance
	{
		get
		{
			return _maxDistance;
		}
		set
		{
			value = Mathf.Clamp(value, _minDistance + 0.01f, float.MaxValue);
			_maxDistance = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.maxDistance = value;
			}
		}
	}

	public AudioVelocityUpdateMode VelocityUpdateMode
	{
		get
		{
			return _velocityUpdateMode;
		}
		set
		{
			_velocityUpdateMode = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.velocityUpdateMode = value;
			}
		}
	}

	public VirtualRolloffMode VolumeRolloffMode
	{
		get
		{
			return _volumeRolloffMode;
		}
		set
		{
			_volumeRolloffMode = value;
			AudioRolloffMode rolloffMode = VirtualRolloffModeToAudioRolloffMode(value);
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.rolloffMode = rolloffMode;
			}
		}
	}

	public VirtualRolloffMode PanLevelRolloffMode
	{
		get
		{
			return _panLevelRolloffMode;
		}
		set
		{
			_panLevelRolloffMode = value;
		}
	}

	public VirtualRolloffMode SpreadRolloffMode
	{
		get
		{
			return _spreadRolloffMode;
		}
		set
		{
			_spreadRolloffMode = value;
		}
	}

	public AnimationCurve VolumeRolloff
	{
		get
		{
			return _volumeRolloff;
		}
		set
		{
			_volumeRolloff = value;
		}
	}

	public AnimationCurve PanLevelRolloff
	{
		get
		{
			return _panLevelRolloff;
		}
		set
		{
			_panLevelRolloff = value;
		}
	}

	public AnimationCurve SpreadRolloff
	{
		get
		{
			return _spreadRolloff;
		}
		set
		{
			_spreadRolloff = value;
		}
	}

	public bool IgnoreListenerVolume
	{
		get
		{
			return _ignoreListenerVolume;
		}
		set
		{
			_ignoreListenerVolume = value;
			if (base.ListenedComponents == null)
			{
				return;
			}
			foreach (AudioSource listenedComponent in base.ListenedComponents)
			{
				listenedComponent.ignoreListenerVolume = value;
			}
		}
	}

	public void Play()
	{
		_isPlaying = true;
		foreach (AudioSource listenedComponent in base.ListenedComponents)
		{
			if (listenedComponent.gameObject.IsActive())
			{
				listenedComponent.Play();
			}
		}
	}

	public void Play(VirtualAudioListener listener)
	{
		if (!base.VirtualObject.ExclusiveListener || !(listener != base.VirtualObject.ExclusiveListener))
		{
			_isPlaying = true;
			base[listener].Play();
		}
	}

	public void Play(ulong delay)
	{
		_isPlaying = true;
		foreach (AudioSource listenedComponent in base.ListenedComponents)
		{
			listenedComponent.Play(delay);
		}
	}

	public void Play(VirtualAudioListener listener, ulong delay)
	{
		if (!base.VirtualObject.ExclusiveListener || !(listener != base.VirtualObject.ExclusiveListener))
		{
			_isPlaying = true;
			base[listener].Play(delay);
		}
	}

	public void Stop()
	{
		_isPlaying = false;
		foreach (AudioSource listenedComponent in base.ListenedComponents)
		{
			listenedComponent.Stop();
		}
	}

	public void Stop(VirtualAudioListener listener)
	{
		if (!base.VirtualObject.ExclusiveListener || !(listener != base.VirtualObject.ExclusiveListener))
		{
			_isPlaying = false;
			base[listener].Stop();
		}
	}

	public void Pause()
	{
		_isPlaying = false;
		foreach (AudioSource listenedComponent in base.ListenedComponents)
		{
			listenedComponent.Pause();
		}
	}

	public void Pause(VirtualAudioListener listener)
	{
		if (!base.VirtualObject.ExclusiveListener || !(listener != base.VirtualObject.ExclusiveListener))
		{
			_isPlaying = false;
			base[listener].Pause();
		}
	}

	public void PlayOneShot(AudioClip clip)
	{
		PlayOneShot(clip, 1f);
	}

	public void PlayOneShot(VirtualAudioListener listener, AudioClip clip)
	{
		PlayOneShot(listener, clip, 1f);
	}

	public void PlayOneShot(AudioClip clip, float volumeScale)
	{
		_isPlaying = true;
		foreach (AudioSource listenedComponent in base.ListenedComponents)
		{
			listenedComponent.PlayOneShot(clip, volumeScale);
		}
	}

	public void PlayOneShot(VirtualAudioListener listener, AudioClip clip, float volumeScale)
	{
		if (!base.VirtualObject.ExclusiveListener || !(listener != base.VirtualObject.ExclusiveListener))
		{
			base[listener].PlayOneShot(clip, volumeScale);
		}
	}

	public void GetOutputData(float[] samples, int channel)
	{
		if ((bool)base.PropertyHelper)
		{
			base.PropertyHelper.GetOutputData(samples, channel);
		}
	}

	public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
	{
		if ((bool)base.PropertyHelper)
		{
			base.PropertyHelper.GetSpectrumData(samples, channel, window);
		}
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
	{
		PlayClipAtPoint(clip, position, 1f);
	}

	public static void PlayClipAtPoint(VirtualAudioListener listener, AudioClip clip, Vector3 position)
	{
		PlayClipAtPoint(listener, clip, position, 1f);
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
	{
		GameObject gameObject = new GameObject(clip.name + " One Shot");
		gameObject.transform.position = position;
		VirtualAudioSource virtualAudioSource = gameObject.AddComponent<VirtualAudioSource>();
		virtualAudioSource.Clip = clip;
		virtualAudioSource.Volume = volume;
		virtualAudioSource.Play();
		Object.Destroy(gameObject, clip.length);
	}

	public static void PlayClipAtPoint(VirtualAudioListener listener, AudioClip clip, Vector3 position, float volume)
	{
		GameObject gameObject = new GameObject(clip.name + " One Shot");
		gameObject.transform.position = position;
		VirtualAudioSource virtualAudioSource = gameObject.AddComponent<VirtualAudioSource>();
		virtualAudioSource.Clip = clip;
		virtualAudioSource.Volume = volume;
		virtualAudioSource.Play(listener);
		Object.Destroy(gameObject, clip.length);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_isPlaying = false;
	}

	protected override void SynchronizeComponentProperties(AudioSource realSource)
	{
		realSource.volume = _volume;
		realSource.pitch = _pitch;
		realSource.clip = _clip;
		realSource.playOnAwake = _playOnAwake;
		realSource.loop = _loop;
		realSource.mute = _mute;
		realSource.bypassEffects = _bypassEffects;
		realSource.timeSamples = TimeSamples;
		realSource.priority = _priority;
		realSource.dopplerLevel = _dopplerLevel;
		realSource.spatialBlend = _panLevel;
		realSource.spread = _spread;
		realSource.panStereo = _pan;
		realSource.minDistance = _minDistance;
		realSource.maxDistance = _maxDistance;
		realSource.rolloffMode = VirtualRolloffModeToAudioRolloffMode(_volumeRolloffMode);
		realSource.velocityUpdateMode = _velocityUpdateMode;
		realSource.ignoreListenerVolume = _ignoreListenerVolume;
		if (Application.isPlaying && realSource.gameObject.IsActive() && realSource.enabled && !realSource.isPlaying && (_playOnAwake || IsPlaying))
		{
			realSource.Play();
		}
	}

	public override void InitializeWithRealComponentProperties(AudioSource component)
	{
		Volume = component.volume;
		Pitch = component.pitch;
		Clip = component.clip;
		PlayOnAwake = component.playOnAwake;
		Loop = component.loop;
		Mute = component.mute;
		BypassEffects = component.bypassEffects;
		TimeSamples = component.timeSamples;
		Priority = component.priority;
		DopplerLevel = component.dopplerLevel;
		PanLevel = component.spatialBlend;
		Spread = component.spread;
		Pan = component.panStereo;
		MinDistance = component.minDistance;
		MaxDistance = component.maxDistance;
		VolumeRolloffMode = AudioRolloffModeToVirtualRolloffMode(component.rolloffMode);
		VelocityUpdateMode = component.velocityUpdateMode;
		IgnoreListenerVolume = component.ignoreListenerVolume;
	}

	public static VirtualRolloffMode AudioRolloffModeToVirtualRolloffMode(AudioRolloffMode audioRolloffMode)
	{
		switch (audioRolloffMode)
		{
		case AudioRolloffMode.Linear:
			return VirtualRolloffMode.Linear;
		case AudioRolloffMode.Logarithmic:
			return VirtualRolloffMode.Logarithmic;
		case AudioRolloffMode.Custom:
			return VirtualRolloffMode.Custom;
		default:
			return VirtualRolloffMode.Logarithmic;
		}
	}

	public static AudioRolloffMode VirtualRolloffModeToAudioRolloffMode(VirtualRolloffMode virtualRolloffMode)
	{
		switch (virtualRolloffMode)
		{
		case VirtualRolloffMode.Constant:
			return AudioRolloffMode.Custom;
		case VirtualRolloffMode.Linear:
			return AudioRolloffMode.Linear;
		case VirtualRolloffMode.Logarithmic:
			return AudioRolloffMode.Logarithmic;
		case VirtualRolloffMode.Custom:
			return AudioRolloffMode.Custom;
		default:
			return AudioRolloffMode.Logarithmic;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "ramjetaudio-speaker_icon.png");
	}
}
