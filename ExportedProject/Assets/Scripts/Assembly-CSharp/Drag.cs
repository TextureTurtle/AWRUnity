using UnityEngine;

[AddComponentMenu("Aerodynamics/Drag")]
public class Drag : MonoBehaviour
{
	[SerializeField]
	private float _cDrag = 0.2f;

	[SerializeField]
	private float _area = 1f;

	private Vector3 _dragForce;

	private float _sleepRelativeSpeedSqrThreshold;

	public float Area
	{
		get
		{
			return _area;
		}
		set
		{
			_area = value;
		}
	}

	public float CDrag
	{
		get
		{
			return _cDrag;
		}
		set
		{
			_cDrag = value;
		}
	}

	private void Start()
	{
		_sleepRelativeSpeedSqrThreshold = 1f * base.rigidbody.mass / (0.6f * _cDrag * _area);
	}

	private void FixedUpdate()
	{
		if (!base.rigidbody.isKinematic)
		{
			AeroManager instance = SingletonComponent<AeroManager>.Instance;
			Vector3 position = base.transform.position;
			Vector3 windVelocity = instance.GetWindVelocity(position, false);
			Vector3 vector = base.rigidbody.velocity - windVelocity;
			float sqrMagnitude = vector.sqrMagnitude;
			if (!(sqrMagnitude < _sleepRelativeSpeedSqrThreshold))
			{
				float airDensity = instance.GetAirDensity(position);
				float num = 0.5f * airDensity * sqrMagnitude * _area * _cDrag;
				_dragForce = vector * ((0f - num) / Mathf.Sqrt(sqrMagnitude));
				base.rigidbody.AddForce(_dragForce);
			}
		}
	}
}
