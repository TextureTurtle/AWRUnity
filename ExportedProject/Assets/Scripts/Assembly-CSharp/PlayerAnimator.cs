using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField]
	private Animation _animation;

	[SerializeField]
	private BodyController _controller;

	[SerializeField]
	private string _walkName = "walking";

	private void Start()
	{
		_animation[_walkName].wrapMode = WrapMode.Loop;
		_animation.Play(_walkName);
	}

	private void Update()
	{
		float num = 0f;
		if (_controller.IsGrounded)
		{
			Vector3 relativeVelocity = _controller.RelativeVelocity;
			relativeVelocity.y = 0f;
			num = relativeVelocity.magnitude;
		}
		float weight = Mathf.Clamp01(num);
		_animation[_walkName].speed = num;
		_animation[_walkName].weight = weight;
	}
}
