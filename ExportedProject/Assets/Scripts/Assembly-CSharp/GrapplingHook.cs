using System;
using RamjetAnvil.Unity.Utility;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(GearMount))]
public class GrapplingHook : MonoBehaviour
{
	public enum GrapplingHookState
	{
		Ready = 0,
		Shooting = 1,
		Latched = 2,
		Released = 3
	}

	[SerializeField]
	private GearMount _mount;

	[SerializeField]
	private float _shotImpulse = 100f;

	[SerializeField]
	private float _kickbackImpulse = 0.1f;

	[SerializeField]
	private float _reelDrag = 2f;

	[SerializeField]
	private Vector3 _tipOffset = new Vector3(0f, 0f, 0.5f);

	[SerializeField]
	private Vector3 _tailOffset = new Vector3(0f, 0f, -0.5f);

	private Player _player;

	private BalloonJoint _joint;

	private GrapplingHookState _state;

	private Rigidbody _latchedBody;

	private Pickup _latchedPickup;

	private float _originalDrag;

	public GrapplingHookState State
	{
		get
		{
			return _state;
		}
	}

	public event Action<GameObject> OnLatched;

	public event Action OnReleased;

	public event Action OnReleasedFromOutside;

	private void Start()
	{
		_state = GrapplingHookState.Ready;
		base.rigidbody.isKinematic = true;
		base.rigidbody.useGravity = false;
		base.rigidbody.collider.isTrigger = true;
	}

	public float Shoot(BalloonJoint joint, Player player)
	{
		_state = GrapplingHookState.Shooting;
		joint.BodyB = base.rigidbody;
		joint.AttachPointB = Vector3.zero;
		_joint = joint;
		_player = player;
		base.rigidbody.isKinematic = false;
		base.rigidbody.useGravity = false;
		base.rigidbody.drag = 0f;
		base.collider.isTrigger = false;
		base.rigidbody.AddForce(base.transform.forward * _shotImpulse, ForceMode.Impulse);
		return _kickbackImpulse;
	}

	public void Release()
	{
		Debug.Log("Claw released");
		_state = GrapplingHookState.Released;
		_joint.BodyB = base.rigidbody;
		_joint.AttachPointB = Vector3.zero;
		base.transform.parent = null;
		base.transform.localScale = Vector3.one;
		base.rigidbody.isKinematic = false;
		base.rigidbody.useGravity = true;
		base.collider.isTrigger = false;
		if ((bool)_latchedPickup && _latchedPickup.IsMounted)
		{
			_latchedPickup.OnPickUp -= OnLatchedObjectPickedUp;
			_latchedPickup.UnmountFrom(_mount);
		}
		if ((bool)_latchedBody)
		{
			_latchedBody.drag = _originalDrag;
			_latchedBody = null;
		}
		if (this.OnReleased != null)
		{
			this.OnReleased();
		}
	}

	public void Reset()
	{
		Debug.Log("Claw Reset");
		_state = GrapplingHookState.Ready;
		_player = null;
		base.rigidbody.isKinematic = true;
		base.rigidbody.useGravity = false;
		base.collider.isTrigger = true;
	}

	public void TryLatch(Collider otherCollider, Vector3 position, Vector3 normal)
	{
		_state = GrapplingHookState.Latched;
		Debug.Log("Latching onto: " + otherCollider.name);
		Pickup componentInParents = MonoBehaviourExtensions.GetComponentInParents<Pickup>(otherCollider.transform);
		if ((bool)componentInParents)
		{
			if (!componentInParents.CanMount(_player, _mount))
			{
				Release();
				return;
			}
			componentInParents.MountTo(_mount);
			_latchedPickup = componentInParents;
			_latchedPickup.OnPickUp += OnLatchedObjectPickedUp;
		}
		Quaternion rotation = Quaternion.LookRotation(-normal);
		PhysicsUtils.ReorientRigidbody(base.rigidbody, position + base.transform.TransformDirection(-_tipOffset), rotation);
		base.rigidbody.isKinematic = true;
		base.collider.isTrigger = true;
		base.transform.parent = otherCollider.transform;
		_latchedBody = PhysicsUtils.FindRigidBodyUpwards(otherCollider.transform);
		if ((bool)_latchedBody)
		{
			_joint.BodyB = _latchedBody;
			Vector3 position2 = base.transform.TransformPoint(_tailOffset);
			_joint.AttachPointB = _latchedBody.transform.InverseTransformPoint(position2);
		}
		else
		{
			_latchedBody = base.rigidbody;
			_joint.BodyB = null;
			_joint.AttachPointB = base.transform.TransformPoint(_tailOffset);
		}
		if (this.OnLatched != null)
		{
			this.OnLatched(otherCollider.gameObject);
		}
	}

	public void OnMaxLengthReached()
	{
		_state = GrapplingHookState.Released;
		base.rigidbody.useGravity = true;
	}

	public void OnStartReel()
	{
		_latchedBody.useGravity = true;
		_originalDrag = _latchedBody.drag;
		_latchedBody.drag = _reelDrag;
	}

	private void OnLatchedObjectPickedUp(Player player)
	{
		Debug.Log("OnLatchedObjectPickedUp");
		_latchedPickup.OnPickUp -= OnLatchedObjectPickedUp;
		_latchedPickup = null;
		Release();
		if (this.OnReleasedFromOutside != null)
		{
			this.OnReleasedFromOutside();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_state == GrapplingHookState.Shooting)
		{
			Collider otherCollider = collision.contacts[0].otherCollider;
			if (otherCollider != base.collider)
			{
				TryLatch(otherCollider, collision.contacts[0].point, collision.contacts[0].normal);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(base.transform.TransformPoint(_tailOffset), size);
		Gizmos.color = Color.red;
		Gizmos.DrawCube(base.transform.TransformPoint(_tipOffset), size);
	}
}
