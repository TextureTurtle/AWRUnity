using UnityEngine;

public class PlayerInteractionEffects : MonoBehaviour
{
	[SerializeField]
	private Player _player;

	[SerializeField]
	private VirtualAudioSource _source;

	[SerializeField]
	private AudioClip _pickupClip;

	[SerializeField]
	private AudioClip _dropClip;

	[SerializeField]
	private AudioClip _mountClip;

	private void Start()
	{
		_player.OnItemPickup += OnPickup;
		_player.OnItemDrop += OnDrop;
		_player.OnItemMount += OnMount;
	}

	private void OnPickup(Pickup pickup)
	{
		_source.PlayOneShot(_player.Listener, _pickupClip);
	}

	private void OnDrop(Pickup pickup)
	{
		_source.PlayOneShot(_player.Listener, _dropClip);
	}

	private void OnMount(Pickup pickup, GearMount mount)
	{
		_source.PlayOneShot(_player.Listener, _mountClip);
	}
}
