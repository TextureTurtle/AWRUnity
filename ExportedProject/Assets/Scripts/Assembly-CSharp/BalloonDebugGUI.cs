using UnityEngine;

public class BalloonDebugGUI : MonoBehaviour
{
	[SerializeField]
	private Player _player;

	[SerializeField]
	private PlayerCursor _playerCursor;

	[SerializeField]
	private BurnerMount _burnerMount;

	[SerializeField]
	private Burner _burner;

	[SerializeField]
	private Balloon _balloon;

	private ItemDescriptor _focusedObjectDescriptor;

	private ItemDescriptor _carriedPickupDescriptor;

	private void Start()
	{
		_playerCursor.OnFocusChanged += OnFocusChange;
		_player.OnItemPickup += OnItemPickup;
		_player.OnItemDrop += OnItemDrop;
		_player.OnItemMount += OnItemMount;
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect((float)Screen.width - 200f, 0f, 200f, 300f), GUI.skin.box);
		GUILayout.BeginVertical();
		GUILayout.Label(string.Format("Looking at: {0}", (!(_focusedObjectDescriptor != null)) ? "None" : _focusedObjectDescriptor.ItemName));
		GUILayout.Label(string.Format("Carrying: {0}", (!(_carriedPickupDescriptor != null)) ? "None" : _carriedPickupDescriptor.ItemName));
		GUILayout.Space(16f);
		GUILayout.Label(string.Format("Canister Fuel: {0:0.0}", (!_burnerMount.MountedCanister) ? (-1f) : _burnerMount.MountedCanister.Fuel));
		GUILayout.Label(string.Format("Burn Rate: {0:0.00}", _burner.BurnRate));
		GUILayout.Space(16f);
		GUILayout.Label(string.Format("Balloon Altitude: {0:0.0}", _balloon.transform.position.y));
		GUILayout.Label(string.Format("Ambient Temperature: {0:0.0}", SingletonComponent<AeroManager>.Instance.GetTemperatureAtPoint(_balloon.transform.position)));
		GUILayout.Label(string.Format("Balloon Temperature: {0:0.0}", _balloon.Temperature));
		GUILayout.Label(string.Format("Balloon Lift: {0:0.0}", _balloon.LiftForce));
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void OnFocusChange(GameObject go)
	{
		if (go != null)
		{
			_focusedObjectDescriptor = go.GetComponent<ItemDescriptor>();
		}
		else
		{
			_focusedObjectDescriptor = null;
		}
	}

	private void OnItemPickup(Pickup pickup)
	{
		if (pickup != null)
		{
			_carriedPickupDescriptor = pickup.GetComponent<ItemDescriptor>();
		}
		else
		{
			_carriedPickupDescriptor = null;
		}
	}

	private void OnItemDrop(Pickup pickup)
	{
		_carriedPickupDescriptor = null;
	}

	private void OnItemMount(Pickup pickup, GearMount mount)
	{
		_carriedPickupDescriptor = null;
	}
}
