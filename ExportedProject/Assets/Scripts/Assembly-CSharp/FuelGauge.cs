using UnityEngine;

public class FuelGauge : MonoBehaviour
{
	[SerializeField]
	private BurnerMount _burnerMount;

	[SerializeField]
	private Transform _needle;

	[SerializeField]
	private Vector3 _needleAxis;

	[SerializeField]
	private float _minAngle = -70f;

	[SerializeField]
	private float _maxAngle = 70f;

	private void Update()
	{
		float t = ((!_burnerMount.MountedCanister) ? 0f : (_burnerMount.MountedCanister.Fuel / _burnerMount.MountedCanister.MaxFuel));
		_needle.rotation = Quaternion.AngleAxis(Mathf.Lerp(_minAngle, _maxAngle, t), base.transform.TransformDirection(_needleAxis));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 direction = base.transform.TransformDirection(_needleAxis);
		Gizmos.DrawRay(_needle.position, direction);
	}
}
