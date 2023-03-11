using UnityEngine;

public class TestCraft : MonoBehaviour
{
	[SerializeField]
	private float _maxSpeed = 100f;

	[SerializeField]
	private float _lookHorizontalSensitivity = 20f;

	[SerializeField]
	private float _lookVerticalSensitivity = 20f;

	private float _forwardInput;

	private void Update()
	{
		_forwardInput = Input.GetAxis("WalkForward");
		float num = Input.GetAxis("LookHorizontal") * _lookHorizontalSensitivity;
		float num2 = Input.GetAxis("LookVertical") * _lookVerticalSensitivity;
		Quaternion rot = Quaternion.Euler(0f, num * Time.deltaTime, 0f) * base.transform.rotation * Quaternion.Euler(num2 * Time.deltaTime, 0f, 0f);
		base.rigidbody.MoveRotation(rot);
	}

	private void FixedUpdate()
	{
		Vector3 vector = base.transform.forward * _forwardInput * _maxSpeed;
		Vector3 force = vector - base.rigidbody.velocity;
		base.rigidbody.AddForce(force, ForceMode.VelocityChange);
	}
}
