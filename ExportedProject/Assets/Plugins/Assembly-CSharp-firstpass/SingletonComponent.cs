using RamjetAnvil.Unity.Utility;
using UnityEngine;

public class SingletonComponent<T> : MonoBehaviour, ISingletonComponent where T : MonoBehaviour, ISingletonComponent
{
	private static T _instance;

	private static bool _isQuitting;

	private static bool _isDestroyed;

	private bool _isInitialized;

	public static bool IsDestroyed
	{
		get
		{
			return _isDestroyed;
		}
	}

	public bool IsInitialized
	{
		get
		{
			return _isInitialized;
		}
	}

	public static T Instance
	{
		get
		{
			if (!_isDestroyed)
			{
				if (!(Object)_instance)
				{
					_instance = Object.FindObjectOfType(typeof(T)) as T;
					if (!(Object)_instance)
					{
						GameObject gameObject = new GameObject("_" + typeof(T).Name);
						_instance = gameObject.AddComponent<T>();
						Object.DontDestroyOnLoad(gameObject);
					}
					if (!(Object)_instance)
					{
						return null;
					}
				}
				if (!_instance.IsInitialized)
				{
					_instance.Initialize();
				}
				return _instance;
			}
			return null;
		}
	}

	static SingletonComponent()
	{
	}

	private void Awake()
	{
		Initialize();
	}

	public void Initialize()
	{
		if (!_isInitialized)
		{
			OnInitializeInPlayMode();
			_isInitialized = true;
		}
	}

	public virtual void OnInitializeInPlayMode()
	{
	}

	protected virtual void OnDestroy()
	{
		_isDestroyed = _isQuitting;
		_instance = null;
	}

	protected virtual void OnApplicationQuit()
	{
		_isQuitting = true;
	}

	private static void OnPlayModeStateChanged()
	{
		if (!Application.isPlaying)
		{
			_isQuitting = false;
			_isDestroyed = false;
		}
	}
}
