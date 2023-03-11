using UnityEngine;

public class InteractableEffects : MonoBehaviour
{
	[SerializeField]
	private Interactable _interactable;

	[SerializeField]
	private VirtualAudioSource _source;

	[SerializeField]
	private AudioClip _useClip;

	[SerializeField]
	private AudioClip _stopUseClip;

	private void Start()
	{
		_interactable.OnUse += OnUse;
		_interactable.OnStopUse += OnStopUse;
	}

	private void OnUse(Player player)
	{
		_source.PlayOneShot(player.Listener, _useClip);
	}

	private void OnStopUse(Player player)
	{
		_source.PlayOneShot(player.Listener, _stopUseClip);
	}
}
