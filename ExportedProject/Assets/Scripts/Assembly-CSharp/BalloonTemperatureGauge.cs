using UnityEngine;

public class BalloonTemperatureGauge : MonoBehaviour
{
	[SerializeField]
	private Balloon _balloon;

	[SerializeField]
	private Transform _needle;

	[SerializeField]
	private Vector3 _needleAxis;

	[SerializeField]
	private float _minAngle = -120f;

	[SerializeField]
	private float _maxAngle = 120f;

	private void Update()
	{
		float t = _balloon.Temperature / _balloon.MaxTemperature;
		_needle.rotation = Quaternion.AngleAxis(Mathf.Lerp(_minAngle, _maxAngle, t), base.transform.TransformDirection(_needleAxis));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 direction = base.transform.TransformDirection(_needleAxis);
		Gizmos.DrawRay(_needle.position, direction);
	}
}
