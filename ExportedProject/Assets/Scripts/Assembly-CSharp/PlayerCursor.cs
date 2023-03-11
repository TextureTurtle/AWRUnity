using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
	[SerializeField]
	private Player _player;

	[SerializeField]
	private Transform _cursorRoot;

	[SerializeField]
	private float _interactionRange = 5.5f;

	private List<GameObject> _ignoredObjects;

	private List<RaycastHit> _hitInfos;

	private GameObject _previousFocusedObject;

	private GameObject _focusedObject;

	public GameObject FocusedObject
	{
		get
		{
			return _focusedObject;
		}
	}

	public event Action<GameObject> OnFocusChanged;

	private void Awake()
	{
		_ignoredObjects = new List<GameObject>();
		_hitInfos = new List<RaycastHit>();
	}

	public void Update()
	{
		Debug.DrawRay(_cursorRoot.position, _cursorRoot.forward * _interactionRange);
		_focusedObject = QueryCursor(_ignoredObjects);
		if (_focusedObject != _previousFocusedObject)
		{
			if ((bool)_previousFocusedObject)
			{
				Focusable component = _previousFocusedObject.GetComponent<Focusable>();
				if ((bool)component)
				{
					component.Unfocus(_player);
				}
			}
			if ((bool)_focusedObject)
			{
				Focusable component2 = _focusedObject.GetComponent<Focusable>();
				if ((bool)component2)
				{
					component2.Focus(_player);
				}
			}
			if (this.OnFocusChanged != null)
			{
				this.OnFocusChanged(_focusedObject);
			}
		}
		_previousFocusedObject = _focusedObject;
	}

	private GameObject QueryCursor(List<GameObject> ignoredObjects)
	{
		RaycastHit[] collection = Physics.RaycastAll(_cursorRoot.position, _cursorRoot.forward, _interactionRange, LayerMasks.Interactive);
		_hitInfos.Clear();
		_hitInfos.AddRange(collection);
		_hitInfos.Sort((RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
		foreach (RaycastHit hitInfo in _hitInfos)
		{
			GameObject gameObject = hitInfo.collider.gameObject;
			GearMount component = gameObject.GetComponent<GearMount>();
			if (!ignoredObjects.Contains(gameObject) && (!component || !component.MountedItem))
			{
				return gameObject;
			}
		}
		return null;
	}

	public void AddIgnoredObject(GameObject go)
	{
		AddIgnoredObjectRecursively(go);
	}

	private void AddIgnoredObjectRecursively(GameObject go)
	{
		_ignoredObjects.Add(go);
		for (int i = 0; i < go.transform.childCount; i++)
		{
			AddIgnoredObjectRecursively(go.transform.GetChild(i).gameObject);
		}
	}

	public void RemoveIgnoredObject(GameObject go)
	{
		RemoveIgnoredObjectRecursively(go);
	}

	private void RemoveIgnoredObjectRecursively(GameObject go)
	{
		_ignoredObjects.Remove(go);
		for (int i = 0; i < go.transform.childCount; i++)
		{
			RemoveIgnoredObjectRecursively(go.transform.GetChild(i).gameObject);
		}
	}
}
