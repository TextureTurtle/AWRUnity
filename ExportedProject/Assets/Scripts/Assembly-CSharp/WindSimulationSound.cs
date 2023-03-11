using UnityEngine;

public class WindSimulationSound : MonoBehaviour
{
	[SerializeField]
	private Rigidbody _body;

	[SerializeField]
	private AudioSource _source;

	[SerializeField]
	private AudioClip _windClip;

	[SerializeField]
	private float _speedMultiplier = 0.1f;

	private void Start()
	{
		_source.loop = true;
		_source.clip = _windClip;
		_source.Play();
	}

	private void Update()
	{
		Vector3 windVelocity = SingletonComponent<AeroManager>.Instance.GetWindVelocity(base.transform.position, true);
		float magnitude = (_body.velocity - windVelocity).magnitude;
		_source.volume = magnitude * _speedMultiplier;
		_source.pitch = magnitude * _speedMultiplier;
	}
}
