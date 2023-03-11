using UnityEngine;

public class FuelCanister : MonoBehaviour
{
	[SerializeField]
	private float _maxFuel = 100f;

	private float _fuel;

	public float MaxFuel
	{
		get
		{
			return _maxFuel;
		}
	}

	public float Fuel
	{
		get
		{
			return _fuel;
		}
	}

	private void Awake()
	{
		_fuel = _maxFuel;
	}

	public float Take(float amount)
	{
		float num = Mathf.Min(amount, _fuel);
		_fuel -= num;
		return num;
	}
}
