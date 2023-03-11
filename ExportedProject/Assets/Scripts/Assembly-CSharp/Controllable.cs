using System;
using UnityEngine;

public class Controllable : MonoBehaviour
{
	private Player _controller;

	private bool _isControlled;

	public bool IsControlled
	{
		get
		{
			return _isControlled;
		}
	}

	public Player Controller
	{
		get
		{
			return _controller;
		}
	}

	public event ActionApproveCallback OnApproveControl;

	public event Action<Player> OnControlTaken;

	public event Action<Player> OnControlReleased;

	public event Action<Player> OnUse;

	public event Action<Player> OnStopUse;

	public event Action<Player> OnCancel;

	public event Action<Player> OnPickup;

	public bool CanControl(Player player)
	{
		if (_isControlled)
		{
			return false;
		}
		if (this.OnApproveControl != null)
		{
			Approval approval = new Approval(player);
			this.OnApproveControl(ref approval);
			return approval.Approved;
		}
		return true;
	}

	public void TakeControl(Player player)
	{
		_isControlled = true;
		_controller = player;
		if (this.OnControlTaken != null)
		{
			this.OnControlTaken(player);
		}
	}

	public void ReleaseControl()
	{
		if (this.OnControlReleased != null)
		{
			this.OnControlReleased(_controller);
		}
		_isControlled = false;
		_controller = null;
	}

	public void Use(Player player)
	{
		if (this.OnUse != null)
		{
			this.OnUse(player);
		}
	}

	public void StopUse(Player player)
	{
		if (this.OnStopUse != null)
		{
			this.OnStopUse(player);
		}
	}

	public void Pickup(Player player)
	{
		if (this.OnPickup != null)
		{
			this.OnPickup(player);
		}
	}

	public void Cancel(Player player)
	{
		if (this.OnCancel != null)
		{
			this.OnCancel(player);
		}
	}
}
