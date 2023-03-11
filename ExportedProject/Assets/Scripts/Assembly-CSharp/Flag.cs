using UnityEngine;

public class Flag : MonoBehaviour
{
	[SerializeField]
	private float _rotationSpeed = 10f;

	private bool _isAffected;

	private Vector3 _lastPosition;

	private Vector3 _velocity;

	private void Start()
	{
		_lastPosition = base.transform.position;
		Pickup component = GetComponent<Pickup>();
		component.OnPickUp += OnPickup;
		component.OnDrop += OnDrop;
		component.OnMounted += OnMounted;
		component.OnUnmounted += OnUnmounted;
	}

	private void Update()
	{
		if (_isAffected)
		{
			Vector3 vector = SingletonComponent<AeroManager>.Instance.GetWindVelocity(base.transform.position, true) * 2f;
			Vector3 vector2 = _velocity - vector;
			if (vector2.sqrMagnitude > 0.1f)
			{
				Debug.DrawRay(base.transform.position, vector2, Color.magenta);
				vector2 -= Vector3.Project(vector2, base.transform.up);
				Quaternion to = Quaternion.LookRotation(-vector2, base.transform.up);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, _rotationSpeed * Time.deltaTime);
			}
		}
	}

	private void FixedUpdate()
	{
		_velocity = (base.transform.position - _lastPosition) / Time.deltaTime;
		_lastPosition = base.transform.position;
	}

	private void OnPickup(Player player)
	{
		_isAffected = true;
	}

	private void OnDrop(Player player)
	{
		_isAffected = false;
	}

	private void OnMounted(GearMount mount)
	{
		_isAffected = true;
	}

	private void OnUnmounted(GearMount mount)
	{
		_isAffected = false;
	}
}
