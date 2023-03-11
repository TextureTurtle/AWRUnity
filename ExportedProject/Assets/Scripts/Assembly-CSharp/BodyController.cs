using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
	private const int RaycastDensity = 2;

	private const float ColliderWidth = 1f;

	private const float StepSize = 1f;

	[SerializeField]
	private float _walkSpeed = 5f;

	[SerializeField]
	private float _runSpeed = 10f;

	[SerializeField]
	private float _jumpSpeed = 10f;

	[SerializeField]
	private float _airSpeed = 3f;

	[SerializeField]
	private float _maxAcceleration = 5f;

	[SerializeField]
	private float _maxSlopeAngle = 45f;

	private bool _walkEnabled = true;

	private Vector3 _walkInput;

	private bool _sprint;

	private bool _jumpInput;

	private List<Collision> _collisions;

	private bool _isGrounded;

	private Collider _groundCollider;

	private Rigidbody _groundBody;

	private Vector3 _relativeVelocity;

	private static readonly Vector3 BackLeft = new Vector3(-0.5f, 1f, -0.5f);

	public bool IsGrounded
	{
		get
		{
			return _isGrounded;
		}
	}

	public bool WalkEnabled
	{
		get
		{
			return _walkEnabled;
		}
		set
		{
			_walkEnabled = value;
		}
	}

	public Vector3 RelativeVelocity
	{
		get
		{
			return _relativeVelocity;
		}
	}

	public void Walk(float horizontal, float vertical, bool sprint)
	{
		if (_walkEnabled)
		{
			_walkInput.x = horizontal;
			_walkInput.z = vertical;
			_sprint = sprint;
		}
	}

	public void Look(float angle)
	{
		Quaternion rot = base.transform.rotation * Quaternion.Euler(0f, angle * Time.deltaTime, 0f);
		base.rigidbody.MoveRotation(rot);
	}

	public void Jump()
	{
		_jumpInput = true;
	}

	private void Awake()
	{
		_collisions = new List<Collision>();
	}

	private void FixedUpdate()
	{
		_isGrounded = false;
		Vector3 planeNormal = Vector3.up;
		foreach (Collision collision in _collisions)
		{
			ContactPoint[] contacts = collision.contacts;
			for (int i = 0; i < contacts.Length; i++)
			{
				ContactPoint contactPoint = contacts[i];
				if (Vector3.Angle(contactPoint.normal, Vector3.up) < _maxSlopeAngle)
				{
					_isGrounded = true;
					planeNormal = contactPoint.normal;
					if (contactPoint.otherCollider != _groundCollider)
					{
						_groundCollider = contactPoint.otherCollider;
						_groundBody = PhysicsUtils.FindRigidBodyUpwards(contactPoint.otherCollider.transform);
					}
					break;
				}
			}
		}
		if (!_isGrounded)
		{
			int num = 0;
			while (!_isGrounded && num < 2)
			{
				int num2 = 0;
				while (!_isGrounded && num2 < 2)
				{
					Vector3 origin = base.transform.TransformPoint(BackLeft + Vector3.right * ((float)num * 1f) + Vector3.forward * ((float)num2 * 1f));
					RaycastHit hitInfo;
					if (Physics.Raycast(origin, -base.transform.up, out hitInfo, 1.1f) && Vector3.Angle(hitInfo.normal, base.transform.up) < _maxSlopeAngle)
					{
						_isGrounded = true;
						planeNormal = hitInfo.normal;
						Collider collider = hitInfo.collider;
						if (collider != _groundCollider)
						{
							_groundCollider = collider;
							_groundBody = PhysicsUtils.FindRigidBodyUpwards(collider.transform);
						}
					}
					num2++;
				}
				num++;
			}
		}
		if (!_isGrounded)
		{
			_groundCollider = null;
			_groundBody = null;
		}
		_relativeVelocity = base.rigidbody.velocity - ((!_groundBody) ? Vector3.zero : _groundBody.velocity);
		if (_isGrounded)
		{
			float num3 = ((!_sprint) ? _walkSpeed : _runSpeed);
			Vector3 inVector = base.transform.TransformDirection(_walkInput) * num3;
			Vector3 vector = Mathx.ProjectOnPlane(inVector, planeNormal);
			Vector3 vector2 = ((!(_groundBody != null)) ? Vector3.zero : _groundBody.velocity);
			Debug.DrawRay(base.transform.position, vector2, Color.blue);
			Vector3 vector3 = vector + vector2;
			Vector3 vector4 = Vector3.ClampMagnitude((vector3 - base.rigidbody.velocity) * 5f, _maxAcceleration * 5f);
			vector4.y = 0f;
			if (_jumpInput && _isGrounded)
			{
				Vector3 vector5 = Vector3.up * _jumpSpeed * base.rigidbody.mass;
				base.rigidbody.AddForce(vector5, ForceMode.Impulse);
				if (_groundBody != null)
				{
					_groundBody.AddForce(vector5 * 0.33f, ForceMode.Impulse);
				}
			}
			foreach (Collision collision2 in _collisions)
			{
				ContactPoint[] contacts2 = collision2.contacts;
				for (int j = 0; j < contacts2.Length; j++)
				{
					ContactPoint contactPoint2 = contacts2[j];
					if ((bool)contactPoint2.otherCollider)
					{
						Rigidbody rigidbody = PhysicsUtils.FindRigidBodyUpwards(contactPoint2.otherCollider.transform);
						if (rigidbody == _groundBody && Vector3.Angle(contactPoint2.normal, Vector3.up) > _maxSlopeAngle && Vector3.Dot(vector4.normalized, contactPoint2.normal) < 0f)
						{
							Debug.DrawRay(contactPoint2.point, contactPoint2.normal, Color.red);
							vector4 -= Vector3.Project(vector4, -contactPoint2.normal);
						}
					}
				}
			}
			Vector3 vector6 = vector4 * base.rigidbody.mass;
			base.rigidbody.AddForce(vector6);
			Debug.DrawRay(base.transform.position, vector6 / base.rigidbody.mass, Color.green);
			if ((bool)_groundBody)
			{
				_groundBody.AddForceAtPosition(-vector6, base.transform.position);
				Debug.DrawRay(base.transform.position, -vector4, Color.yellow);
			}
		}
		else
		{
			Vector3 vector7 = base.transform.TransformDirection(_walkInput) * _airSpeed;
			vector7.y = 0f;
			Vector3 velocity = base.rigidbody.velocity;
			velocity.y = 0f;
			if (Vector3.Dot(vector7, velocity) > 0f)
			{
				vector7 -= Vector3.Project(vector7, base.rigidbody.velocity);
			}
			Vector3 force = vector7 * base.rigidbody.mass * 5f;
			base.rigidbody.AddForce(force);
		}
		_collisions.Clear();
		_walkInput = Vector3.zero;
		_sprint = false;
		_jumpInput = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		ContactPoint[] contacts = collision.contacts;
		foreach (ContactPoint contactPoint in contacts)
		{
			if (contactPoint.otherCollider == base.collider)
			{
				return;
			}
		}
		_collisions.Add(collision);
	}

	private void OnCollisionStay(Collision collision)
	{
		ContactPoint[] contacts = collision.contacts;
		foreach (ContactPoint contactPoint in contacts)
		{
			if (contactPoint.otherCollider == base.collider)
			{
				return;
			}
		}
		_collisions.Add(collision);
	}
}
