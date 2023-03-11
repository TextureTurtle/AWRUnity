using UnityEngine;

public class SkySphere : MonoBehaviour
{
	private Transform _cameraTransform;

	private void Start()
	{
		SingletonComponent<Game>.Instance.OnPlayerSpawned += delegate(Player player)
		{
			_cameraTransform = player.Camera.transform;
		};
		SingletonComponent<Game>.Instance.OnPlayerDespawned += delegate
		{
			_cameraTransform = null;
		};
	}

	private void Update()
	{
		if ((bool)_cameraTransform)
		{
			base.transform.position = _cameraTransform.position;
		}
	}
}
