using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingHookLauncher : MonoBehaviour
{
	public enum LauncherState
	{
		Ready = 0,
		Shooting = 1,
		Latched = 2,
		Reeling = 3,
		Released = 4
	}

	[SerializeField]
	private Controllable _controllable;

	[SerializeField]
	private Pickup _pickup;

	[SerializeField]
	private BalloonJoint _balloonJoint;

	[SerializeField]
	private Transform _turretTransform;

	[SerializeField]
	private Transform _grapplingHookRest;

	[SerializeField]
	private GrapplingHook _grapplingHook;

	[SerializeField]
	private float _maxTurretAngle = 75f;

	[SerializeField]
	private float _rotationSpeed = 3f;

	[SerializeField]
	private float _maxRopeLength = 200f;

	[SerializeField]
	private float _minRopeLength = 1f;

	[SerializeField]
	private float _ropeFlex = 1f;

	[SerializeField]
	private float _reelSpeed = 3f;

	private Transform _aimTransform;

	private LauncherState _state;

	private float _ropeLength;

	private Vector3 _playerAimPoint;

	private Vector3 _turretAimPoint;

	private float _turretAngle;

	public Vector3 TurretAimPoint
	{
		get
		{
			return _turretAimPoint;
		}
	}

	public event Action OnShoot;

	public event Action OnStartReel;

	public event Action OnStopReel;

	public event Action OnDoneReeling;

	private void Start()
	{
		Reset();
		_controllable.OnApproveControl += OnApproveControl;
		_controllable.OnControlTaken += OnControlTaken;
		_controllable.OnControlReleased += OnControlReleased;
		_controllable.OnUse += OnUse;
		_controllable.OnPickup += OnPickup;
		_controllable.OnCancel += OnCancel;
		_pickup.OnMounted += OnMounted;
		_pickup.OnUnmounted += OnUnmounted;
		_pickup.OnApprovePickup += ApprovePickup;
		_grapplingHook.OnLatched += OnLatched;
		_grapplingHook.OnReleasedFromOutside += OnHookReleasedFromOutside;
	}

	private void OnApproveControl(ref Approval approval)
	{
		approval.Approve(_pickup.IsMounted);
	}

	private void OnControlTaken(Player player)
	{
		_aimTransform = player.Camera.transform;
	}

	private void OnControlReleased(Player player)
	{
		_aimTransform = null;
	}

	private void ApprovePickup(ref Approval approval)
	{
		approval.Approve(!_controllable.IsControlled && _state == LauncherState.Ready);
	}

	private void OnMounted(GearMount mount)
	{
		_balloonJoint.BodyA = mount.MountBody;
	}

	private void OnUnmounted(GearMount mount)
	{
		_balloonJoint.BodyA = base.rigidbody;
	}

	private void OnLatched(GameObject go)
	{
		_state = LauncherState.Latched;
		_ropeLength = Vector3.Distance(_turretTransform.position, _grapplingHook.transform.position) + 3f;
	}

	public void OnUse(Player player)
	{
		switch (_state)
		{
		case LauncherState.Ready:
			Shoot();
			break;
		case LauncherState.Shooting:
		case LauncherState.Latched:
		case LauncherState.Reeling:
			Release();
			Reset();
			break;
		case LauncherState.Released:
			Reset();
			break;
		}
	}

	public void OnPickup(Player player)
	{
		switch (_state)
		{
		case LauncherState.Latched:
			StartReeling();
			break;
		case LauncherState.Reeling:
			StopReeling();
			break;
		}
	}

	public void OnCancel(Player player)
	{
		_controllable.ReleaseControl();
	}

	private void Reset()
	{
		Debug.Log("Launcher reset");
		_state = LauncherState.Ready;
		_grapplingHook.Reset();
		_balloonJoint.enabled = false;
		_grapplingHook.transform.parent = _grapplingHookRest.transform;
		_grapplingHook.transform.position = _grapplingHookRest.transform.position;
		_grapplingHook.transform.rotation = _grapplingHookRest.transform.rotation;
	}

	private void Shoot()
	{
		_balloonJoint.enabled = true;
		_ropeLength = _maxRopeLength;
		SetJointLength(_maxRopeLength);
		_grapplingHook.transform.parent = null;
		float num = _grapplingHook.Shoot(_balloonJoint, _controllable.Controller);
		_balloonJoint.BodyA.AddForce(-base.transform.forward * num, ForceMode.Impulse);
		_state = LauncherState.Shooting;
		if (this.OnShoot != null)
		{
			this.OnShoot();
		}
	}

	private void StartReeling()
	{
		Debug.Log("Launcher started reeling");
		_state = LauncherState.Reeling;
		_grapplingHook.OnStartReel();
		if (this.OnStartReel != null)
		{
			this.OnStartReel();
		}
	}

	private void StopReeling()
	{
		Debug.Log("Launcher stopped reeling");
		_state = LauncherState.Latched;
		if (this.OnStopReel != null)
		{
			this.OnStopReel();
		}
	}

	private void Release()
	{
		Debug.Log("Launcher released");
		_state = LauncherState.Released;
		_grapplingHook.Release();
	}

	private void OnHookReleasedFromOutside()
	{
		Reset();
	}

	private void Update()
	{
		if (!_pickup.IsMounted)
		{
			return;
		}
		Quaternion to = _pickup.Mount.transform.rotation;
		switch (_state)
		{
		case LauncherState.Ready:
			if (_controllable.IsControlled)
			{
				_playerAimPoint = RaycastForPoint(_aimTransform.position, _aimTransform.forward);
				Quaternion rotation = _pickup.Mount.transform.rotation;
				Quaternion quaternion = Quaternion.LookRotation(_playerAimPoint - _turretTransform.position);
				_turretAngle = Quaternion.Angle(quaternion, rotation);
				to = Quaternion.RotateTowards(rotation, quaternion, Mathf.Clamp(_turretAngle, 0f, _maxTurretAngle));
				_turretAimPoint = RaycastForPoint(_turretTransform.position, _turretTransform.forward);
			}
			break;
		case LauncherState.Shooting:
		case LauncherState.Latched:
		case LauncherState.Reeling:
		case LauncherState.Released:
			to = Quaternion.LookRotation(_grapplingHook.transform.position - _turretTransform.position);
			break;
		}
		_turretTransform.rotation = Quaternion.Slerp(_turretTransform.rotation, to, _rotationSpeed * Time.deltaTime);
		if (_state == LauncherState.Shooting && _balloonJoint.Distance >= _maxRopeLength)
		{
			_grapplingHook.OnMaxLengthReached();
			_state = LauncherState.Released;
		}
		if (_state != LauncherState.Reeling)
		{
			return;
		}
		_ropeLength = Mathf.Clamp(_ropeLength - _reelSpeed * Time.deltaTime, _minRopeLength, _maxRopeLength);
		SetJointLength(_ropeLength);
		if (_ropeLength <= 0f)
		{
			_state = LauncherState.Released;
			if (this.OnDoneReeling != null)
			{
				this.OnDoneReeling();
			}
		}
	}

	private Vector3 RaycastForPoint(Vector3 position, Vector3 direction)
	{
		RaycastHit[] collection = Physics.RaycastAll(position, direction, _maxRopeLength, ~(LayerMasks.Launcher | LayerMasks.Ship | LayerMasks.Player));
		List<RaycastHit> source = new List<RaycastHit>(collection);
		source = source.OrderBy((RaycastHit hit) => hit.distance).ToList();
		List<Collider> physicalColliders = _pickup.PhysicalColliders;
		foreach (RaycastHit item2 in source)
		{
			Collider item = item2.collider;
			if (!physicalColliders.Contains(item))
			{
				return item2.point;
			}
		}
		return position + direction * _maxRopeLength;
	}

	private void SetJointLength(float length)
	{
		_balloonJoint.MaxDistance = length;
		_balloonJoint.MinDistance = Mathf.Clamp(length - _ropeFlex, 0f, length);
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying && _controllable.IsControlled)
		{
			if (_turretAngle < _maxTurretAngle)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(_playerAimPoint, 0.5f);
			}
			Gizmos.color = Color.red;
			Gizmos.DrawLine(base.transform.position, _turretAimPoint);
			Gizmos.DrawSphere(_turretAimPoint, 0.5f);
		}
	}
}
