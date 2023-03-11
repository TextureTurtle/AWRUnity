using System;
using System.Collections;
using UnityEngine;

public class VirtualRoundRobinSound : MonoBehaviour
{
	[SerializeField]
	private VirtualAudioSource _source;

	[SerializeField]
	private AudioClip[] _clips;

	[SerializeField]
	private bool _playAutomatically = true;

	[SerializeField]
	private float _minDelay;

	[SerializeField]
	private float _maxDelay;

	private System.Random _random;

	private void Start()
	{
		_random = new System.Random();
		if (_playAutomatically)
		{
			StartCoroutine(PlayAutomaticallyAsync());
		}
	}

	public void Play()
	{
		Play(1f);
	}

	public void Play(float volumeScale)
	{
		_source.PlayOneShot(_clips[_random.Next(_clips.Length - 1)], volumeScale);
	}

	private IEnumerator PlayAutomaticallyAsync()
	{
		while (true)
		{
			Play();
			yield return new WaitForSeconds(_minDelay + (float)_random.NextDouble() * (_maxDelay - _minDelay));
		}
	}
}
