using UnityEngine;

public class BurnerEffects : MonoBehaviour
{
	[SerializeField]
	private Burner _burner;

	[SerializeField]
	private VirtualAudioSource _useSource;

	[SerializeField]
	private VirtualAudioSource _burnSource;

	[SerializeField]
	private AudioClip _clickClip;

	[SerializeField]
	private AudioClip _igniteClip;

	[SerializeField]
	private AudioClip _stopFlameClip;

	[SerializeField]
	private ParticleSystem _particleSystem;

	[SerializeField]
	private Light _light;

	private Interactable _interactable;

	private float _maxVolume;

	private float _maxEmission;

	private float _maxIntensity;

	private Player _lastUser;

	private void Start()
	{
		_interactable = _burner.GetComponent<Interactable>();
		_interactable.OnUse += OnUse;
		_interactable.OnStopUse += OnStopUse;
		_maxVolume = _burnSource.Volume;
		_burnSource.Volume = 0f;
		_maxEmission = _particleSystem.emissionRate;
		_particleSystem.emissionRate = 0f;
		_maxIntensity = _light.intensity;
		_light.intensity = 0f;
		_burnSource.Loop = true;
		_burnSource.Play();
	}

	private void OnUse(Player player)
	{
		_useSource.PlayOneShot(player.Listener, (!_burner.IsOn) ? _clickClip : _igniteClip);
	}

	private void OnStopUse(Player player)
	{
		if (_burner.IsOn)
		{
			_useSource.PlayOneShot(player.Listener, _stopFlameClip);
		}
	}

	private void Update()
	{
		float num = _burner.BurnRate / _burner.MaxBurnRate;
		_burnSource.Volume = _maxVolume * num;
		_particleSystem.emissionRate = _maxEmission * num;
		_light.intensity = _maxIntensity * num;
	}
}
