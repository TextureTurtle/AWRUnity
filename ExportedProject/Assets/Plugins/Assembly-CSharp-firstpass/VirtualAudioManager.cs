using System.Collections.Generic;
using RamjetAnvil.Unity.VirtualAudio;
using UnityEngine;

[AddComponentMenu("Virtual Audio/Manager")]
public class VirtualAudioManager : SingletonComponent<VirtualAudioManager>
{
	private const HideFlags HiddenHideflags = HideFlags.HideInHierarchy;

	private const HideFlags ShownHideflags = HideFlags.NotEditable;

	private const int DefaultMaxListeners = 2;

	private const int MaxListenersLimit = 16;

	private const string iconFileName = "ramjetaudio-microphone_real_icon.png";

	[SerializeField]
	private bool _showRealObjects;

	[SerializeField]
	private HideFlags _virtualHideFlags = HideFlags.HideInHierarchy;

	[SerializeField]
	private int _maxListeners = 2;

	private ICollection<IListenable> _listenables;

	private ICollection<VirtualAudioListener> _virtualListeners;

	public static bool ShowRealObjects
	{
		get
		{
			return (bool)SingletonComponent<VirtualAudioManager>.Instance && SingletonComponent<VirtualAudioManager>.Instance._showRealObjects;
		}
		set
		{
			if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
			{
				SingletonComponent<VirtualAudioManager>.Instance._showRealObjects = value;
				SingletonComponent<VirtualAudioManager>.Instance._virtualHideFlags = ((!value) ? HideFlags.HideInHierarchy : HideFlags.NotEditable);
			}
		}
	}

	public static HideFlags VirtualHideFlags
	{
		get
		{
			return (!SingletonComponent<VirtualAudioManager>.Instance) ? HideFlags.HideInHierarchy : SingletonComponent<VirtualAudioManager>.Instance._virtualHideFlags;
		}
	}

	public static int MaxListeners
	{
		get
		{
			return (!SingletonComponent<VirtualAudioManager>.Instance) ? (-1) : SingletonComponent<VirtualAudioManager>.Instance._maxListeners;
		}
		set
		{
			if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
			{
				value = Mathf.Clamp(value, 0, 16);
				SingletonComponent<VirtualAudioManager>.Instance._maxListeners = value;
				if (Application.isPlaying)
				{
					SetGlobalMaxListeners(value);
				}
			}
		}
	}

	public static int NumListeners
	{
		get
		{
			return (!SingletonComponent<VirtualAudioManager>.Instance) ? (-1) : SingletonComponent<VirtualAudioManager>.Instance._virtualListeners.Count;
		}
	}

	public static void RegisterListenable(IListenable listenable)
	{
		if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
		{
			SingletonComponent<VirtualAudioManager>.Instance.RegisterListenableInternal(listenable);
		}
	}

	public static void UnregisterListenable(IListenable listenable)
	{
		if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
		{
			SingletonComponent<VirtualAudioManager>.Instance.UnregisterListenableInternal(listenable);
		}
	}

	public static bool RegisterListener(VirtualAudioListener virtualListener)
	{
		if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
		{
			return SingletonComponent<VirtualAudioManager>.Instance.RegisterListenerInteral(virtualListener);
		}
		return false;
	}

	public static void UnregisterListener(VirtualAudioListener virtualListener)
	{
		if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
		{
			SingletonComponent<VirtualAudioManager>.Instance.UnregisterListenerInternal(virtualListener);
		}
	}

	public static void OnListenerEnable(VirtualAudioListener virtualListener)
	{
		if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
		{
			SingletonComponent<VirtualAudioManager>.Instance.OnListenerEnableInternal(virtualListener);
		}
	}

	public static void OnListenerDisable(VirtualAudioListener virtualListener)
	{
		if ((bool)SingletonComponent<VirtualAudioManager>.Instance)
		{
			SingletonComponent<VirtualAudioManager>.Instance.OnListenerDisableInternal(virtualListener);
		}
	}

	private void RegisterListenableInternal(IListenable virtualAudioComponent)
	{
		_listenables.Add(virtualAudioComponent);
		InitializeListenable(virtualAudioComponent, _virtualListeners);
	}

	private void InitializeListenable(IListenable virtualAudioComponent, IEnumerable<VirtualAudioListener> virtualListeners)
	{
		foreach (VirtualAudioListener virtualListener in virtualListeners)
		{
			virtualAudioComponent.AddListener(virtualListener);
		}
	}

	private void UnregisterListenableInternal(IListenable listenable)
	{
		_listenables.Remove(listenable);
	}

	private bool RegisterListenerInteral(VirtualAudioListener virtualListener)
	{
		if (_virtualListeners.Count >= _maxListeners)
		{
			return false;
		}
		_virtualListeners.Add(virtualListener);
		foreach (IListenable listenable in _listenables)
		{
			listenable.AddListener(virtualListener);
		}
		return true;
	}

	private void UnregisterListenerInternal(VirtualAudioListener virtualListener)
	{
		if (!_virtualListeners.Contains(virtualListener))
		{
			return;
		}
		_virtualListeners.Remove(virtualListener);
		foreach (IListenable listenable in _listenables)
		{
			listenable.RemoveListener(virtualListener);
		}
	}

	private void OnListenerEnableInternal(VirtualAudioListener virtualListener)
	{
		foreach (IListenable listenable in _listenables)
		{
			listenable.OnListenerEnabled(virtualListener);
		}
	}

	private void OnListenerDisableInternal(VirtualAudioListener virtualListener)
	{
		foreach (IListenable listenable in _listenables)
		{
			listenable.OnListenerDisabled(virtualListener);
		}
	}

	private static void SetGlobalMaxListeners(int maxListeners)
	{
	}

	public override void OnInitializeInPlayMode()
	{
		Debug.Log("VirtualAudioManager: InitializeInPlayMode");
		_listenables = new List<IListenable>();
		_virtualListeners = new List<VirtualAudioListener>();
		if (!GetComponent<AudioListener>())
		{
			base.gameObject.AddComponent<AudioListener>();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "ramjetaudio-microphone_real_icon.png");
	}
}
