using System;
using UnityEngine;

[RequireComponent(typeof(ItemDescriptor), typeof(Focusable))]
public class Interactable : MonoBehaviour
{
	private Player _user;

	private bool _isBeingUsed;

	public bool IsBeingUsed
	{
		get
		{
			return _isBeingUsed;
		}
	}

	public Player User
	{
		get
		{
			return _user;
		}
	}

	public event Action<Player> OnUse;

	public event Action<Player> OnStopUse;

	public void Use(Player player)
	{
		_user = player;
		_isBeingUsed = true;
		if (this.OnUse != null)
		{
			this.OnUse(player);
		}
	}

	public void StopUse(Player player)
	{
		_user = null;
		_isBeingUsed = false;
		if (this.OnStopUse != null)
		{
			this.OnStopUse(player);
		}
	}
}
