using UnityEngine;

public class PressureReleaseValveEffects : MonoBehaviour
{
	[SerializeField]
	private Balloon _balloon;

	[SerializeField]
	private float _pressureDifferenceMultiplier = 1f;

	[SerializeField]
	private VirtualAudioSource _source;

	[SerializeField]
	private ParticleSystem _particleSystem;

	private float _maxVolume;

	private float _maxEmission;

	private bool _isValveOpen;

	private float _pressureRate;

	private void Start()
	{
		_balloon.OnOpenReleaseValve += OnValveOpen;
		_maxVolume = _source.Volume;
		_source.Volume = 0f;
		_maxEmission = _particleSystem.emissionRate;
		_particleSystem.emissionRate = 0f;
	}

	private void Update()
	{
		if (_isValveOpen)
		{
			float to = Mathf.Abs(_balloon.Temperature - _balloon.AmbientTemperature) * _pressureDifferenceMultiplier;
			_pressureRate = Mathf.Lerp(_pressureRate, to, Time.time * 1f);
			_source.Volume = _maxVolume * _pressureRate;
			_particleSystem.emissionRate = _maxEmission * _pressureRate;
		}
		else
		{
			_source.Volume = 0f;
			_particleSystem.emissionRate = 0f;
		}
	}

	private void OnValveOpen(bool open)
	{
		_isValveOpen = open;
	}
}
