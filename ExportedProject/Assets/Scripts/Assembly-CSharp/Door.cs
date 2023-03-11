using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Door : MonoBehaviour
{
	[SerializeField]
	private Interactable _interactable;

	[SerializeField]
	private Vector3 _rotationAxis;

	[SerializeField]
	private float _maxAngle = 100f;

	[SerializeField]
	private float _openingTime = 0.3f;

	private Quaternion _startRotation;

	private bool _isOpen;

	public bool IsOpen
	{
		get
		{
			return _isOpen;
		}
	}

	private void Start()
	{
		_startRotation = base.transform.localRotation;
		_interactable.OnUse += OnUse;
	}

	private void OnUse(Player player)
	{
		_isOpen = !_isOpen;
		StopAllCoroutines();
		StartCoroutine(RotateDoorAsync(_isOpen));
	}

	private IEnumerator RotateDoorAsync(bool open)
	{
		Quaternion startRotation = base.transform.localRotation;
		Quaternion targetRotation = ((!open) ? _startRotation : (_startRotation * Quaternion.AngleAxis(_maxAngle, _rotationAxis)));
		float time = 0f;
		while (time < _openingTime)
		{
			base.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, time / _openingTime);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(base.transform.position, base.transform.TransformDirection(_rotationAxis));
	}
}
