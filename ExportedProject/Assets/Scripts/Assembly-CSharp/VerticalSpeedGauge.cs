using UnityEngine;

public class VerticalSpeedGauge : MonoBehaviour
{
	[SerializeField]
	private Rigidbody _balloonBody;

	[SerializeField]
	private float _velocityScale = 1f;

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
		float value = _balloonBody.velocity.y * _velocityScale;
		_needle.rotation = Quaternion.AngleAxis(Mathf.Clamp(value, _minAngle, _maxAngle), base.transform.TransformDirection(_needleAxis));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 direction = base.transform.TransformDirection(_needleAxis);
		Gizmos.DrawRay(_needle.position, direction);
	}
}
