using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveTrailRenderer : MonoBehaviour
{
	[Serializable]
	public class Segment
	{
		public Color Color;

		public float Width;
	}

	private class Point
	{
		private float _timeCreated;

		public Vector3 Position;

		public Quaternion Rotation;

		public float LifeTime
		{
			get
			{
				return Time.time - _timeCreated;
			}
		}

		public void Create()
		{
			_timeCreated = Time.time;
		}

		public void Update(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}
	}

	private const string TintColorPropery = "_TintColor";

	[SerializeField]
	private Material _material;

	private Material _instanceMaterial;

	[SerializeField]
	private float _maxPointLifeTime = 1f;

	[SerializeField]
	private Segment[] _segments = new Segment[3]
	{
		new Segment
		{
			Color = new Color(1f, 1f, 1f, 0f)
		},
		new Segment
		{
			Color = new Color(1f, 1f, 1f, 0f),
			Width = 1f
		},
		new Segment
		{
			Color = new Color(1f, 1f, 1f, 0f)
		}
	};

	[SerializeField]
	private float _widthMultiplier = 1f;

	[SerializeField]
	private float _angleLimit = 2f;

	[SerializeField]
	private float _minVertexDistance = 0.1f;

	[SerializeField]
	private float _maxVertexDistance = 1f;

	[SerializeField]
	private int _maxPoints = 128;

	private GameObject _trailObject;

	private Mesh _mesh;

	private Vector3[] _vertices;

	private Vector2[] _uvs;

	private int[] _triangles;

	private Color[] _meshColors;

	private Point[] _points;

	private int _pointCount;

	private int _startIndex;

	private bool _isEmitting;

	public float WidthMultiplier
	{
		get
		{
			return _widthMultiplier;
		}
		set
		{
			_widthMultiplier = value;
		}
	}

	public bool IsEmitting
	{
		get
		{
			return _isEmitting;
		}
	}

	private void Awake()
	{
		if (_segments.Length < 3)
		{
			Debug.LogError("At least three segment profiles need to be defined!");
		}
		_pointCount = Mathf.ClosestPowerOfTwo(_pointCount);
		_trailObject = new GameObject("Trail");
		MeshFilter meshFilter = _trailObject.AddComponent<MeshFilter>();
		_mesh = meshFilter.mesh;
		_mesh.MarkDynamic();
		_trailObject.AddComponent<MeshRenderer>();
		_instanceMaterial = new Material(_material);
		_trailObject.renderer.material = _instanceMaterial;
		_trailObject.renderer.castShadows = false;
		_trailObject.renderer.receiveShadows = false;
		_trailObject.renderer.enabled = false;
		_vertices = new Vector3[(_maxPoints + 1) * 2];
		_uvs = new Vector2[(_maxPoints + 1) * 2];
		_triangles = new int[_maxPoints * 6];
		_meshColors = new Color[(_maxPoints + 1) * 2];
		_points = new Point[_maxPoints];
		for (int i = 0; i < _maxPoints; i++)
		{
			_points[i] = new Point();
		}
		Reset();
	}

	public void Emit()
	{
		_trailObject.renderer.enabled = true;
		_isEmitting = true;
	}

	public void Stop()
	{
		_trailObject.renderer.enabled = false;
		_isEmitting = false;
		Reset();
	}

	public void Stop(Action doneAction)
	{
		Fade(0f, 1f, delegate
		{
			_trailObject.renderer.enabled = false;
			Reset();
			doneAction();
		});
		_isEmitting = false;
	}

	public void TranslatePoints(Vector3 offset, List<Vector3> viewPositions)
	{
		if (IsEmitting)
		{
			for (int i = 0; i < _pointCount; i++)
			{
				_points[ToCircularIndex(i)].Position += offset;
			}
			RebuildMesh(viewPositions);
		}
	}

	public void UpdateMesh(List<Vector3> viewPositions)
	{
		TryRemoveOldPoints();
		TryAddNewPoints();
		RebuildMesh(viewPositions);
	}

	private void TryRemoveOldPoints()
	{
		bool flag = false;
		while (!flag)
		{
			if (_pointCount > 2 && _points[ToCircularIndex(0)].LifeTime >= _maxPointLifeTime)
			{
				RemovePointAtTail();
			}
			else
			{
				flag = true;
			}
		}
	}

	private void TryAddNewPoints()
	{
		Point point = _points[ToCircularIndex(_pointCount - 2)];
		float magnitude = (point.Position - base.transform.position).magnitude;
		if (magnitude > _minVertexDistance && (magnitude > _maxVertexDistance || Quaternion.Angle(base.transform.rotation, point.Rotation) > _angleLimit))
		{
			AddPointAtHead();
		}
		Point point2 = _points[ToCircularIndex(_pointCount - 1)];
		point2.Update(base.transform.position, base.transform.rotation);
	}

	private void AddPointAtHead()
	{
		if (_pointCount >= _maxPoints)
		{
			RemovePointAtTail();
		}
		Point point = _points[ToCircularIndex(_pointCount)];
		point.Create();
		point.Update(base.transform.position, base.transform.rotation);
		_pointCount++;
	}

	private void RemovePointAtTail()
	{
		_startIndex = Mathx.FastSqrModulo(_startIndex + 1, _maxPoints);
		_pointCount--;
	}

	private void Reset()
	{
		_pointCount = 0;
		_startIndex = 0;
	}

	private void RebuildMesh(List<Vector3> viewPositions)
	{
		if (_pointCount < 2)
		{
			return;
		}
		int pointCount = _pointCount;
		int maxPoints = _maxPoints;
		Point[] points = _points;
		Segment[] segments = _segments;
		float widthMultiplier = _widthMultiplier;
		Vector3[] vertices = _vertices;
		Vector2[] uvs = _uvs;
		int[] triangles = _triangles;
		Color[] meshColors = _meshColors;
		Point point = points[ToCircularIndex(0)];
		Point point2 = points[ToCircularIndex(pointCount - 1)];
		float num = 1f / (point2.LifeTime - point.LifeTime);
		float num2 = 1f / (float)pointCount * (float)(segments.Length - 1);
		Vector3 rhs = base.transform.rotation * Vector3.forward;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < viewPositions.Count; i++)
		{
			Vector3 normalized = (base.transform.position - viewPositions[i]).normalized;
			zero += normalized * Mathf.Sign(Vector3.Dot(normalized, rhs));
		}
		zero /= (float)viewPositions.Count;
		Vector3 vector = Vector3.Cross(zero, rhs);
		for (int j = 0; j < pointCount; j++)
		{
			Point point3 = points[ToCircularIndex(j)];
			float num3 = (float)j * num2;
			int num4 = (int)num3;
			int num5 = num4 + 1;
			float t = num3 - (float)num4;
			Segment segment = segments[num4];
			Segment segment2 = segments[num5];
			int num6 = j * 2;
			Color color = Color.Lerp(segment.Color, segment2.Color, t);
			meshColors[num6] = color;
			meshColors[num6 + 1] = color;
			float num7 = Mathf.Lerp(segment.Width, segment2.Width, t) * widthMultiplier;
			Vector3 vector2 = vector * (num7 * 0.5f);
			vertices[num6] = point3.Position + vector2;
			vertices[num6 + 1] = point3.Position - vector2;
			float x = (point3.LifeTime - point.LifeTime) * num;
			uvs[num6] = new Vector2(x, 0f);
			uvs[num6 + 1] = new Vector2(x, 1f);
			if (j > 0)
			{
				int num8 = (j - 1) * 6;
				triangles[num8] = num6 - 2;
				triangles[num8 + 1] = num6 - 1;
				triangles[num8 + 2] = num6;
				triangles[num8 + 3] = num6 + 1;
				triangles[num8 + 4] = num6;
				triangles[num8 + 5] = num6 - 1;
			}
		}
		for (int k = pointCount; k < maxPoints; k++)
		{
			int num9 = (k - 1) * 6;
			triangles[num9] = 0;
			triangles[num9 + 1] = 0;
			triangles[num9 + 2] = 0;
			triangles[num9 + 3] = 0;
			triangles[num9 + 4] = 0;
			triangles[num9 + 5] = 0;
		}
		_mesh.MarkDynamic();
		_mesh.vertices = _vertices;
		_mesh.triangles = _triangles;
		_mesh.uv = _uvs;
		_mesh.colors = _meshColors;
		_mesh.RecalculateBounds();
	}

	private int ToCircularIndex(int index)
	{
		return Mathx.FastSqrModulo(_startIndex + index, _maxPoints);
	}

	public void SetAlpha(float alpha)
	{
		Color color = _instanceMaterial.GetColor("_TintColor");
		color.a = alpha;
		_instanceMaterial.SetColor("_TintColor", color);
	}

	public void Fade(float targetAlpha, float time, Action doneAction)
	{
		StopAllCoroutines();
		StartCoroutine(FadeAsync(targetAlpha, time, doneAction));
	}

	private IEnumerator FadeAsync(float targetAlpha, float time, Action doneAction)
	{
		Color color = _instanceMaterial.GetColor("_TintColor");
		float startAlpha = color.a;
		float timer = 0f;
		while (timer < time)
		{
			color.a = Mathf.Lerp(startAlpha, targetAlpha, timer / time);
			_instanceMaterial.SetColor("_TintColor", color);
			timer += 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		color.a = targetAlpha;
		_instanceMaterial.SetColor("_TintColor", color);
		if (doneAction != null)
		{
			doneAction();
		}
	}
}
