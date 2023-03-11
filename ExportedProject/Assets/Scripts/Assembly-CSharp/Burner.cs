using UnityEngine;

[RequireComponent(typeof(ItemDescriptor))]
public class Burner : MonoBehaviour
{
	[SerializeField]
	private Interactable _interactable;

	[SerializeField]
	private Pickup _pickup;

	[SerializeField]
	private float _maxBurnRate = 0.01f;

	[SerializeField]
	private float _burnRateChangeSpeed = 0.5f;

	[SerializeField]
	private float _fuelToEnergy = 100f;

	private Balloon _balloon;

	private BurnerMount _burnerMount;

	private float _burnRate;

	private bool _isOn;

	public bool IsOn
	{
		get
		{
			return _isOn;
		}
	}

	public float BurnRate
	{
		get
		{
			return _burnRate;
		}
	}

	public float MaxBurnRate
	{
		get
		{
			return _maxBurnRate;
		}
	}

	private void Start()
	{
		_pickup.OnMounted += OnMounted;
		_pickup.OnUnmounted += OnUnmounted;
	}

	private void OnMounted(GearMount mount)
	{
		_burnerMount = mount.GetComponent<BurnerMount>();
		if ((bool)_burnerMount)
		{
			_balloon = _burnerMount.Balloon;
		}
	}

	private void OnUnmounted(GearMount mount)
	{
		_burnerMount = null;
		_burnRate = 0f;
	}

	private void Update()
	{
		_isOn = (bool)_burnerMount && _burnerMount.MountedCanister != null && _burnerMount.MountedCanister.Fuel > 0f;
		float to = ((!_isOn || !_interactable.IsBeingUsed) ? 0f : _maxBurnRate);
		_burnRate = Mathf.Lerp(_burnRate, to, _burnRateChangeSpeed * Time.deltaTime);
		if (_isOn)
		{
			float num = _burnerMount.Take(_burnRate * Time.deltaTime);
			if ((bool)_balloon)
			{
				_balloon.AddEnergy(num * _fuelToEnergy);
			}
		}
	}
}
