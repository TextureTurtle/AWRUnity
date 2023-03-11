using UnityEngine;

public class DoorEffects : MonoBehaviour
{
	[SerializeField]
	private Door _door;

	[SerializeField]
	private VirtualAudioSource _source;

	[SerializeField]
	private AudioClip _openClip;

	[SerializeField]
	private AudioClip _closeClip;

	private void Start()
	{
		Interactable component = _door.GetComponent<Interactable>();
		component.OnUse += OnUse;
	}

	private void OnUse(Player player)
	{
		_source.PlayOneShot(player.Listener, (!_door.IsOpen) ? _closeClip : _openClip);
	}
}
