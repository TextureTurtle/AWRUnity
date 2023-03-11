using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamlineEmitter : MonoBehaviour
{
	private class Streamline
	{
		public readonly Transform Transform;

		public readonly AdaptiveTrailRenderer TrailRenderer;

		public float StartTime;

		public float LastUpdateTime;

		public float LifeTime
		{
			get
			{
				return Time.time - StartTime;
			}
		}

		public Streamline(Transform transform, AdaptiveTrailRenderer renderer)
		{
			Transform = transform;
			TrailRenderer = renderer;
			StartTime = Time.time;
			LastUpdateTime = StartTime;
		}
	}

	[SerializeField]
	private Game _game;

	[SerializeField]
	private Transform _streamlinePrefab;

	[SerializeField]
	private int _poolSize;

	[SerializeField]
	private float _rate = 1f;

	[SerializeField]
	private float _lifeTime = 10f;

	[SerializeField]
	private float _speedMultiplier = 1f;

	[SerializeField]
	private float _horizontalRadius = 2000f;

	[SerializeField]
	private float _verticalRadius = 1000f;

	[SerializeField]
	private AnimationCurve _distributionCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	private float _minWidth = 1f;

	[SerializeField]
	private float _maxWidth = 5f;

	[SerializeField]
	private AnimationCurve _alphaOverLifeTime = AnimationCurve.Linear(0f, 1f, 1f, 1f);

	private Vector3 _center;

	private System.Random _random;

	private List<Streamline> _streamlines;

	private List<Vector3> _cameraPositions;

	private void Awake()
	{
		_lifeTime = Mathf.Max(_lifeTime, (float)_poolSize / _rate - 1f);
		_random = new System.Random();
		_streamlines = new List<Streamline>(_poolSize);
		_cameraPositions = new List<Vector3>(2);
	}

	private void Start()
	{
		SingletonComponent<Game>.Instance.OnGameStarted += OnGameStarted;
		SingletonComponent<Game>.Instance.OnGameOver += OnGameOver;
		SingletonComponent<World>.Instance.OnSceneMove += OnSceneMove;
	}

	private void OnDestroy()
	{
		if ((bool)SingletonComponent<Game>.Instance)
		{
			SingletonComponent<Game>.Instance.OnGameStarted -= OnGameStarted;
			SingletonComponent<Game>.Instance.OnGameOver -= OnGameOver;
		}
		if ((bool)SingletonComponent<World>.Instance)
		{
			SingletonComponent<World>.Instance.OnSceneMove -= OnSceneMove;
		}
	}

	private void Prewarm()
	{
	}

	private IEnumerator SpawnParticlesAsync()
	{
		while (true)
		{
			SpawnParticle();
			float delay = 1f / _rate;
			yield return new WaitForSeconds(delay);
		}
	}

	private Streamline SpawnParticle()
	{
		_center = Vector3.zero;
		Player[] players = _game.Players;
		foreach (Player player in players)
		{
			_center += player.transform.position;
		}
		_center /= (float)_game.Players.Length;
		Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		float num = _distributionCurve.Evaluate((float)_random.NextDouble());
		insideUnitSphere.x *= num * _horizontalRadius;
		insideUnitSphere.y *= num * _verticalRadius;
		insideUnitSphere.z *= num * _horizontalRadius;
		Vector3 position = _center + insideUnitSphere;
		Transform transform = PoolManager.Pools["Particles"].Spawn(_streamlinePrefab);
		transform.position = position;
		AdaptiveTrailRenderer component = transform.GetComponent<AdaptiveTrailRenderer>();
		Streamline streamline = new Streamline(transform, component);
		streamline.TrailRenderer.Emit();
		_streamlines.Add(streamline);
		return streamline;
	}

	private void Update()
	{
		List<PlayerCamera> cameras = SingletonComponent<CameraManager>.Instance.Cameras;
		_cameraPositions.Clear();
		for (int i = 0; i < cameras.Count; i++)
		{
			_cameraPositions.Add(cameras[i].transform.position);
		}
		float lifeTimeFactor = 1f / _lifeTime;
		AeroManager instance = SingletonComponent<AeroManager>.Instance;
		for (int j = 0; j < _streamlines.Count; j++)
		{
			Streamline streamline = _streamlines[j];
			if (streamline.LifeTime > _lifeTime)
			{
				streamline.TrailRenderer.Stop();
				PoolManager.Pools["Particles"].Despawn(streamline.Transform);
				_streamlines.RemoveAt(j);
				j--;
				continue;
			}
			float num = float.MaxValue;
			for (int k = 0; k < _cameraPositions.Count; k++)
			{
				float sqrMagnitude = (_cameraPositions[k] - streamline.Transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
				}
			}
			float num2 = Mathf.Lerp(0.02f, 0.33f, num * 1.2E-06f);
			if (Time.time > streamline.LastUpdateTime + num2)
			{
				Vector3 windVelocity = instance.GetWindVelocity(streamline.Transform.position, false);
				UpdateTrail(streamline, windVelocity, _cameraPositions, lifeTimeFactor);
			}
		}
	}

	private void UpdateTrail(Streamline streamline, Vector3 windVector, List<Vector3> viewPositions, float lifeTimeFactor)
	{
		Transform transform = streamline.Transform;
		transform.rotation = Quaternion.LookRotation(windVector);
		transform.Translate(windVector * _speedMultiplier * (Time.time - streamline.LastUpdateTime), Space.World);
		AdaptiveTrailRenderer trailRenderer = streamline.TrailRenderer;
		trailRenderer.WidthMultiplier = Mathf.Lerp(_minWidth, _maxWidth, windVector.magnitude / 40f);
		trailRenderer.SetAlpha(_alphaOverLifeTime.Evaluate(streamline.LifeTime * lifeTimeFactor));
		trailRenderer.UpdateMesh(viewPositions);
		streamline.LastUpdateTime = Time.time;
	}

	private void OnGameStarted()
	{
		Prewarm();
		StartCoroutine(SpawnParticlesAsync());
	}

	private void OnGameOver()
	{
		StopAllCoroutines();
	}

	private void OnSceneMove(Vector3 delta)
	{
		List<PlayerCamera> cameras = SingletonComponent<CameraManager>.Instance.Cameras;
		_cameraPositions.Clear();
		for (int i = 0; i < cameras.Count; i++)
		{
			_cameraPositions.Add(cameras[i].transform.position);
		}
		for (int j = 0; j < _streamlines.Count; j++)
		{
			Streamline streamline = _streamlines[j];
			streamline.Transform.Translate(delta, Space.World);
			streamline.TrailRenderer.TranslatePoints(delta, _cameraPositions);
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(_center, _horizontalRadius);
		}
	}
}
