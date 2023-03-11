using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Aurora/Item/Pickup")]
[RequireComponent(typeof(WorldTransform), typeof(ItemDescriptor))]
public class Pickup : MonoBehaviour
{
	[SerializeField]
	private MountType _mountType;

	private bool _isHeld;

	private bool _isMounted;

	private Player _holder;

	private GearMount _mount;

	private Shader _diffuseShader;

	private Shader _transparentShader;

	private static float diffuseAlpha = 1f;

	private static float transparentAlpha = 0.75f;

	private Renderer[] _renderers;

	private List<Collider> _physicalColliders;

	private List<Collider> _collidersInTrigger;

	public MountType MountType
	{
		get
		{
			return _mountType;
		}
	}

	public bool IsHeld
	{
		get
		{
			return _isHeld;
		}
	}

	public Player Holder
	{
		get
		{
			return _holder;
		}
	}

	public bool IsMounted
	{
		get
		{
			return _isMounted;
		}
	}

	public GearMount Mount
	{
		get
		{
			return _mount;
		}
		set
		{
			_mount = value;
		}
	}

	public List<Collider> PhysicalColliders
	{
		get
		{
			return _physicalColliders;
		}
	}

	public event ActionApproveCallback OnApprovePickup;

	public event ActionApproveCallback OnApproveMount;

	public event Action<Player> OnPickUp;

	public event Action<Player> OnDrop;

	public event Action<GearMount> OnMounted;

	public event Action<GearMount> OnUnmounted;

	public bool CanPickup(Player player)
	{
		if (_isHeld)
		{
			return false;
		}
		if (IsMounted && !_mount.CanUnmount(player))
		{
			return false;
		}
		if (this.OnApprovePickup != null)
		{
			Approval approval = new Approval(player);
			this.OnApprovePickup(ref approval);
			return approval.Approved;
		}
		return true;
	}

	public void PickUp(Player player)
	{
		Debug.Log("Pickup");
		if (IsMounted)
		{
			Debug.Log("Should unmount");
			UnmountFrom(_mount);
		}
		_holder = player;
		base.transform.parent = player.CarryPoint;
		base.transform.position = player.CarryPoint.position;
		base.transform.rotation = player.CarryPoint.rotation;
		SetCollidersKinematic(true);
		base.rigidbody.isKinematic = true;
		_isHeld = true;
		SetShader(_transparentShader, transparentAlpha);
		if (this.OnPickUp != null)
		{
			this.OnPickUp(player);
		}
	}

	public bool CanDrop()
	{
		if (_collidersInTrigger.Count != 0)
		{
			return false;
		}
		return true;
	}

	public void Drop(Vector3 velocity)
	{
		_holder = null;
		base.transform.parent = null;
		base.rigidbody.isKinematic = false;
		SetCollidersKinematic(false);
		base.rigidbody.velocity = velocity;
		_isHeld = false;
		SetShader(_diffuseShader, diffuseAlpha);
		if (this.OnDrop != null)
		{
			this.OnDrop(_holder);
		}
	}

	public bool CanMount(Player player, GearMount mount)
	{
		if (_isHeld && _isMounted)
		{
			return false;
		}
		if (!mount.CanMount(player, this))
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

	public void MountTo(GearMount mount)
	{
		if (mount.IsKinematicMount)
		{
			base.rigidbody.isKinematic = true;
			SetCollidersKinematic(true);
			base.transform.parent = mount.MountPoint;
			base.transform.position = mount.MountPoint.position;
			base.transform.rotation = mount.MountPoint.rotation;
		}
		mount.Mount(this);
		_mount = mount;
		_isMounted = true;
		_isHeld = false;
		SetShader(_diffuseShader, diffuseAlpha);
		if (this.OnMounted != null)
		{
			this.OnMounted(mount);
		}
	}

	public void UnmountFrom(GearMount mount)
	{
		Debug.Log("UnmountFrom");
		if (mount.IsKinematicMount)
		{
			base.rigidbody.isKinematic = false;
			SetCollidersKinematic(false);
			base.transform.parent = null;
		}
		mount.Unmount();
		_mount = null;
		_isMounted = false;
		if (this.OnUnmounted != null)
		{
			this.OnUnmounted(mount);
		}
	}

	private void Awake()
	{
		_diffuseShader = Shader.Find("Diffuse");
		_transparentShader = Shader.Find("Transparent/Diffuse");
		_renderers = GetComponentsInChildren<MeshRenderer>(true);
		_collidersInTrigger = new List<Collider>();
		_physicalColliders = new List<Collider>(GetComponentsInChildren<Collider>(true));
		_physicalColliders.Remove(base.collider);
	}

	private void OnTriggerEnter(Collider otherCollider)
	{
		if (!otherCollider.isTrigger && otherCollider != base.collider && otherCollider.tag != "Player" && otherCollider.tag != "Mount" && !_physicalColliders.Contains(otherCollider))
		{
			_collidersInTrigger.Add(otherCollider);
		}
	}

	private void OnTriggerExit(Collider otherCollider)
	{
		if (_collidersInTrigger.Contains(otherCollider))
		{
			_collidersInTrigger.Remove(otherCollider);
		}
	}

	private void SetCollidersKinematic(bool kinematic)
	{
		foreach (Collider physicalCollider in _physicalColliders)
		{
			physicalCollider.isTrigger = kinematic;
		}
	}

	private void SetShader(Shader shader, float alpha)
	{
		Renderer[] renderers = _renderers;
		foreach (Renderer renderer in renderers)
		{
			renderer.material.shader = shader;
			Color color = renderer.material.color;
			color.a = alpha;
			renderer.material.color = color;
		}
	}
}
