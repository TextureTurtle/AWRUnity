using UnityEngine;

[RequireComponent(typeof(ItemDescriptor))]
public class PressureReleaseValve : MonoBehaviour
{
	[SerializeField]
	private Interactable interactable;

	[SerializeField]
	private Balloon _balloon;

	public bool IsBeingUsed { get; private set; }

	private void Start()
	{
		interactable.OnUse += OnUse;
		interactable.OnStopUse += OnStopUse;
	}

	public void OnUse(Player player)
	{
		_balloon.OpenReleaseValve(true);
		IsBeingUsed = true;
	}

	public void OnStopUse(Player player)
	{
		_balloon.OpenReleaseValve(false);
		IsBeingUsed = false;
	}
}
