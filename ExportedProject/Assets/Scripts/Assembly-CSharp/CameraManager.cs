using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonComponent<CameraManager>
{
	[SerializeField]
	private Game _game;

	[SerializeField]
	private bool _recalculateCameraRects = true;

	private List<PlayerCamera> _cameras;

	public List<PlayerCamera> Cameras
	{
		get
		{
			return _cameras;
		}
	}

	private void Awake()
	{
		_cameras = new List<PlayerCamera>();
	}

	private void Start()
	{
		_game.OnPlayerSpawned += OnPlayerSpawned;
		_game.OnPlayerDespawned += OnPlayerDespawned;
	}

	private void OnPlayerSpawned(Player player)
	{
		_cameras.Add(player.GetComponentInChildren<PlayerCamera>());
		RecalculateCameraRects();
	}

	private void OnPlayerDespawned(Player player)
	{
		_cameras.Remove(player.GetComponentInChildren<PlayerCamera>());
		RecalculateCameraRects();
	}

	private void RecalculateCameraRects()
	{
		if (_recalculateCameraRects)
		{
			int num = _game.Players.Length;
			Player[] players = _game.Players;
			foreach (Player player in players)
			{
				Camera componentInChildren = player.Camera.GetComponentInChildren<Camera>();
				float num2 = 1f / (float)num;
				componentInChildren.rect = new Rect(0f, 1f - num2 * (float)(player.PlayerId + 1), 1f, num2);
				componentInChildren.cullingMask &= ~(1 << 30 + player.PlayerId);
			}
		}
	}
}
