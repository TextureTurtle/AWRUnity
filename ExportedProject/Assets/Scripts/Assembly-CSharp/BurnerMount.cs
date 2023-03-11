using UnityEngine;

public class BurnerMount : MonoBehaviour
{
	[SerializeField]
	private GearMount _fuelMount;

	[SerializeField]
	private Balloon _balloon;

	private FuelCanister _mountedCanister;

	public Balloon Balloon
	{
		get
		{
			return _balloon;
		}
	}

	public FuelCanister MountedCanister
	{
		get
		{
			return _mountedCanister;
		}
	}

	public float Take(float amount)
	{
		return (!_mountedCanister) ? 0f : _mountedCanister.Take(amount);
	}

	private void Start()
	{
		_fuelMount.OnPickupMounted += OnFuelPickupMounted;
		_fuelMount.OnPickupUnmounted += OnFuelPickupUnmounted;
	}

	private void OnFuelPickupMounted(Pickup obj)
	{
		_mountedCanister = obj.GetComponent<FuelCanister>();
	}

	private void OnFuelPickupUnmounted(Pickup obj)
	{
		_mountedCanister = null;
	}
}
