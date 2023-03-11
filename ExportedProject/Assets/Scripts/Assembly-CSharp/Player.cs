using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private PlayerInput _input;

	[SerializeField]
	private BodyController _bodyController;

	[SerializeField]
	private PlayerCamera _camera;

	[SerializeField]
	private PlayerCursor _cursor;

	[SerializeField]
	private VirtualAudioListener _listener;

	[SerializeField]
	private Transform _carryPoint;

	[SerializeField]
	private float _maxUseDistance = 5.5f;

	[SerializeField]
	private float _maxUseAngle = 110f;

	private int _playerId;

	private Pickup _carriedItem;

	private bool _isControllingItem;

	private Controllable _controlledItem;

	private Interactable _usedItem;

	public Pickup CarriedItem
	{
		get
		{
			return _carriedItem;
		}
	}

	public PlayerInput Input
	{
		get
		{
			return _input;
		}
		set
		{
			_input = value;
		}
	}

	public int PlayerId
	{
		get
		{
			return _playerId;
		}
	}

	public BodyController BodyController
	{
		get
		{
			return _bodyController;
		}
	}

	public PlayerCamera Camera
	{
		get
		{
			return _camera;
		}
	}

	public PlayerCursor Cursor
	{
		get
		{
			return _cursor;
		}
	}

	public VirtualAudioListener Listener
	{
		get
		{
			return _listener;
		}
	}

	public Transform CarryPoint
	{
		get
		{
			return _carryPoint;
		}
	}

	public event Action<Pickup, GearMount> OnItemMount;

	public event Action<Pickup> OnItemPickup;

	public event Action<Pickup> OnItemDrop;

	public void Initialize(int playerId)
	{
		_playerId = playerId;
		_input = base.gameObject.AddComponent<PlayerInput>();
		_input.Initialize(this);
		int layer = 30 + playerId;
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.gameObject.layer = layer;
		}
	}

	public void Use()
	{
		GameObject focusedObject = _cursor.FocusedObject;
		if ((bool)focusedObject)
		{
			Interactable component = focusedObject.GetComponent<Interactable>();
			if ((bool)component)
			{
				if (_isControllingItem)
				{
					_controlledItem.Cancel(this);
				}
				component.Use(this);
				_usedItem = component;
				return;
			}
			Controllable component2 = focusedObject.GetComponent<Controllable>();
			if ((bool)component2)
			{
				if (_isControllingItem)
				{
					_controlledItem.Cancel(this);
				}
				else
				{
					TryTakeControl(component2);
				}
				return;
			}
		}
		if (_isControllingItem)
		{
			_controlledItem.Use(this);
		}
	}

	public void StopUse()
	{
		if ((bool)_usedItem)
		{
			_usedItem.StopUse(this);
			_usedItem = null;
			return;
		}
		GameObject focusedObject = _cursor.FocusedObject;
		if ((bool)focusedObject)
		{
			Interactable component = focusedObject.GetComponent<Interactable>();
			if ((bool)component)
			{
				component.StopUse(this);
			}
		}
	}

	public void Cancel()
	{
		if (_isControllingItem)
		{
			_controlledItem.Cancel(this);
		}
	}

	public void Pickup()
	{
		GameObject focusedObject = _cursor.FocusedObject;
		if ((bool)_carriedItem)
		{
			if (_isControllingItem)
			{
				_controlledItem.Pickup(this);
				return;
			}
			if (!focusedObject)
			{
				TryDropCarriedObject();
				return;
			}
			GearMount component = focusedObject.GetComponent<GearMount>();
			if ((bool)component)
			{
				TryMountCarriedObject(component);
				return;
			}
		}
		else if ((bool)focusedObject)
		{
			Pickup component2 = focusedObject.GetComponent<Pickup>();
			if ((bool)component2)
			{
				if (_isControllingItem)
				{
					_controlledItem.Cancel(this);
				}
				TryPickupObject(component2);
				return;
			}
			GearMount component3 = focusedObject.GetComponent<GearMount>();
			if ((bool)component3 && (bool)component3.MountedItem)
			{
				TryPickupObject(component3.MountedItem);
				return;
			}
		}
		if (_isControllingItem)
		{
			_controlledItem.Pickup(this);
		}
	}

	private void TryTakeControl(Controllable controllable)
	{
		if (controllable.CanControl(this))
		{
			controllable.TakeControl(this);
			controllable.OnControlReleased += OnControlReleased;
			_controlledItem = controllable;
			_isControllingItem = true;
		}
	}

	private void OnControlReleased(Player player)
	{
		Debug.Log("OnControlReleased");
		_controlledItem.OnControlReleased -= OnControlReleased;
		_controlledItem = null;
		_isControllingItem = false;
	}

	private void TryPickupObject(Pickup pickup)
	{
		if (pickup.CanPickup(this))
		{
			pickup.PickUp(this);
			_carriedItem = pickup;
			_cursor.AddIgnoredObject(pickup.gameObject);
			if (this.OnItemPickup != null)
			{
				this.OnItemPickup(pickup);
			}
		}
	}

	private void TryDropCarriedObject()
	{
		if ((bool)_carriedItem && _carriedItem.CanDrop())
		{
			Pickup carriedItem = _carriedItem;
			_cursor.RemoveIgnoredObject(_carriedItem.gameObject);
			_carriedItem = null;
			carriedItem.Drop(base.rigidbody.velocity);
			if (this.OnItemDrop != null)
			{
				this.OnItemDrop(carriedItem);
			}
		}
	}

	private void TryMountCarriedObject(GearMount mountSlot)
	{
		if (_carriedItem.CanMount(this, mountSlot))
		{
			_carriedItem.MountTo(mountSlot);
			if (this.OnItemMount != null)
			{
				this.OnItemMount(_carriedItem, mountSlot);
			}
			_cursor.RemoveIgnoredObject(_carriedItem.gameObject);
			_carriedItem = null;
		}
	}

	private void Update()
	{
		if ((bool)_usedItem)
		{
			Vector3 to = _usedItem.transform.position - base.transform.position;
			float magnitude = to.magnitude;
			float num = Vector3.Angle(_camera.transform.forward, to);
			if (magnitude > _maxUseDistance || num > _maxUseAngle)
			{
				_usedItem.StopUse(this);
				_usedItem = null;
			}
		}
		if ((bool)_controlledItem)
		{
			Vector3 to2 = _controlledItem.transform.position - base.transform.position;
			float magnitude2 = to2.magnitude;
			float num2 = Vector3.Angle(_camera.transform.forward, to2);
			if (magnitude2 > _maxUseDistance || num2 > _maxUseAngle)
			{
				_controlledItem.ReleaseControl();
				_controlledItem = null;
			}
		}
	}
}
