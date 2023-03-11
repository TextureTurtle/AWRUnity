using UnityEngine;

public class BalloonJoint : MonoBehaviour
{
	[SerializeField]
	private bool _showDebugGUI;

	[SerializeField]
	private float _springStrength = 1f;

	[SerializeField]
	private float _springExponent = 1.5f;

	[SerializeField]
	private float _dampeningStrength = 1f;

	[SerializeField]
	private float _maxSpringForce = 5000f;

	[SerializeField]
	private float _minDistance = 5f;

	[SerializeField]
	private float _maxDistance = 6f;

	[SerializeField]
	private Rigidbody _bodyA;

	[SerializeField]
	private Rigidbody _bodyB;

	[SerializeField]
	private Vector3 _attachPointB;

	private float _distance;

	private float _workDistanceNormalized;

	private float _springForce;

	private float _dampeningForce;

	private Vector3 _forceVectorA;

	private Vector3 _forceVectorB;

	private float _previousDistance;

	public float SpringStrength
	{
		get
		{
			return _springStrength;
		}
		set
		{
			_springStrength = value;
		}
	}

	public float SpringExponent
	{
		get
		{
			return _springExponent;
		}
		set
		{
			_springExponent = value;
		}
	}

	public float DampeningStrength
	{
		get
		{
			return _dampeningStrength;
		}
		set
		{
			_dampeningStrength = value;
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
			_minDistance = value;
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
			_maxDistance = value;
		}
	}

	public Rigidbody BodyB
	{
		get
		{
			return _bodyB;
		}
		set
		{
			_bodyB = value;
		}
	}

	public Rigidbody BodyA
	{
		get
		{
			return _bodyA;
		}
		set
		{
			_bodyA = value;
		}
	}

	public Vector3 AttachPointB
	{
		get
		{
			return _attachPointB;
		}
		set
		{
			_attachPointB = value;
		}
	}

	public Vector3 AttachPointA
	{
		get
		{
			return base.transform.position;
		}
	}

	public float Distance
	{
		get
		{
			return _distance;
		}
	}

	private void Awake()
	{
		if (!_bodyA)
		{
			_bodyA = PhysicsUtils.FindRigidBodyUpwards(base.transform);
		}
		Vector3 position = base.transform.position;
		Vector3 vector = _bodyB.transform.TransformPoint(_attachPointB);
		_distance = (vector - position).magnitude;
		_previousDistance = _distance;
	}

	private void FixedUpdate()
	{
		Vector3 position = base.transform.position;
		Vector3 vector = ((!_bodyB) ? _attachPointB : _bodyB.transform.TransformPoint(_attachPointB));
		Vector3 vector2 = vector - position;
		Vector3 normalized = vector2.normalized;
		_distance = vector2.magnitude;
		float num = _maxDistance - _minDistance;
		_workDistanceNormalized = Mathf.Clamp(_distance - _minDistance, 0f, float.MaxValue) / num;
		_springForce = Mathf.Clamp(Mathf.Pow(_workDistanceNormalized, _springExponent) * _springStrength, 0f, _maxSpringForce);
		float num2 = _distance - _previousDistance;
		_dampeningForce = Mathf.Clamp(num2 * _dampeningStrength * _workDistanceNormalized, 0f, _maxSpringForce);
		Vector3 vector3 = normalized * _dampeningForce;
		Vector3 vector4 = normalized * _springForce;
		_forceVectorA = vector4 + vector3;
		_forceVectorB = -vector4 - vector3;
		_bodyA.AddForceAtPosition(_forceVectorA, position);
		if ((bool)_bodyB)
		{
			_bodyB.AddForceAtPosition(_forceVectorB, vector);
		}
		_previousDistance = _distance;
	}

	private void OnDrawGizmos()
	{
		if ((bool)_bodyA)
		{
			Vector3 position = base.transform.position;
			Vector3 vector = ((!_bodyB) ? _attachPointB : _bodyB.transform.TransformPoint(_attachPointB));
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere(position, 0.1f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(vector, 0.1f);
			Gizmos.color = Color.Lerp(Color.white, Color.red, _workDistanceNormalized);
			Gizmos.DrawLine(position, vector);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(position, _forceVectorA / _bodyA.mass);
			if ((bool)BodyB)
			{
				Gizmos.DrawRay(vector, _forceVectorB / BodyB.mass);
			}
		}
	}

	private void OnGUI()
	{
		if (_showDebugGUI)
		{
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Label(string.Format("Distance: {0:0.0}", _distance));
			GUILayout.Label(string.Format("Normalized Work Distance: {0:0.0}", _workDistanceNormalized));
			GUILayout.Label(string.Format("Spring Force: {0:0.0}", _springForce));
			GUILayout.Label(string.Format("Dampening Force: {0:0.0}", _dampeningForce));
			GUILayout.EndVertical();
		}
	}
}
