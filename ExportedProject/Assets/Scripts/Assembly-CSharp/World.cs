using System;
using System.Collections.Generic;
using UnityEngine;

public class World : SingletonComponent<World>
{
	[SerializeField]
	private double _sceneOriginStartX;

	[SerializeField]
	private double _sceneOriginStartZ;

	[SerializeField]
	private WorldTransform _perspective;

	[SerializeField]
	private float _snapDistance = 1000f;

	[SerializeField]
	private bool _disableVerticalSnapping = true;

	private List<WorldTransform> _managedTransforms;

	private DVector3 _sceneOriginPosition;

	public DVector3 SceneOriginPosition
	{
		get
		{
			return _sceneOriginPosition;
		}
		set
		{
			_sceneOriginPosition = value;
		}
	}

	public event Action<Vector3> OnSceneMove;

	public DVector3 LocalPointToWorld(Vector3 local)
	{
		return _sceneOriginPosition + new DVector3(local);
	}

	public Vector3 WorldPointToLocal(DVector3 world)
	{
		return DVector3.ToVector3(world - _sceneOriginPosition);
	}

	public static void Register(WorldTransform worldTransform)
	{
		if ((bool)SingletonComponent<World>.Instance)
		{
			SingletonComponent<World>.Instance.RegisterInternal(worldTransform);
		}
	}

	private void RegisterInternal(WorldTransform worldTransform)
	{
		_managedTransforms.Add(worldTransform);
	}

	public static void Unregister(WorldTransform worldTransform)
	{
		if ((bool)SingletonComponent<World>.Instance)
		{
			SingletonComponent<World>.Instance.UnregisterInternal(worldTransform);
		}
	}

	private void UnregisterInternal(WorldTransform worldTransform)
	{
		_managedTransforms.Remove(worldTransform);
	}

	private void Awake()
	{
		_sceneOriginPosition = new DVector3(_sceneOriginStartX, 0.0, _sceneOriginStartZ);
		Debug.Log("Scene starting at: " + _sceneOriginPosition);
		_managedTransforms = new List<WorldTransform>();
	}

	private void Update()
	{
		Vector3 position = _perspective.transform.position;
		if (_disableVerticalSnapping)
		{
			position.y = 0f;
		}
		float largestSingleAxisDistance = GetLargestSingleAxisDistance(Vector3.zero, position);
		if (largestSingleAxisDistance > _snapDistance)
		{
			MoveScene(position);
		}
	}

	private void MoveScene(Vector3 delta)
	{
		Debug.Log("MoveScene");
		foreach (WorldTransform managedTransform in _managedTransforms)
		{
			managedTransform.OnSceneMove(-delta);
		}
		if (this.OnSceneMove != null)
		{
			this.OnSceneMove(-delta);
		}
		_sceneOriginPosition += new DVector3(delta);
	}

	private float GetLargestSingleAxisDistance(Vector3 a, Vector3 b)
	{
		return Mathf.Max(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y), Mathf.Abs(b.z - a.z));
	}
}
