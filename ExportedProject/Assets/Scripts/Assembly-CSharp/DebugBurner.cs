using UnityEngine;

public class DebugBurner : MonoBehaviour
{
	[SerializeField]
	private float _maxBurnRate = 0.01f;

	[SerializeField]
	private float _fuelToEnergy = 100f;

	[SerializeField]
	private Balloon _balloon;

	private void Update()
	{
		float num = _maxBurnRate * Time.deltaTime;
		_balloon.AddEnergy(num * _fuelToEnergy);
	}
}
