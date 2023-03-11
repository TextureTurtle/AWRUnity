using UnityEngine;

[RequireComponent(typeof(VirtualRoundRobinSound))]
public class CollisionEffects : MonoBehaviour
{
	[SerializeField]
	private CollisionSource _collisionSource;

	[SerializeField]
	private VirtualRoundRobinSound _roundRobin;

	[SerializeField]
	private float _minImpactForce = 3f;

	[SerializeField]
	private float _impactForceScale = 1f;

	private void Start()
	{
		_collisionSource.OnCollisionEntered += OnCollisionEntered;
	}

	private void OnCollisionEntered(Collision collision)
	{
		float magnitude = collision.impactForceSum.magnitude;
		if (magnitude > _minImpactForce)
		{
			_roundRobin.Play(collision.impactForceSum.magnitude * _impactForceScale);
		}
	}
}
