using UnityEngine;

[AddComponentMenu("Virtual Audio/Listener")]
public class VirtualAudioListener : MonoBehaviour
{
	private const string iconFileName = "ramjetaudio-microphone_virtual_icon.png";

	private bool _initialized;

	private void Start()
	{
		if (VirtualAudioManager.RegisterListener(this))
		{
			VirtualAudioManager.OnListenerEnable(this);
			_initialized = true;
		}
		else
		{
			_initialized = false;
		}
	}

	private void OnDestroy()
	{
		VirtualAudioManager.UnregisterListener(this);
		_initialized = false;
	}

	private void OnEnable()
	{
		if (_initialized)
		{
			VirtualAudioManager.OnListenerEnable(this);
		}
	}

	private void OnDisable()
	{
		if (_initialized)
		{
			VirtualAudioManager.OnListenerDisable(this);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "ramjetaudio-microphone_virtual_icon.png");
	}
}
