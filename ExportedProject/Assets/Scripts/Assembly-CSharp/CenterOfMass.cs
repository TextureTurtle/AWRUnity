using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
	[SerializeField]
	private Vector3 _centerOfMass;

	private void Awake()
	{
		base.rigidbody.centerOfMass = _centerOfMass;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(base.transform.TransformPoint(_centerOfMass), 0.1f);
	}
}
