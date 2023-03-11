using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[SerializeField]
	private float _verticalMin = -80f;

	[SerializeField]
	private float _verticalMax = 80f;

	private float _verticalAngle;

	public void Look(float vertical)
	{
		_verticalAngle = Mathf.Clamp(_verticalAngle - vertical * Time.deltaTime, _verticalMin, _verticalMax);
		base.transform.localRotation = Quaternion.Euler(_verticalAngle, 0f, 0f);
	}
}
