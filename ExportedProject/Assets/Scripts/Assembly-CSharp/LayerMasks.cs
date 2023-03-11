using UnityEngine;

public static class LayerMasks
{
	public const string IgnoreRaycastName = "Ignore Raycast";

	public const string ShipName = "Ship";

	public const string InteractiveName = "Interactive";

	public const string PlayerName = "Player";

	public const string LauncherName = "Player";

	private static int _ignoreRaycast = NameToLayerMask("Ignore Raycast");

	private static int _ship = NameToLayerMask("Player");

	private static int _interactive = NameToLayerMask("Interactive");

	private static int _player = NameToLayerMask("Player");

	private static int _launcher = NameToLayerMask("Player");

	public static int IgnoreRaycast
	{
		get
		{
			return _ignoreRaycast;
		}
	}

	public static int Ship
	{
		get
		{
			return _ship;
		}
	}

	public static int Interactive
	{
		get
		{
			return _interactive;
		}
	}

	public static int Player
	{
		get
		{
			return _player;
		}
	}

	public static int Launcher
	{
		get
		{
			return _launcher;
		}
	}

	public static int NameToLayerMask(string name)
	{
		return 1 << LayerMask.NameToLayer(name);
	}
}
