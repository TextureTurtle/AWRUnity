using System.Collections.Generic;
using RamjetAnvil.Unity.Utility;
using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

public class VirtualAudioObject : MonoBehaviour, IListenable
{
	public enum VirtualAudioMode
	{
		Normal = 0,
		SingleListener = 1,
		Shared2D = 2
	}

	[SerializeField]
	private VirtualAudioListener _exclusiveListener;

	private bool _initializedInPlayMode;

	private List<VirtualAudioComponentBase> _virtualComponents;

	private List<GameObject> _realObjects;

	private Dictionary<VirtualAudioListener, GameObject> _listenedObjects;

	public VirtualAudioListener ExclusiveListener
	{
		get
		{
			return _exclusiveListener;
		}
		set
		{
			if (value != _exclusiveListener)
			{
				RemoveAllListeners();
				AddListener(value);
				_exclusiveListener = value;
			}
		}
	}

	public bool InitializedInPlayMode
	{
		get
		{
			return _initializedInPlayMode;
		}
	}

	public int NumVirtualComponents
	{
		get
		{
			return (_virtualComponents == null) ? (-1) : _virtualComponents.Count;
		}
	}

	public int NumRealObjects
	{
		get
		{
			return (_realObjects == null) ? (-1) : _realObjects.Count;
		}
	}

	public int NumListeners
	{
		get
		{
			return (_listenedObjects == null) ? (-1) : _listenedObjects.Count;
		}
	}

	public ICollection<GameObject> RealObjects
	{
		get
		{
			return _realObjects;
		}
	}

	private void Reset()
	{
		if (!Application.isPlaying)
		{
		}
	}

	private void Awake()
	{
		if (!_initializedInPlayMode)
		{
			InitializeInPlayMode();
		}
	}

	private void OnDestroy()
	{
		VirtualAudioManager.UnregisterListenable(this);
		DestroyVirtualComponents();
		DestroyRealObjects();
		_initializedInPlayMode = false;
	}

	public void InitializeInPlayMode()
	{
		if (!_initializedInPlayMode)
		{
			_virtualComponents = new List<VirtualAudioComponentBase>();
			_realObjects = new List<GameObject>();
			_listenedObjects = new Dictionary<VirtualAudioListener, GameObject>();
			VirtualAudioManager.RegisterListenable(this);
			EnsureValidHideFlags();
			_initializedInPlayMode = true;
		}
	}

	public void RegisterVirtualComponent(VirtualAudioComponentBase virtualAudioComponent)
	{
		_virtualComponents.Add(virtualAudioComponent);
		foreach (KeyValuePair<VirtualAudioListener, GameObject> listenedObject in _listenedObjects)
		{
			virtualAudioComponent.OnListenerAdded(listenedObject.Key, listenedObject.Value);
		}
	}

	public void UnregisterVirtualComponent(VirtualAudioComponentBase virtualAudioComponent)
	{
		_virtualComponents.Remove(virtualAudioComponent);
		if (_virtualComponents.Count == 0)
		{
			MonoBehaviourExtensions.DestroyAgnostic(this);
		}
	}

	public void AddListener(VirtualAudioListener virtualListener)
	{
		if (_listenedObjects.Count < VirtualAudioManager.MaxListeners && !_listenedObjects.ContainsKey(virtualListener) && (!(_exclusiveListener != null) || !(virtualListener != _exclusiveListener)))
		{
			GameObject value = CreateObjectForListener(virtualListener);
			_listenedObjects.Add(virtualListener, value);
		}
	}

	public void RemoveListener(VirtualAudioListener virtualListener)
	{
		if (!_listenedObjects.ContainsKey(virtualListener))
		{
			return;
		}
		GameObject gameObject = _listenedObjects[virtualListener];
		_listenedObjects.Remove(virtualListener);
		foreach (VirtualAudioComponentBase virtualComponent in _virtualComponents)
		{
			virtualComponent.OnListenerRemoved(virtualListener);
		}
		if ((bool)gameObject)
		{
			Object.Destroy(gameObject);
		}
	}

	private void RemoveAllListeners()
	{
		foreach (KeyValuePair<VirtualAudioListener, GameObject> listenedObject in _listenedObjects)
		{
			foreach (VirtualAudioComponentBase virtualComponent in _virtualComponents)
			{
				virtualComponent.OnListenerRemoved(listenedObject.Key);
			}
			Object.Destroy(listenedObject.Value);
		}
		_listenedObjects.Clear();
	}

	public void OnListenerEnabled(VirtualAudioListener listener)
	{
		if (_listenedObjects.ContainsKey(listener))
		{
			_listenedObjects[listener].SetActive(true);
		}
	}

	public void OnListenerDisabled(VirtualAudioListener listener)
	{
		if (_listenedObjects.ContainsKey(listener))
		{
			GameObject gameObject = _listenedObjects[listener];
			if ((bool)gameObject)
			{
				gameObject.SetActive(false);
			}
		}
	}

	private GameObject CreateObjectForListener(VirtualAudioListener listener)
	{
		GameObject gameObject = new GameObject(base.name + "_audio_" + listener.name);
		gameObject.transform.parent = SingletonComponent<VirtualAudioManager>.Instance.transform;
		_realObjects.Add(gameObject);
		foreach (VirtualAudioComponentBase virtualComponent in _virtualComponents)
		{
			virtualComponent.OnListenerAdded(listener, gameObject);
		}
		SynchronizeObjectProperties(listener, gameObject);
		gameObject.SetActive(true);
		return gameObject;
	}

	private void DestroyRealObjects()
	{
		foreach (GameObject realObject in _realObjects)
		{
			if (realObject != null)
			{
				MonoBehaviourExtensions.DestroyAgnostic(realObject, true);
			}
		}
		_realObjects.Clear();
	}

	private void DestroyVirtualComponents()
	{
		foreach (VirtualAudioComponentBase virtualComponent in _virtualComponents)
		{
			if (virtualComponent != null)
			{
				virtualComponent.DestroyRealComponents();
				MonoBehaviourExtensions.DestroyAgnostic(virtualComponent, true);
			}
		}
	}

	public void EnsureValidHideFlags()
	{
		HideFlags virtualHideFlags = VirtualAudioManager.VirtualHideFlags;
		if (Application.isPlaying)
		{
			SetHideflags(virtualHideFlags);
		}
	}

	public void SetHideflags(HideFlags hideFlags)
	{
		if (_realObjects == null)
		{
			return;
		}
		foreach (GameObject realObject in _realObjects)
		{
			if ((bool)realObject)
			{
				realObject.hideFlags = hideFlags;
			}
		}
	}

	public void Update()
	{
		if (Application.isPlaying)
		{
			UpdateRealSources();
		}
	}

	private void UpdateRealSources()
	{
		foreach (KeyValuePair<VirtualAudioListener, GameObject> listenedObject in _listenedObjects)
		{
			SynchronizeObjectProperties(listenedObject.Key, listenedObject.Value);
		}
	}

	private void SynchronizeObjectProperties(VirtualAudioListener listener, GameObject realObject)
	{
		Vector3 position = listener.transform.InverseTransformPoint(base.transform.position);
		Vector3 position2 = SingletonComponent<VirtualAudioManager>.Instance.transform.TransformPoint(position);
		realObject.transform.position = position2;
	}
}
