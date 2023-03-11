using System;
using UnityEngine;

[RequireComponent(typeof(ItemDescriptor))]
public class GearMount : MonoBehaviour
{
	[SerializeField]
	private MountType _mountType;

	[SerializeField]
	private bool _isKinematicMount = true;

	[SerializeField]
	private Transform _mountPoint;

	private Rigidbody _mountBody;

	private Pickup _mountedItem;

	public MountType MountType
	{
		get
		{
			return _mountType;
		}
	}

	public bool IsKinematicMount
	{
		get
		{
			return _isKinematicMount;
		}
	}

	public Transform MountPoint
	{
		get
		{
			return _mountPoint;
		}
	}

	public Pickup MountedItem
	{
		get
		{
			return _mountedItem;
		}
	}

	public Rigidbody MountBody
	{
		get
		{
			return _mountBody;
		}
	}

	public event Action<Pickup> OnPickupMounted;

	public event Action<Pickup> OnPickupUnmounted;

	public event ActionApproveCallback OnApproveMount;

	public event ActionApproveCallback OnApproveUnmount;

	private void Start()
	{
		_mountBody = PhysicsUtils.FindRigidBodyUpwards(base.transform);
		base.collider.isTrigger = true;
	}

	public bool CanMount(Player player, Pickup pickup)
	{
		if ((bool)_mountedItem)
		{
			return false;
		}
		if (_mountType != MountType.Generic && pickup.MountType != _mountType)
		{
			return false;
		}
		if (this.OnApproveMount != null)
		{
			Approval approval = new Approval(player);
			this.OnApproveMount(ref approval);
			return approval.Approved;
		}
		return true;
	}

	public bool CanUnmount(Player player)
	{
		if (this.OnApproveUnmount != null)
		{
			Approval approval = new Approval(player);
			this.OnApproveUnmount(ref approval);
			return approval.Approved;
		}
		return true;
	}

	public void Mount(Pickup item)
	{
		_mountedItem = item;
		if (this.OnPickupMounted != null)
		{
			this.OnPickupMounted(item);
		}
	}

	public Pickup Unmount()
	{
		Pickup mountedItem = _mountedItem;
		_mountedItem = null;
		if (this.OnPickupUnmounted != null)
		{
			this.OnPickupUnmounted(mountedItem);
		}
		return mountedItem;
	}
}
