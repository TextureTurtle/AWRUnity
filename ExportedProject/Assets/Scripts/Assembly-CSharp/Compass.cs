using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class Compass : MonoBehaviour
{
	[SerializeField]
	private float _rotationSpeed = 1f;

	[SerializeField]
	private Transform _needle;

	[SerializeField]
	private float _jitterStartDistance = 1000f;

	private WorldTransform _worldTransform;

	private DVector3 _northPolePosition = DVector3.Zero;

	private Vector3 _targetDirection;

	private void Start()
	{
		_worldTransform = GetComponent<WorldTransform>();
		double num = SingletonComponent<WorldStreamer>.Instance.ZoneSize * 0.5;
		_northPolePosition = new DVector3(num, 0.0, num);
		StartCoroutine(UpdateHeading());
	}

	private void Update()
	{
		Vector3 forward = base.transform.InverseTransformDirection(_targetDirection - Vector3.Project(_targetDirection, base.transform.up));
		Quaternion to = Quaternion.LookRotation(forward, base.transform.up);
		_needle.localRotation = Quaternion.Slerp(_needle.localRotation, to, Time.deltaTime * _rotationSpeed);
	}

	private IEnumerator UpdateHeading()
	{
		while (true)
		{
			DVector3 position = _worldTransform.Position;
			DVector3 northPoleDelta = _northPolePosition - position;
			northPoleDelta.Y = 0.0;
			_targetDirection = DVector3.ToVector3(northPoleDelta.GetNormalized);
			double poleDistance = northPoleDelta.GetMagnitude;
			if (poleDistance < (double)_jitterStartDistance)
			{
				_targetDirection = Vector3.Lerp(to: Random.insideUnitSphere.normalized, t: 1f - (float)poleDistance / _jitterStartDistance, from: _targetDirection);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
