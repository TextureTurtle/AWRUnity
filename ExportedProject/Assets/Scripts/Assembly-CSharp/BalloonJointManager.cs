using UnityEngine;

public class BalloonJointManager : MonoBehaviour
{
	[SerializeField]
	private BalloonJoint[] _joints;

	[SerializeField]
	private float _springStrength = 1f;

	[SerializeField]
	private float _springExponent = 1f;

	[SerializeField]
	private float _dampeningStrength;

	[SerializeField]
	private float _minDistance = 5f;

	[SerializeField]
	private float _maxDistance = 6f;

	private void Awake()
	{
		Apply();
	}

	public void Apply()
	{
		BalloonJoint[] joints = _joints;
		foreach (BalloonJoint balloonJoint in joints)
		{
			balloonJoint.SpringStrength = _springStrength;
			balloonJoint.DampeningStrength = _dampeningStrength;
			balloonJoint.MinDistance = _minDistance;
			balloonJoint.MaxDistance = _maxDistance;
			balloonJoint.SpringExponent = _springExponent;
		}
	}
}
