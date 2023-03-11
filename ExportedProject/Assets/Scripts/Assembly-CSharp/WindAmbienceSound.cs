using System;
using System.Collections.Generic;
using UnityEngine;

public class WindAmbienceSound : MonoBehaviour
{
	[Serializable]
	public class WindClip
	{
		[SerializeField]
		public AudioClip AudioClip;

		[SerializeField]
		public AnimationCurve VolumeEnvelope = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public float MinWindSpeed
		{
			get
			{
				return VolumeEnvelope.keys[0].time;
			}
		}

		public float MaxWindSpeed
		{
			get
			{
				return VolumeEnvelope.keys[VolumeEnvelope.keys.Length - 1].time;
			}
		}
	}

	[SerializeField]
	private VirtualAudioListener _listener;

	[SerializeField]
	private Rigidbody _velocitySource;

	[SerializeField]
	private WindClip[] _clips;

	[SerializeField]
	private float _volume = 1f;

	[SerializeField]
	private float _maxSources;

	[SerializeField]
	private float _smoothSpeed = 2f;

	private Dictionary<WindClip, VirtualAudioSource> _sources;

	private float _lastRelativeSpeed;

	private void Start()
	{
		_sources = new Dictionary<WindClip, VirtualAudioSource>();
	}

	private void Update()
	{
		Vector3 vector = _velocitySource.velocity - SingletonComponent<AeroManager>.Instance.GetWindVelocity(base.transform.position, true);
		float num = (_lastRelativeSpeed = Mathf.Lerp(_lastRelativeSpeed, vector.magnitude, _smoothSpeed * Time.deltaTime));
		WindClip[] clips = _clips;
		foreach (WindClip windClip in clips)
		{
			if (num > windClip.MinWindSpeed && num < windClip.MaxWindSpeed)
			{
				VirtualAudioSource virtualAudioSource = null;
				if (_sources.ContainsKey(windClip))
				{
					virtualAudioSource = _sources[windClip];
				}
				else if ((float)_sources.Count >= _maxSources)
				{
					virtualAudioSource = StealLeastImportantSource();
					virtualAudioSource.Clip = windClip.AudioClip;
					virtualAudioSource.Play();
					_sources.Add(windClip, virtualAudioSource);
				}
				else
				{
					virtualAudioSource = base.gameObject.AddComponent<VirtualAudioSource>();
					virtualAudioSource.VirtualObject.ExclusiveListener = _listener;
					virtualAudioSource.DopplerLevel = 0f;
					virtualAudioSource.Clip = windClip.AudioClip;
					virtualAudioSource.Loop = true;
					virtualAudioSource.Play();
					_sources.Add(windClip, virtualAudioSource);
				}
				float time = num;
				virtualAudioSource.Volume = windClip.VolumeEnvelope.Evaluate(time) * _volume;
			}
		}
	}

	private VirtualAudioSource StealLeastImportantSource()
	{
		KeyValuePair<WindClip, VirtualAudioSource> keyValuePair = default(KeyValuePair<WindClip, VirtualAudioSource>);
		float num = 1f;
		foreach (KeyValuePair<WindClip, VirtualAudioSource> source in _sources)
		{
			if (source.Value.Volume < num)
			{
				keyValuePair = source;
				num = source.Value.Volume;
			}
		}
		_sources.Remove(keyValuePair.Key);
		return keyValuePair.Value;
	}
}
